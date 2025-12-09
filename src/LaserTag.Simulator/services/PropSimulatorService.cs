using LaserTag.Simulator.Models;
using System.Text;
using System.Text.Json;

namespace LaserTag.Simulator.Services;

public class PropSimulatorService : IDisposable
{
    private readonly HttpClient _httpClient = new();
    private System.Threading.Timer? _heartbeatTimer;
    private PropSnapshotDto _currentPropState;
    private string _targetUrl;
    private int _updateFrequency;
    private CancellationTokenSource _cts;
    private DateTime _startTime;

    public event Action<string>? OnError;
    public event Action<long>? OnPropTimerUpdate;

    public PropSimulatorService()
    {
        _currentPropState = new PropSnapshotDto
        {
            State = "Idle",
            TimerMs = 0,
            UptimeMs = 0
        };
        _targetUrl = string.Empty;
        _cts = new CancellationTokenSource();
    }

    public void Start(string targetUrl, int updateFrequency)
    {
        _targetUrl = targetUrl;
        _updateFrequency = updateFrequency;
        _cts = new CancellationTokenSource();
        _startTime = DateTime.UtcNow;
        _heartbeatTimer = new System.Threading.Timer(HeartbeatCallback, null, 0, _updateFrequency);
    }

    public void Stop()
    {
        _heartbeatTimer?.Dispose();
        _heartbeatTimer = null;
        _cts.Cancel();
        _currentPropState.UptimeMs = 0; // Reset uptime on stop
    }

    public void UpdatePropState(string state, long timerMs = 0)
    {
        _currentPropState.State = state;
        _currentPropState.TimerMs = timerMs;
        if (state == "Arming")
        {
            // Reset timer when entering Arming state
            _currentPropState.TimerMs = 45000; // Example: 45 seconds to arm
        }
    }

    private async void HeartbeatCallback(object? state)
    {
        if (_cts.IsCancellationRequested) return;

        _currentPropState.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        _currentPropState.UptimeMs = (long)(DateTime.UtcNow - _startTime).TotalMilliseconds;

        if (_currentPropState.State == "Armed" && _currentPropState.TimerMs > 0)
        {
            _currentPropState.TimerMs -= _updateFrequency;
            if (_currentPropState.TimerMs < 0) _currentPropState.TimerMs = 0;
            OnPropTimerUpdate?.Invoke(_currentPropState.TimerMs);
        }
        else if (_currentPropState.State == "Armed" && _currentPropState.TimerMs == 0)
        {
            _currentPropState.State = "Detonated"; // Auto-detonate if timer runs out
            OnPropTimerUpdate?.Invoke(0);
            Stop();
        }

        try
        {
            var json = JsonSerializer.Serialize(_currentPropState, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await _httpClient.PostAsync($"{_targetUrl}/prop", content, _cts.Token);
        }
        catch (Exception ex)
        {
            OnError?.Invoke($"Error sending prop heartbeat: {ex.Message}");
            // Don't crash, just log.
        }
    }

    public void Dispose()
    {
        Stop();
        _httpClient.Dispose();
    }
}
