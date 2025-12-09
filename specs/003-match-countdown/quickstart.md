# Quickstart: Laser Tag Simulator (Match Emulation Enhancements)

**Date**: 2025-12-09
**Input**: [Implementation Plan](plan.md)

This document provides updated instructions on how to run and use the Laser Tag Simulator with the new match emulation features.

## Prerequisites

*   .NET 9.0 SDK
*   A running instance of the `GOTF-laser-tag` application (middleware).

## How to Run

1.  **Build the Application**:
    *   Navigate to the repository root.
    *   Run `dotnet build`.

2.  **Run the Application**:
    *   After a successful build, run the executable located in `src/LaserTag.Simulator/bin/Debug/net9.0-windows/LaserTag.Simulator.exe`.

## How to Use Match Emulation (New Features)

1.  **Configure Match Settings**:
    *   **Target URL**: Ensure this points to your running middleware.
    *   **Update Frequency (ms)**: How often heartbeats are sent.
    *   **Match Duration (s)**: The total game time after countdown.
    *   **NEW: Countdown Duration (s)**: Configure the pre-match countdown time.

2.  **Generate Roster**:
    *   The roster should be automatically generated on startup. You can adjust team names/players per team and click "Generate Roster" again if needed. Player IDs are now simple integers (1, 2, 3...).

3.  **Match Workflow**:
    *   **Initial State**: The match starts in a "WaitingOnStart" state. The UI will show "Countdown: --:--" (or the configured countdown time).
    *   **Start Match**: Click **"Start Match"**.
        *   The match state will transition to "Countdown".
        *   The countdown timer will begin visually decrementing in the UI.
        *   When the countdown reaches zero, the match state will automatically transition to "Running", and the main Match Timer will begin.
    *   **During Match**:
        *   Use "Pause Match" to temporarily halt the match and its timers.
        *   Use "Stop/Reset Match" to immediately end the match and go to "Completed" (or a similar final state).
    *   **NEW: Reset Match**: Click **"Reset Match"** to:
        *   Immediately stop any active match or countdown.
        *   Reset the match state to "WaitingOnStart".
        *   Reset all player stats (Health, Ammo, Kills, Deaths) to their default initial values.

4.  **Prop Simulation**:
    *   Use the "Start Prop" and "Stop Prop" buttons to control the prop heartbeat simulation independently.

5.  **Dynamic URL Changes**:
    *   You can change the **Target URL** at any time. The simulation services will use the new URL for the very next heartbeat they send.
