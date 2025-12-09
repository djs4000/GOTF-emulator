# Implementation Plan: Fix Data Sending Logic

**Branch**: `002-fix-data-sending` | **Date**: 2025-12-09 | **Spec**: [spec.md](D:\Documents\Code Project\GOTF-emulator\specs\002-fix-data-sending\spec.md)
**Input**: Feature specification from `specs/002-fix-data-sending/spec.md`

## Summary

This plan addresses several bugs and usability issues in the Laser Tag Simulator. The core objectives are to:
1.  Ensure that changes to the Target URL in the UI are immediately reflected in all running and future data-sending services.
2.  Change the default Target URL to `http://192.168.1.234:9055`.
3.  Provide explicit, independent UI controls to start and stop both the Match and Prop simulation services, removing the automatic start for the prop simulator.

The technical approach involves refactoring the `MainForm.cs` and the simulation services (`MatchSimulatorService.cs`, `PropSimulatorService.cs`) to dynamically read the URL and to be controlled by new UI buttons.

## Technical Context

**Language/Version**: C# / .NET 9.0
**Primary Dependencies**: Windows Forms (WinForms), System.Text.Json, HttpClient
**Storage**: N/A
**Testing**: Compilation Only
**Target Platform**: Windows 10/11
**Project Type**: Standalone Desktop Application
**Performance Goals**: N/A
**Constraints**: N/A
**Scale/Scope**: N/A

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- [X] **Technology Stack**: Is the proposed solution using C# and .NET 9.0? (Yes, all changes will be within the existing C#/.NET 9 project.)
- [X] **Verification**: Does the plan rely only on compilation for verification? (Yes, verification will be through successful compilation and manual testing of the UI.)
- [X] **Modularity**: Is the proposed project structure modular? (Yes, the changes will respect the existing modularity of UI and services.)

## Project Structure

This feature involves refactoring existing files and does not introduce new files to the overall structure. The following files will be modified:

```text
src/
└── LaserTag.Simulator/
    ├── MainForm.cs          # Logic for new UI controls, dynamic URL handling
    ├── MainForm.Designer.cs # Add new buttons, change default URL text
    └── services/
        ├── MatchSimulatorService.cs # Refactor to accept URL dynamically
        └── PropSimulatorService.cs  # Refactor to accept URL dynamically and remove auto-start
```

**Structure Decision**: No changes to the existing single-project structure are necessary.

## Complexity Tracking

> No complexity tracking needed as no constitutional violations are present.

| Violation | Why Needed | Simpler Alternative Rejected Because |
|-----------|------------|-------------------------------------|
| N/A       | N/A        | N/A                                 |