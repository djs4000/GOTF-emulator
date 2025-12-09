# Quickstart: Laser Tag Simulator (Updated)

**Date**: 2025-12-09
**Input**: [Implementation Plan](plan.md)

This document provides updated instructions on how to run and use the Laser Tag Simulator.

## Prerequisites

*   .NET 9.0 SDK
*   A running instance of the `GOTF-laser-tag` application.

## How to Run

1.  **Build the Application**:
    *   Navigate to the repository root.
    *   Run `dotnet build`.

2.  **Run the Application**:
    *   After a successful build, run the executable located in `src/LaserTag.Simulator/bin/Debug/net9.0-windows/LaserTag.Simulator.exe`.

## How to Use

1.  **Configure Settings**:
    *   The **Target URL** will default to `http://192.168.1.234:9055`. Change it if your target application is running elsewhere.
    *   Adjust other settings as needed.

2.  **Generate Roster**:
    *   Click the **"Generate Roster"** button to populate the player grid.

3.  **Start the Simulations**:
    *   **Match Simulation**: Click **"Start Match"** to begin sending `/match` heartbeats. Use "Pause" and "Stop/Reset" to control the flow.
    *   **Prop Simulation**: Click the new **"Start Prop"** button to begin sending `/prop` heartbeats. Use the new **"Stop Prop"** button to cease sending.

4.  **Simulate Player Actions**:
    *   Right-click on a player in the grid to access actions like "Shoot Player", "Kill Player", or "Register Kill".

5.  **Dynamic URL Changes**:
    *   You can change the **Target URL** at any time. The simulation services will use the new URL for the very next heartbeat they send.
