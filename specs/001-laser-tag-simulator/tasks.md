# Tasks: Laser Tag Simulator

**Input**: Design documents from `specs/001-laser-tag-simulator/`

**Verification**: Verification is by compilation only, as per the project constitution.

**Organization**: Tasks are grouped by user story to enable independent implementation and verification.

---

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Initialize the solution and project structure.

- [ ] T001 Create the solution file `GOTF-Emulator.sln`.
- [ ] T002 [P] Create the WinForms project file `src/LaserTag.Simulator/LaserTag.Simulator.csproj` and add it to the solution.
- [ ] T003 [P] Create the application entry point file `src/LaserTag.Simulator/Program.cs`.

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Create the core data models and the basic UI shell that all user stories depend on.

- [ ] T004 Define `MatchSnapshotDto`, `PlayerDto`, and `PropSnapshotDto` in `src/LaserTag.Simulator/DtoModels.cs`.
- [ ] T005 [P] Design the main application window UI in `src/LaserTag.Simulator/MainForm.Designer.cs`, adding all required controls (text boxes, buttons, DataGridView, etc.).
- [ ] T006 Initialize UI component properties and basic event handler stubs in `src/LaserTag.Simulator/MainForm.cs`.

---

## Phase 3: User Story 1 - Configure Simulation (Priority: P1)

**Goal**: Allow a developer to configure all simulation parameters through the UI.
**Verification**: The application compiles, and the values entered in the UI are correctly read by the application's logic.

- [ ] T007 [P] [US1] Implement the "Generate Roster" button logic in `src/LaserTag.Simulator/MainForm.cs` to create player data and populate the `DataGridView`.
- [ ] T008 [US1] Add logic in `src/LaserTag.Simulator/MainForm.cs` to hold and update configuration data from the various UI text and number fields.

---

## Phase 4: User Story 2 - Simulate a Match Lifecycle (Priority: P2)

**Goal**: Implement the core `/match` heartbeat simulation.
**Verification**: The application compiles and sends valid HTTP POST requests to the `/match` endpoint when the simulation is active.

- [ ] T009 [P] [US2] Create the service file `src/LaserTag.Simulator/services/MatchSimulatorService.cs`.
- [ ] T010 [US2] Implement the timer-based heartbeat loop (Start, Pause, Stop logic) within `src/LaserTag.Simulator/services/MatchSimulatorService.cs`.
- [ ] T011 [US2] Implement the `HttpClient` logic in `src/LaserTag.Simulator/services/MatchSimulatorService.cs` to send the `MatchSnapshotDto` to the `/match` endpoint.
- [ ] T012 [US2] Wire the "Start Match", "Pause", and "Stop/Reset" buttons in `src/LaserTag.Simulator/MainForm.cs` to control the `MatchSimulatorService`.

---

## Phase 5: User Story 3 - Simulate Live Player Events (Priority: P3)

**Goal**: Allow real-time interaction with player data.
**Verification**: The application compiles, and UI interactions with the player grid are reflected in subsequent `/match` heartbeats.

- [ ] T013 [P] [US3] Add a context menu to the `DataGridView` in `src/LaserTag.Simulator/MainForm.cs` for player actions.
- [ ] T014 [US3] Implement the logic for "Shoot Player", "Kill Player", and "Register Kill" in `src/LaserTag.Simulator/MainForm.cs` to modify the underlying player data source.
- [ ] T015 [US3] Ensure the `MatchSimulatorService` accesses the modified player data for each heartbeat, sending the most current state.

---

## Phase 6: User Story 4 - Simulate an Interactive Prop (Priority: P4)

**Goal**: Implement the independent `/prop` heartbeat simulation.
**Verification**: The application compiles and sends valid HTTP POST requests to the `/prop` endpoint.

- [ ] T016 [P] [US4] Create the service file `src/LaserTag.Simulator/services/PropSimulatorService.cs`.
- [ ] T017 [US4] Implement an independent timer-based heartbeat loop within `src/LaserTag.Simulator/services/PropSimulatorService.cs`.
- [ ] T018 [US4] Implement `HttpClient` logic in `src/LaserTag.Simulator/services/PropSimulatorService.cs` to send the `PropSnapshotDto` to the `/prop` endpoint.
- [ ] T019 [US4] Wire the prop state UI controls in `src/LaserTag.Simulator/MainForm.cs` to control the `PropSimulatorService`.

---

## Phase 7: Polish & Cross-Cutting Concerns

**Purpose**: Finalize the application with error handling and logging.

- [ ] T020 Add a "Console" `TextBox` to the UI in `src/LaserTag.Simulator/MainForm.Designer.cs`.
- [ ] T021 Implement graceful `HttpClient` error handling in both `MatchSimulatorService.cs` and `PropSimulatorService.cs` to prevent crashes.
- [ ] T022 Implement logging to the console `TextBox` in `src/LaserTag.Simulator/MainForm.cs` to display status messages and errors from the services.
- [ ] T023 Final review and code cleanup across all files.

---

## Dependencies & Execution Order

- **Setup (Phase 1)** must be completed first.
- **Foundational (Phase 2)** depends on Setup and blocks all user stories.
- **User Stories (Phase 3-6)** can be implemented sequentially in priority order (US1 → US2 → US3 → US4).
- **Polish (Phase 7)** should be done last.

Within each user story, `[P]` tasks can be started in parallel, but implementation should generally follow: Service creation → Logic implementation → UI wiring.
