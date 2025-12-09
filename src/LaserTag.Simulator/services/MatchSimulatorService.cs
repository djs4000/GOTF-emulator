using LaserTag.Simulator.Models;
using System.Text;
using System.Text.Json;

namespace LaserTag.Simulator.Services;

    public class MatchSimulatorService : IDisposable
    {
        private readonly HttpClient _httpClient = new();
        private System.Threading.Timer? _heartbeatTimer;
        private long _remainingTimeMs;
        private long _remainingCountdownTimeMs;
        private MatchState _matchState;
            private string _matchId;
            private List<PlayerDto> _players;
            private List<PlayerDto> _initialPlayers; // To store initial roster for reset
            private Func<string> _getTargetUrl;
            private int _updateFrequency;
            private CancellationTokenSource _cts;
        
            public event Action<string>? OnError;
            public event Action<MatchState, long, long>? OnMatchStateChanged; // Added remainingMatchTimeMs
            public event Action<long>? OnCountdownUpdate;
        
            public MatchSimulatorService()
            {
                _matchId = $"sim-match-{Guid.NewGuid().ToString().Substring(0, 8)}";
                _players = new List<PlayerDto>();
                _initialPlayers = new List<PlayerDto>();
                _getTargetUrl = () => string.Empty;
                _cts = new CancellationTokenSource();
                            _matchState = MatchState.Idle; // Default to Idle
                            _remainingCountdownTimeMs = 0;
                        }
                
                        public void SetMatchState(MatchState newState)
                        {
                            if (_matchState == newState) return;
                
                            _matchState = newState;
                            OnMatchStateChanged?.Invoke(_matchState, _remainingCountdownTimeMs, _remainingTimeMs);
                
                            if (_matchState == MatchState.Idle)
                            {
                                // Stop any running timers when going to Idle
                                _heartbeatTimer?.Dispose();
                                _heartbeatTimer = null;
                                _cts.Cancel();
                                _remainingTimeMs = 0;
                                _remainingCountdownTimeMs = 0;
                            }
                            else if (_matchState == MatchState.WaitingOnStart)
                            {
                                // Start sending heartbeats immediately from WaitingOnStart
                                _cts = new CancellationTokenSource();
                                _heartbeatTimer = new System.Threading.Timer(HeartbeatCallback, null, 0, _updateFrequency);
                            }
                        }
                        
                        public void StartMatchCountdown(Func<string> getTargetUrl, int updateFrequency, int matchDuration, int countdownDuration, List<PlayerDto> players)
                        {
                            if (_matchState != MatchState.WaitingOnStart)
                            {
                                OnError?.Invoke($"Cannot start countdown from {_matchState} state.");
                                return;
                            }
                        
                            _getTargetUrl = getTargetUrl;
                            _updateFrequency = updateFrequency;
                            _remainingTimeMs = matchDuration * 1000;
                            _remainingCountdownTimeMs = countdownDuration * 1000;
                            _initialPlayers = players.Select(p => p.Clone()).ToList(); // Store a clone for reset
                            _players = players.Select(p => p.Clone()).ToList(); // Work with a clone
                        
                            _matchState = MatchState.Countdown;
                            OnMatchStateChanged?.Invoke(_matchState, _remainingCountdownTimeMs, _remainingTimeMs);
                            // Timer is already running from WaitingOnStart
                        }
                        
                        public void Pause()
                        {
                            if (_matchState == MatchState.Running)
                            {
                                _matchState = MatchState.Paused;
                                _heartbeatTimer?.Change(Timeout.Infinite, Timeout.Infinite);
                                OnMatchStateChanged?.Invoke(_matchState, _remainingCountdownTimeMs, _remainingTimeMs);
                            }
                        }
                        
                        public void Start() // Renamed from original Start, now resumes paused match
                        {
                            if (_matchState == MatchState.Paused)
                            {
                                _matchState = MatchState.Running;
                                _heartbeatTimer?.Change(0, _updateFrequency);
                                OnMatchStateChanged?.Invoke(_matchState, _remainingCountdownTimeMs, _remainingTimeMs);
                            }
                        }
                        
                        public void Stop()
                        {
                            _heartbeatTimer?.Dispose();
                            _heartbeatTimer = null;
                            _cts.Cancel();
                            _matchState = MatchState.Completed;
                            _remainingTimeMs = 0;
                            _remainingCountdownTimeMs = 0;
                            OnMatchStateChanged?.Invoke(_matchState, _remainingCountdownTimeMs, _remainingTimeMs);
                        }
                        
                        public void ResetMatch()
                        {
                            Stop(); // Ensure any running timer is stopped
                            _matchState = MatchState.WaitingOnStart;
                            _remainingCountdownTimeMs = 0;
                            _remainingTimeMs = 0;
                            // Reset players to their initial state
                            _players = _initialPlayers.Select(p => p.Clone()).ToList();
                            OnMatchStateChanged?.Invoke(_matchState, _remainingCountdownTimeMs, _remainingTimeMs);
                        }
                        
                        private async void HeartbeatCallback(object? state)
                        {
                            if (_cts.IsCancellationRequested || _matchState == MatchState.Idle) return;
                        
                            if (_matchState == MatchState.Countdown)
                            {
                                _remainingCountdownTimeMs -= _updateFrequency;
                                if (_remainingCountdownTimeMs <= 0)
                                {
                                    _remainingCountdownTimeMs = 0;
                                    _matchState = MatchState.Running;
                                    OnMatchStateChanged?.Invoke(_matchState, _remainingCountdownTimeMs, _remainingTimeMs);
                                    OnCountdownUpdate?.Invoke(0);
                                }
                                else
                                {
                                    OnMatchStateChanged?.Invoke(_matchState, _remainingCountdownTimeMs, _remainingTimeMs);
                                    OnCountdownUpdate?.Invoke(_remainingCountdownTimeMs);
                                }
                            }
                            else if (_matchState == MatchState.Running)
                            {
                                _remainingTimeMs -= _updateFrequency;
                                if (_remainingTimeMs < 0) _remainingTimeMs = 0;
                        
                                OnMatchStateChanged?.Invoke(_matchState, _remainingCountdownTimeMs, _remainingTimeMs);
                            }
                        
                            var snapshot = new MatchSnapshotDto
                            {
                                Id = _matchId,
                                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                                Status = _matchState.ToString(), // Deprecated, but now consistent with State
                                State = _matchState, // New State
                                RemainingTimeMs = _remainingTimeMs,
                                RemainingCountdownTimeMs = _remainingCountdownTimeMs,
                                Players = _players
                            };
                            try
                            {
                                var targetUrl = _getTargetUrl();
                                var json = JsonSerializer.Serialize(snapshot, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                                var content = new StringContent(json, Encoding.UTF8, "application/json");
                                await _httpClient.PostAsync($"{targetUrl}/match", content, _cts.Token);
                            }
                            catch (Exception ex)
                            {
                                OnError?.Invoke($"Error sending match heartbeat: {ex.Message}");
                                // Don't crash, just log.
                            }
                
                            if (_matchState == MatchState.Running && _remainingTimeMs <= 0)
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