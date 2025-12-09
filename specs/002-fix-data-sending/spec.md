# Feature Specification: Fix Data Sending Logic

**Feature Branch**: `002-fix-data-sending`  
**Created**: 2025-12-09
**Status**: Draft  
**Input**: User description: "The basic framework is not implemented. Need to add a way to start and stop sending data. When starting to send data it should also update the url it's sending to. Currently, it seems changing the url has no impact on where the data is being sent. Make the default url "http://192.168.1.234:9055""

## User Scenarios & Verification *(mandatory)*

### User Story 1 - Update Data-Sending URL (Priority: P1)

As a developer, I want to change the target URL in the UI and have the application immediately use the new address for sending data.

**Why this priority**: The simulator is not useful if it cannot be pointed to the correct target application.

**Acceptance Scenarios**:
1. **Given** a simulation is stopped, **When** I change the URL in the textbox and click "Start Match", **Then** the match data is sent to the new URL.
2. **Given** a simulation is already running, **When** I change the URL in the textbox, **Then** subsequent data packets for that simulation are immediately sent to the new URL.

---

### User Story 2 - Control Data-Sending Services (Priority: P2)

As a developer, I want explicit controls to start and stop all data-sending services independently.

**Why this priority**: Provides precise control over the simulation, which is necessary for effective testing.

**Acceptance Scenarios**:
1. **Given** the application is launched, **When** I click the "Start Match" button, **Then** only the match simulation begins sending data.
2. **Given** the application is launched, **When** I activate the prop simulation, **Then** only the prop simulation begins sending data.
3. **Given** a simulation is running, **When** its corresponding "Stop" control is activated, **Then** it ceases sending data without affecting other running simulations.

---

### User Story 3 - Set Default URL (Priority: P3)

As a developer, I want the Target URL field to be pre-filled with a specific default address on application startup.

**Why this priority**: A quality-of-life improvement that reduces configuration friction.

**Acceptance Scenarios**:
1. **Given** the application is launched for the first time, **When** the main window appears, **Then** the Target URL textbox contains the value "http://192.168.1.234:9055".

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: The Target URL textbox MUST have a default value of "http://192.168.1.234:9055".
- **FR-002**: All data-sending services (Match and Prop simulators) MUST read the current value from the Target URL textbox each time they prepare to send a request.
- **FR-003**: The UI MUST provide separate and clear controls for starting and stopping the Match simulation.
- **FR-004**: The UI MUST provide separate and clear controls for starting and stopping the Prop simulation (the prop simulation should no longer start automatically).

### Key Entities *(include if feature involves data)*

- No new data entities are introduced in this feature. This work refactors existing behavior.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: When the application starts, the Target URL textbox is populated with "http://192.168.1.234:9055".
- **SC-002**: 100% of HTTP requests sent by any simulation service are directed to the exact URL currently displayed in the Target URL textbox at the time of sending.
- **SC-003**: The prop simulation only sends data after its specific "start" control has been activated by the user.

### Edge Cases
- How should the system behave if the URL is changed to an invalid format while a service is running? (It should stop sending and log an error).
- What happens if the "Stop" button is clicked when the service is already stopped? (Nothing, the state should remain "stopped").