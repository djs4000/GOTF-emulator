using LaserTag.Simulator.Models;
using System.Text;
using System.Text.Json;

namespace LaserTag.Simulator.Services;

public class MatchSimulatorService : IDisposable
{
    private readonly HttpClient _httpClient = new();
    private System.Threading.Timer? _heartbeatTimer;
    private long _remainingTimeMs;
    private string _matchId;
    private List<PlayerDto> _players;
    private string _targetUrl;
    private int _updateFrequency;
    private CancellationTokenSource _cts;

    public event Action<string>? OnError;

    public MatchSimulatorService()
    {
        _matchId = $"sim-match-{Guid.NewGuid().ToString().Substring(0, 8)}";
        _players = new List<PlayerDto>();
        _targetUrl = string.Empty;
        _cts = new CancellationTokenSource();
    }

    public void Start(string targetUrl, int updateFrequency, int matchDuration, List<PlayerDto> players)
    {
        _targetUrl = targetUrl;
        _updateFrequency = updateFrequency;
        _remainingTimeMs = matchDuration * 1000;
        _players = players;
        
        _cts = new CancellationTokenSource();
        _heartbeatTimer = new System.Threading.Timer(HeartbeatCallback, null, 0, _updateFrequency);
    }

    public void Pause()
    {
        _heartbeatTimer?.Change(Timeout.Infinite, Timeout.Infinite);
    }

    public void Stop()
    {
        _heartbeatTimer?.Dispose();
        _heartbeatTimer = null;
        _cts.Cancel();
    }

    private async void HeartbeatCallback(object? state)
    {
        if (_cts.IsCancellationRequested) return;

        _remainingTimeMs -= _updateFrequency;
        if (_remainingTimeMs < 0) _remainingTimeMs = 0;

        var snapshot = new MatchSnapshotDto
        {
            Id = _matchId,
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            Status = _remainingTimeMs > 0 ? "Running" : "Completed",
            RemainingTimeMs = _remainingTimeMs,
            Players = _players
        };

        try
        {
            var json = JsonSerializer.Serialize(snapshot, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await _httpClient.PostAsync($"{_targetUrl}/match", content, _cts.Token);
        }
        catch (Exception ex)
        {
            OnError?.Invoke($"Error sending match heartbeat: {ex.Message}");
            // Don't crash, just log.
        }

        if (_remainingTimeMs <= 0)
        {
            Stop();
        }
    }

    public void Dispose()
    {
        Stop();
        _httpClient.Dispose();
    }
}