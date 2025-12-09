# Implementation Plan: Enhanced Match Emulation with Countdown

**Branch**: `003-match-countdown` | **Date**: 2025-12-09 | **Spec**: [spec.md](D:\Documents\Code Project\GOTF-emulator\specs\003-match-countdown\spec.md)
**Input**: Feature specification from `specs/003-match-countdown/spec.md`

## Summary

This plan outlines the implementation of enhanced match emulation features for the Laser Tag Simulator. Key objectives include:
1.  **Configurable Countdown:** Introduce a configurable pre-match countdown timer.
2.  **Match State Management:** Implement new match states ("WaitingOnStart", "Countdown", "Running") with automated transitions.
3.  **Match Reset:** Provide functionality to reset the match to a "WaitingOnStart" state, including resetting player statistics.

The technical approach will involve modifying the `MatchSimulatorService` to manage these new states and timers, updating the UI (`MainForm.cs`, `MainForm.Designer.cs`) to display and control the new features, and extending the `MatchSnapshotDto` to reflect the new state information.

## Technical Context

**Language/Version**: C# / .NET 9.0
**Primary Dependencies**: Windows Forms (WinForms), System.Text.Json, HttpClient
**Storage**: N/A
**Testing**: Compilation Only
**Target Platform**: Windows 10/11
**Project Type**: Standalone Desktop Application
**Performance Goals**: N/A
**Constraints**: N/A
**Scale/Scope**: This feature primarily extends the match simulation logic and UI. No significant changes to overall performance or architectural patterns are anticipated.

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- [X] **Technology Stack**: Is the proposed solution using C# and .NET 9.0? (Yes, all changes will be within the existing C#/.NET 9 project.)
- [X] **Verification**: Does the plan rely only on compilation for verification? (Yes, verification will be through successful compilation and manual testing of the UI.)
- [X] **Modularity**: Is the proposed project structure modular? (Yes, the changes will respect the existing modularity of UI, services, and DTOs.)

## Project Structure

This feature primarily modifies existing files and introduces minor additions to DTOs.

```text
src/
└── LaserTag.Simulator/
    ├── DtoModels.cs             # Add MatchState enum, RemainingCountdownTimeMs to MatchSnapshotDto
    ├── MainForm.cs              # UI logic for new controls, state display, and reset
    ├── MainForm.Designer.cs     # Add countdown input, reset button, match state display
    └── services/
        └── MatchSimulatorService.cs # State management, countdown timer logic, player stat reset
```

**Structure Decision**: No changes to the existing single-project structure are necessary.

## Complexity Tracking

> No complexity tracking needed as no constitutional violations are present.

| Violation | Why Needed | Simpler Alternative Rejected Because |
|-----------|------------|-------------------------------------|
| N/A       | N/A        | N/A                                 |