# Tasks: Enhanced Match Emulation with Countdown

**Input**: Design documents from `specs/003-match-countdown/`

**Verification**: Verification is by compilation only, as per the project constitution.

**Organization**: Tasks are grouped by user story and foundational requirements to enable independent implementation and verification.

---

## Phase 1: Foundational Setup

**Goal**: Establish the basic structures (enums, DTO properties, core UI elements) required for all match countdown functionality.

- [X] T001 Define `MatchState` enum (`WaitingOnStart`, `Countdown`, `Running`, `Paused`, `Completed`) in `src/LaserTag.Simulator/DtoModels.cs`.
- [X] T002 Add `State` (type `MatchState`) and `RemainingCountdownTimeMs` (type `long`) properties to `MatchSnapshotDto` in `src/LaserTag.Simulator/DtoModels.cs`.
- [X] T003 Add a configurable `NumericUpDown` for "Countdown Duration (s)" in the Match Controls group box in `src/LaserTag.Simulator/MainForm.Designer.cs`.
- [X] T004 Add UI controls for displaying match state and countdown timer (`MainForm.Designer.cs`).
- [X] T005 Add a "Reset Match" button to the Match Controls group box in `src/LaserTag.Simulator/MainForm.Designer.cs`.

---

## Phase 2: User Story 1 - Configure and Display Countdown Timer (P1)

**Goal**: Enable configuration of the countdown timer and ensure its display in the UI.

- [X] T006 Implement logic in `src/LaserTag.Simulator/MainForm.cs` to read the "Countdown Duration" from the UI and store it in a private field.
- [X] T007 Implement UI update logic in `src/LaserTag.Simulator/MainForm.cs` to display the current match state (e.g., "Waiting on Start") and the remaining countdown time. This will involve updating event handlers and possibly introducing a new timer for UI updates.

---

## Phase 3: User Story 2 - Match State Transitions (P1)

**Goal**: Implement the match state machine, including countdown and automatic transition to running.

- [X] T008 [P] Modify `src/LaserTag.Simulator/services/MatchSimulatorService.cs` to manage an internal `MatchState` variable and `_remainingCountdownTimeMs`.
- [X] T009 [P] Update `Start` method in `src/LaserTag.Simulator/services/MatchSimulatorService.cs` to set the initial state to `Countdown` (after the "Start Match" button is pressed) and initialize the countdown timer.
- [X] T010 [P] Modify the `HeartbeatCallback` in `src/LaserTag.Simulator/services/MatchSimulatorService.cs` to:
    *   Decrement `_remainingCountdownTimeMs` when in `Countdown` state.
    *   Transition state from `Countdown` to `Running` when `_remainingCountdownTimeMs` reaches zero, and start the `_remainingTimeMs` (match duration timer).
    *   Include `State` and `RemainingCountdownTimeMs` in the `MatchSnapshotDto` sent.
- [X] T011 Update `StartMatchButton_Click` in `src/LaserTag.Simulator/MainForm.cs` to set the `MatchSimulatorService` into a `Countdown` state and start the process.

---

## Phase 4: User Story 3 - Match Reset Functionality (P1)

**Goal**: Provide a control to reset the match and player statistics.

- [X] T012 Add an event handler for the "Reset Match" button in `src/LaserTag.Simulator/MainForm.cs`.
- [X] T013 Implement a `ResetMatch()` method in `src/LaserTag.Simulator/services/MatchSimulatorService.cs` that sets the state to `WaitingOnStart`, stops all timers, and stores the initial roster.
- [X] T014 Modify `src/LaserTag.Simulator/MainForm.cs` to iterate through `_playerBindingSource.DataSource` players and reset their `Health`, `Ammo`, `KillsCount`, and `Deaths` to initial values, then refresh the DataGridView, when "Reset Match" is pressed.
- [X] T015 Call `ResetMatch()` from the "Reset Match" button event handler in `src/LaserTag.Simulator/MainForm.cs`.

---

## Phase 5: Polish & Cross-Cutting Concerns

**Goal**: Refine UI interactions and ensure robustness.

- [X] T016 Implement UI control enabling/disabling logic in `src/LaserTag.Simulator/MainForm.cs` based on the current match state (e.g., disable "Start Match" if already running, enable "Reset Match" always).
- [X] T017 Review and refactor `src/LaserTag.Simulator/services/MatchSimulatorService.cs` for clarity, error handling, and maintainability.
- [ ] T018 Final testing and verification of all new match emulation features.

---

## Dependencies & Execution Order

-   Phase 1 (Foundational Setup) must be completed before other phases.
-   Within Phase 3 (User Story 2), tasks T008, T009, and T010 can be done in parallel, but T011 depends on them.
-   Phases 2, 3, and 4 can largely be developed in parallel after Phase 1, but there are interdependencies between them regarding `MatchSimulatorService` and `MainForm` updates. The order presented is a logical flow.
-   Phase 5 (Polish) should be performed after all other functional tasks are complete.
