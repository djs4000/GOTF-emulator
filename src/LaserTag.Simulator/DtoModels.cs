using System.Text.Json.Serialization;

namespace LaserTag.Simulator.Models;

public enum MatchState
{
    Idle,
    WaitingOnStart,
    Countdown,
    Running,
    WaitingOnFinalData,
    Completed,
    Cancelled,
    Paused
}

public class MatchSnapshotDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("timestamp")]
    public long Timestamp { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("state")]
    public MatchState State { get; set; } // New property

    [JsonPropertyName("remaining_time_ms")]
    public long RemainingTimeMs { get; set; }

    [JsonPropertyName("remaining_countdown_time_ms")] // New property
    public long RemainingCountdownTimeMs { get; set; }

    [JsonPropertyName("players")]
    public List<PlayerDto> Players { get; set; }

    [JsonPropertyName("isFinal")]
    public bool IsFinal { get; set; }
}

public class PlayerDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("team")]
    public string Team { get; set; }

    [JsonPropertyName("state")]
    public string State { get; set; }

    [JsonPropertyName("health")]
    public int Health { get; set; }

    [JsonPropertyName("kills_count")]
    public int KillsCount { get; set; }

    [JsonPropertyName("deaths")]
    public int Deaths { get; set; }

    [JsonPropertyName("ammo")]
    public int Ammo { get; set; }

    public PlayerDto Clone()
    {
        return (PlayerDto)MemberwiseClone();
    }
}

public class PropSnapshotDto
{
    [JsonPropertyName("timestamp")]
    public long Timestamp { get; set; }

    [JsonPropertyName("state")]
    public string State { get; set; }

    [JsonPropertyName("uptime_ms")]
    public long UptimeMs { get; set; }

    [JsonPropertyName("timer_ms")]
    public long TimerMs { get; set; }
}
