# Data Models: Enhanced Match Emulation with Countdown

**Date**: 2025-12-09
**Input**: [Feature Specification](spec.md)

This feature introduces modifications to existing data transfer objects to support new match states and countdown information, and defines how player data will be reset.

## Modified Entity: MatchSnapshotDto

The `MatchSnapshotDto` will be extended to include match state and countdown timer information.

```csharp
// In DtoModels.cs
public enum MatchState
{
    WaitingOnStart,
    Countdown,
    Running,
    Paused,
    Completed
}

public class MatchSnapshotDto
{
    // Existing properties...
    public string Id { get; set; }
    public long Timestamp { get; set; }
    public string Status { get; set; } // Will be deprecated or derived from MatchState
    public long RemainingTimeMs { get; set; }
    public List<PlayerDto> Players { get; set; }

    // New properties:
    public MatchState State { get; set; } // New enum property
    public long RemainingCountdownTimeMs { get; set; } // New property for countdown
}
```

## Modified Entity: PlayerDto

The `PlayerDto` itself does not change structurally, but its initial values will be used for resetting player statistics.

```csharp
// In DtoModels.cs (Existing structure)
public class PlayerDto
{
    public string Id { get; set; }
    public string Team { get; set; }
    public int Health { get; set; } = 100; // Default health
    public int Ammo { get; set; } = 60;   // Default ammo
    public string State { get; set; } = "Active";
    public int KillsCount { get; set; } = 0;
    public int Deaths { get; set; } = 0;
}
```

**Reset Logic for PlayerDto**:
When a match is reset to "WaitingOnStart", the player data for each player (`Health`, `Ammo`, `KillsCount`, `Deaths`) will be reverted to its initial generated state (100 Health, 60 Ammo, 0 Kills, 0 Deaths). This will be handled programmatically within the `MatchSimulatorService` and `MainForm`.
