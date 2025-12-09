# Feature Specification: Laser Tag Test Simulator

**Feature Branch**: `001-laser-tag-simulator`  
**Created**: 2025-12-09
**Status**: Draft  
**Input**: User description: "Create a new desktop application that will act as a 'Test Harness' or 'Emulator' that generates and pushes HTTP POST requests to the `GOTF-laser-tag` application."

## User Scenarios & Verification *(mandatory)*

### User Story 1 - Configure Simulation (Priority: P1)

As a developer, I need to configure the simulation's target endpoint and match parameters so I can control the test environment.

**Why this priority**: Configuration is essential before any simulation can be executed.

**Acceptance Scenarios**:
1. **Given** the application is open, **When** I enter a new URL in the "Target URL" field, **Then** subsequent requests are sent to the new URL.
2. **Given** the application is open, **When** I change the "Match Duration" and "Players Per Team", **Then** clicking "Generate Roster" creates a player list matching the new parameters.

---

### User Story 2 - Simulate a Match Lifecycle (Priority: P2)

As a developer, I need to start, pause, and stop a match simulation to test how the main application handles the entire match lifecycle.

**Why this priority**: This is the primary function of the host simulation.

**Acceptance Scenarios**:
1. **Given** a valid configuration, **When** I click "Start Match", **Then** the simulator begins sending match data heartbeats at the configured interval.
2. **Given** a match is running, **When** I click "Pause", **Then** the heartbeats stop.
3. **Given** a match is running or paused, **When** I click "Stop/Reset", **Then** the simulation ends and all timers are reset.

---

### User Story 3 - Simulate Live Player Events (Priority: P3)

As a developer, I need to simulate real-time player events to test the dynamic game logic of the target application.

**Why this priority**: Allows for testing real-time data changes.

**Acceptance Scenarios**:
1. **Given** a match is running and a player is selected in the roster grid, **When** I trigger the "Shoot Player" action, **Then** the selected player's health is lower in the next match data heartbeat.
2. **Given** a player's health is at or below the threshold, **When** they are shot again, **Then** their state changes to "Dead" in the next heartbeat.

---

### User Story 4 - Simulate an Interactive Prop (Priority: P4)

As a developer, I need to simulate an interactive prop with its own lifecycle to test the target application's ability to handle multiple, independent data sources.

**Why this priority**: Completes the simulation by emulating all hardware inputs.

**Acceptance Scenarios**:
1. **Given** the application is running, **When** I change the prop's state via the UI, **Then** the new state is reflected in the next prop data heartbeat sent to the `/prop` endpoint.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: The system MUST provide a user interface for configuring the target server URL and data push frequency.
- **FR-002**: The system MUST allow users to define match parameters, including duration, team names, and player count.
- **FR-003**: The system MUST provide a function to generate a player roster based on configured parameters.
- **FR-004**: The system MUST provide controls to Start, Pause, and Stop/Reset the match simulation.
- **FR-005**: The system MUST periodically send match state data to a `/match` endpoint via HTTP POST.
- **FR-006**: The system MUST display the generated player roster in an interactive grid.
- **FR-007**: Users MUST be able to trigger events for specific players that alter their state (health, kills, etc.) in subsequent data pushes.
- **FR-008**: The system MUST provide controls to manage the state of a secondary "prop" simulation.
- **FR-009**: The system MUST periodically send prop state data to a `/prop` endpoint, independent of the match simulation.
- **FR-010**: The application MUST detect and log connection errors without crashing.
- **FR-011**: All outgoing data payloads MUST use `camelCase` for field names.

### Key Entities *(include if feature involves data)*

- **Match Snapshot**: Represents the state of the entire match at a point in time. Contains a list of `Player` objects.
- **Player**: Represents the state of a single player, including health, team, kills, deaths, etc.
- **Prop Snapshot**: Represents the state of the interactive prop, including its status and timers.

### Assumptions
- The user of this simulator is a developer testing the `GOTF-laser-tag` application.
- The target application is expecting JSON payloads.
- The `/match` and `/prop` endpoints are on the same target server.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: The simulator successfully sends HTTP POST requests to the configured target URL for both `/match` and `/prop` endpoints.
- **SC-002**: Match heartbeat payloads are sent at the user-defined frequency (e.g., every 100ms), +/- 20ms.
- **SC-003**: 100% of UI-driven player state changes are included in the subsequent `/match` heartbeat payload.
- **SC-004**: The application remains responsive and does not crash if the target URL is unreachable, logging at least one error message per 5 seconds of downtime.
- **SC-005**: Prop simulation states can be changed via the UI, and the change is reflected in the `/prop` payload within one second.

### Edge Cases
- What happens if the Target URL is invalid or the server is offline?
- How does the system handle a Match Duration of 0 or a negative number?
- What happens if "Players Per Team" is set to 0?
- How are large numbers of players handled in the UI and in the payload?
