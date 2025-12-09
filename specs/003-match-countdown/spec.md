# Feature Specification: Enhanced Match Emulation with Countdown

**Feature Branch**: `003-match-countdown`  
**Created**: 2025-12-09
**Status**: Draft  
**Input**: User description: "need to add better funtionality to the match emulation. For one, need to have a configurable countdown time in addition to match time. Need a way to set the status to WaitingOnStart which will include the current state and the countdown timer. From there, need a way start the match, which would go to countdown status and the countdown timer would start. Once the countdown timer starts, it should go to running automatically and start the match timer. Will need controls to reset the match to waitingonstart (which should also reset player health and ammo and kills.)"

## User Scenarios & Verification *(mandatory)*

### User Story 1 - Configure and Display Countdown Timer (Priority: P1)

As a developer, I want to configure a pre-match countdown duration and have it prominently displayed in the UI, so that I can prepare for match initiation.

**Acceptance Scenarios**:
1. **Given** the application is running, **When** I input a value into the "Countdown Duration" field, **Then** the value is stored and reflected in the Match Simulation panel when the match state is "WaitingOnStart".
2. **Given** the match state is "WaitingOnStart", **Then** the UI displays the configurable countdown timer in an easy-to-read format (e.g., "Countdown: HH:MM:SS").

---

### User Story 2 - Match State Transitions (Priority: P1)

As a developer, I want to control the match lifecycle through "WaitingOnStart", "Countdown", and "Running" states, ensuring a smooth and automated transition from countdown to active match play.

**Acceptance Scenarios**:
1. **Given** the match is in the "WaitingOnStart" state, **When** I activate the "Start Match" control, **Then** the match state transitions to "Countdown".
2. **Given** the match is in the "Countdown" state, **When** the countdown timer reaches zero, **Then** the match state automatically transitions to "Running".
3. **Given** the match is in the "Running" state, **Then** the match duration timer starts decrementing.

---

### User Story 3 - Match Reset Functionality (Priority: P1)

As a developer, I want a "Reset Match" control that reverts the match to "WaitingOnStart" and resets all player statistics, so that I can easily prepare for a new match simulation.

**Acceptance Scenarios**:
1. **Given** a match is in any state other than "WaitingOnStart" (e.g., "Running", "Paused", "Completed"), and players have accumulated stats (health, ammo, kills, deaths), **When** I activate the "Reset Match" control, **Then** the match state returns to "WaitingOnStart".
2. **Given** a match is in any state other than "WaitingOnStart", and players have accumulated stats, **When** I activate the "Reset Match" control, **Then** all player health is reset to 100, ammo to 60, and kills/deaths to 0.

## Requirements *(mandatory)*

### Functional Requirements

-   **FR-001**: The UI MUST include a configurable numeric input field for "Countdown Duration (s)" in the Configuration panel.
-   **FR-002**: The `MatchSimulatorService` MUST support the following match states: "WaitingOnStart", "Countdown", "Running", "Paused", "Completed".
-   **FR-003**: The UI MUST display the current match state (e.g., "WaitingOnStart", "Countdown", "Running").
-   **FR-004**: When the match is in "WaitingOnStart" or "Countdown" states, the UI MUST display the remaining countdown time.
-   **FR-005**: A "Start Match" control MUST be available in the UI which, when triggered from "WaitingOnStart", initiates the countdown.
-   **FR-006**: The `MatchSimulatorService` MUST manage the countdown timer, decrementing it based on the configured "Update Frequency".
-   **FR-007**: Upon countdown completion (reaching zero), the `MatchSimulatorService` MUST automatically transition the match state from "Countdown" to "Running" and begin the match duration timer.
-   **FR-008**: A "Reset Match" control MUST be available in the UI.
-   **FR-009**: Activating the "Reset Match" control MUST set the match state to "WaitingOnStart".
-   **FR-010**: Activating the "Reset Match" control MUST reset all player health to 100, ammo to 60, and kills/deaths to 0, and refresh the player display.
-   **FR-011**: The `MatchSnapshotDto` MUST be updated to include a `MatchState` property (enum) reflecting "WaitingOnStart", "Countdown", "Running", "Paused", "Completed".
-   **FR-012**: The `MatchSnapshotDto` MUST include `RemainingCountdownTimeMs` when the match state is "WaitingOnStart" or "Countdown".

### Key Entities *(include if feature involves data)*

-   **MatchSnapshotDto**:
    *   Add `MatchState` (enum: WaitingOnStart, Countdown, Running, Paused, Completed)
    *   Add `RemainingCountdownTimeMs` (long)
-   **PlayerDto**:
    *   Values (`Health`, `Ammo`, `KillsCount`, `Deaths`) need to be reset to initial state.

## Success Criteria *(mandatory)*

### Measurable Outcomes

-   **SC-001**: Users can successfully configure a pre-match countdown duration.
-   **SC-002**: Match state transitions ("WaitingOnStart" -> "Countdown" -> "Running") occur automatically and visually without user intervention after initial "Start Match" trigger.
-   **SC-003**: A full match lifecycle, including countdown and game time, can be simulated and reset multiple times without error.
-   **SC-004**: Player statistics consistently reset to their initial values (Health: 100, Ammo: 60, Kills: 0, Deaths: 0) upon match reset.

### Edge Cases
-   What happens if "Start Match" is pressed while already running? (Should be ignored or disabled).
-   What happens if "Reset Match" is pressed while a countdown is active? (Should immediately transition to "WaitingOnStart" and reset stats).
-   Validation for countdown duration input (e.g., non-negative integer).