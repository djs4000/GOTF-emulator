# Data Models: Laser Tag Simulator

**Date**: 2025-12-09
**Input**: [Feature Specification](spec.md)

This document defines the data entities used in the Laser Tag Simulator. These models correspond to the JSON payloads that will be sent to the target application.

---

### 1. MatchSnapshotDto

Represents the state of the entire match at a point in time. This is the payload for the `/match` endpoint.

**Fields**:

| Name                | Type           | Description                                                 | Constraints      |
|---------------------|----------------|-------------------------------------------------------------|------------------|
| `id`                | string (GUID)  | A unique identifier for the match instance.                 | Required         |
| `timestamp`         | long (Unix ms) | The UTC timestamp when the snapshot was generated.          | Required         |
| `status`            | string         | The current status of the match (e.g., "Running", "Completed"). | Required         |
| `remaining_time_ms` | long           | The time remaining in the match, in milliseconds.           | Required         |
| `players`           | Array<PlayerDto> | An array of objects representing each player in the match.  | Required         |

---

### 2. PlayerDto

Represents the state of a single player.

**Fields**:

| Name          | Type    | Description                                       | Constraints      |
|---------------|---------|---------------------------------------------------|------------------|
| `id`          | string  | A unique identifier for the player.               | Required         |
| `team`        | string  | The name of the team the player belongs to.       | Required         |
| `state`       | string  | The player's current state (e.g., "Active", "Dead"). | Required         |
| `health`      | int     | The player's current health points.               | Required, >= 0   |
| `kills_count` | int     | The number of kills the player has.               | Required, >= 0   |
| `deaths`      | int     | The number of times the player has died.          | Required, >= 0   |
| `ammo`        | int     | The player's current ammo count.                  | Required, >= 0   |

---

### 3. PropSnapshotDto

Represents the state of the interactive bomb prop. This is the payload for the `/prop` endpoint.

**Fields**:

| Name        | Type           | Description                                                              | Constraints      |
|-------------|----------------|--------------------------------------------------------------------------|------------------|
| `timestamp` | long (Unix ms) | The UTC timestamp when the snapshot was generated.                       | Required         |
| `state`     | string         | The prop's current state (e.g., "Idle", "Armed", "Detonated").         | Required         |
| `uptime_ms` | long           | The total uptime of the prop in milliseconds.                            | Required         |
| `timer_ms`  | long           | The time remaining on the prop's internal timer (e.g., bomb countdown). | Required         |
