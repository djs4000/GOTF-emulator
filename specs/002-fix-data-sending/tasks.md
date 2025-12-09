# Tasks: Fix Data Sending Logic

**Input**: Design documents from `specs/002-fix-data-sending/`

**Verification**: Verification is by compilation only, as per the project constitution.

**Organization**: Tasks are grouped by user story to enable independent implementation and verification.

---

## Phase 1: User Story 1 - Update Data-Sending URL (P1)

**Goal**: Ensure the simulation services use the most current URL from the UI.
**Verification**: The application compiles, and changing the URL in the textbox while a simulation is running correctly redirects subsequent HTTP requests.

- [X] T001 [P] [US1] Refactor the `Start` method in `src/LaserTag.Simulator/services/MatchSimulatorService.cs` to accept a `Func<string>` that provides the target URL dynamically.
- [X] T002 [P] [US1] Refactor the `Start` method in `src/LaserTag.Simulator/services/PropSimulatorService.cs` to accept a `Func<string>` that provides the target URL dynamically.
- [X] T003 [US1] Update the `StartMatchButton_Click` and `StartPropButton_Click` handlers in `src/LaserTag.Simulator/MainForm.cs` to pass a lambda expression (`() => _targetUrl`) to the services.

---

## Phase 2: User Story 2 - Control Data-Sending Services (P2)

**Goal**: Provide explicit start/stop controls for all services.
**Verification**: The application compiles, the Prop simulator no longer starts automatically, and it can be started and stopped independently using new UI buttons.

- [X] T004 [P] [US2] Add "Start Prop" and "Stop Prop" buttons to the `propGroupBox` in `src/LaserTag.Simulator/MainForm.Designer.cs`.
- [X] T005 [US2] Remove the automatic start of `_propSimulatorService` from the constructor in `src/LaserTag.Simulator/MainForm.cs`.
- [X] T006 [US2] Implement the `Click` event handlers for the new "Start Prop" and "Stop Prop" buttons in `src/LaserTag.Simulator/MainForm.cs`.

---

## Phase 3: User Story 3 - Set Default URL (P3)

**Goal**: Set the correct default URL in the configuration UI.
**Verification**: The application compiles and, on launch, the Target URL textbox is pre-filled with `http://192.168.1.234:9055`.

- [X] T007 [P] [US3] Change the default `Text` property of the `targetUrlTextBox` in `src/LaserTag.Simulator/MainForm.Designer.cs` to "http://192.168.1.234:9055".

---

## Phase 4: Polish & Cross-Cutting Concerns

**Purpose**: Finalize the changes.

- [X] T008 Final review and code cleanup across all modified files.

---

## Dependencies & Execution Order

- The tasks can be executed by user story.
- Within User Story 1, tasks T001 and T002 can be done in parallel, but T003 depends on them.
- User Stories 2 and 3 have tasks that can be done in parallel with User Story 1.
- The final polish phase should be done last.
