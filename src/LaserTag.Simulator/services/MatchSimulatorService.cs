using LaserTag.Simulator.Models;
using System.Text;
using System.Text.Json;
using System.Linq;

namespace LaserTag.Simulator.Services;

public class MatchSimulatorService : IDisposable
{
    private readonly HttpClient _httpClient = new();
    private System.Threading.Timer? _heartbeatTimer;
    private CancellationTokenSource _cts;
    private long _remainingTimeMs;
    private long _remainingCountdownTimeMs;
    private long _matchDurationMs;
    private long _countdownDurationMs;
    private MatchState _matchState;
    private string _matchId;
    private List<PlayerDto> _players;
    private List<PlayerDto> _initialPlayers;
    private Func<string> _getTargetUrl;
    private int _updateFrequency;

    public event Action<string>? OnError;
    public event Action<MatchState, long, long>? OnMatchStateChanged;
    public event Action<long>? OnCountdownUpdate;

    public MatchSimulatorService()
    {
        _matchId = $"sim-match-{Guid.NewGuid().ToString()[..8]}";
        _players = new List<PlayerDto>();
        _initialPlayers = new List<PlayerDto>();
        _getTargetUrl = () => string.Empty;
        _cts = new CancellationTokenSource();
        _matchState = MatchState.Idle;
    }

    public void ConfigureMatch(
        Func<string> getTargetUrl,
        int updateFrequency,
        int matchDurationSeconds,
        int countdownDurationSeconds,
        List<PlayerDto> players)
    {
        _getTargetUrl = getTargetUrl;
        _updateFrequency = Math.Max(1, updateFrequency);
        _matchDurationMs = matchDurationSeconds * 1000L;
        _countdownDurationMs = countdownDurationSeconds * 1000L;
        _remainingCountdownTimeMs = _countdownDurationMs;
        _remainingTimeMs = _countdownDurationMs;
        _initialPlayers = players.Select(p => p.Clone()).ToList();
        _players = players;

        TransitionToWaitingOnStart();
    }

    public void BeginCountdown()
    {
        if (_matchState != MatchState.WaitingOnStart)
        {
            OnError?.Invoke($"Cannot start countdown from {_matchState} state.");
            return;
        }

        _matchState = MatchState.Countdown;
        _remainingCountdownTimeMs = _countdownDurationMs;
        OnMatchStateChanged?.Invoke(_matchState, _remainingCountdownTimeMs, _matchDurationMs);
    }

    public async Task CompleteMatchAsync()
    {
        if (_matchState == MatchState.Completed)
        {
            return;
        }

        StopHeartbeatTimer();

        _matchState = MatchState.Completed;
        _remainingTimeMs = 0;
        _remainingCountdownTimeMs = 0;
        OnMatchStateChanged?.Invoke(_matchState, _remainingCountdownTimeMs, _remainingTimeMs);

        try
        {
            await SendSnapshotAsync(isFinal: true, CancellationToken.None);
        }
        catch (Exception ex)
        {
            OnError?.Invoke($"Error sending final match snapshot: {ex.Message}");
        }
        finally
        {
            _cts.Cancel();
        }
    }

    public void ResetMatch()
    {
        StopHeartbeatTimer();
        _cts.Cancel();
        _cts = new CancellationTokenSource();
        ResetPlayerStateToInitial();
        _remainingCountdownTimeMs = _countdownDurationMs;
        _remainingTimeMs = _countdownDurationMs;
        TransitionToWaitingOnStart();
    }

    private void ResetPlayerStateToInitial()
    {
        foreach (var player in _players)
        {
            var initial = _initialPlayers.FirstOrDefault(p => p.Id == player.Id);
            if (initial == null)
            {
                continue;
            }

            player.Team = initial.Team;
            player.State = initial.State;
            player.Health = initial.Health;
            player.Ammo = initial.Ammo;
            player.KillsCount = initial.KillsCount;
            player.Deaths = initial.Deaths;
        }
    }

    private void TransitionToWaitingOnStart()
    {
        StopHeartbeatTimer();
        _cts.Cancel();
        _cts = new CancellationTokenSource();
        _matchState = MatchState.WaitingOnStart;
        StartHeartbeat();
        OnMatchStateChanged?.Invoke(_matchState, _remainingCountdownTimeMs, _countdownDurationMs);
        _ = SendSnapshotAsync(isFinal: false, CancellationToken.None);
    }

    private void StartHeartbeat()
    {
        _heartbeatTimer = new System.Threading.Timer(HeartbeatCallback, null, 0, _updateFrequency);
    }

    private void StopHeartbeatTimer()
    {
        _heartbeatTimer?.Dispose();
        _heartbeatTimer = null;
    }

    private async void HeartbeatCallback(object? state)
    {
        if (_cts.IsCancellationRequested || _matchState == MatchState.Completed || _matchState == MatchState.Idle)
        {
            return;
        }

        switch (_matchState)
        {
            case MatchState.WaitingOnStart:
                _remainingCountdownTimeMs = _countdownDurationMs;
                _remainingTimeMs = _countdownDurationMs;
                break;
            case MatchState.Countdown:
                _remainingCountdownTimeMs = Math.Max(0, _remainingCountdownTimeMs - _updateFrequency);
                if (_remainingCountdownTimeMs == 0)
                {
                    _matchState = MatchState.Running;
                    _remainingTimeMs = _matchDurationMs;
                    OnCountdownUpdate?.Invoke(0);
                }
                else
                {
                    OnCountdownUpdate?.Invoke(_remainingCountdownTimeMs);
                }
                break;
            case MatchState.Running:
                _remainingTimeMs = Math.Max(0, _remainingTimeMs - _updateFrequency);
                if (_remainingTimeMs == 0)
                {
                    _matchState = MatchState.WaitingOnFinalData;
                }
                break;
            case MatchState.WaitingOnFinalData:
                _remainingTimeMs = 0;
                _remainingCountdownTimeMs = 0;
                break;
        }

        OnMatchStateChanged?.Invoke(_matchState, _remainingCountdownTimeMs, GetDisplayedRemainingTime());

        try
        {
            await SendSnapshotAsync(isFinal: false);
        }
        catch (Exception ex)
        {
            OnError?.Invoke($"Error sending match heartbeat: {ex.Message}");
        }
    }

    private long GetDisplayedRemainingTime()
    {
        return _matchState switch
        {
            MatchState.WaitingOnStart or MatchState.Countdown => _remainingCountdownTimeMs,
            _ => _remainingTimeMs
        };
    }

    private async Task SendSnapshotAsync(bool isFinal, CancellationToken? cancellationToken = null)
    {
        var snapshot = new MatchSnapshotDto
        {
            Id = _matchId,
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            Status = _matchState.ToString(),
            State = _matchState,
            RemainingTimeMs = GetDisplayedRemainingTime(),
            RemainingCountdownTimeMs = _matchState == MatchState.WaitingOnStart ? _countdownDurationMs : _remainingCountdownTimeMs,
            Players = _players,
            IsFinal = isFinal
        };

        var json = JsonSerializer.Serialize(snapshot, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        await _httpClient.PostAsync($"{_getTargetUrl()}/match", content, cancellationToken ?? _cts.Token);
    }

    public void Dispose()
    {
        StopHeartbeatTimer();
        _cts.Cancel();
        _httpClient.Dispose();
    }
}
