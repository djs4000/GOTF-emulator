# Quickstart: Laser Tag Simulator

**Date**: 2025-12-09
**Input**: [Implementation Plan](plan.md)

This document provides instructions on how to run and use the Laser Tag Simulator.

## Prerequisites

*   .NET 9.0 SDK
*   A running instance of the `GOTF-laser-tag` application.

## How to Run

1.  **Build the Application**:
    *   Navigate to the `src/LaserTag.Simulator` directory.
    *   Run `dotnet build`.

2.  **Run the Application**:
    *   After a successful build, run the executable located in `bin/Debug/net9.0/LaserTag.Simulator.exe`.

## How to Use

1.  **Configure Settings**:
    *   Ensure the **Target URL** is pointing to your running instance of the `GOTF-laser-tag` application (default is `http://127.0.0.1:5055`).
    *   Adjust the **Update Frequency**, **Match Duration**, and other settings as needed.

2.  **Generate Roster**:
    *   Click the **"Generate Roster"** button to populate the player grid.

3.  **Start the Simulation**:
    *   Click **"Start Match"** to begin sending `/match` heartbeats.
    *   Use the **"Pause"** and **"Stop/Reset"** buttons to control the match flow.

4.  **Simulate Player Actions**:
    *   Right-click on a player in the grid to access actions like "Shoot Player", "Kill Player", or "Register Kill".
    *   Changes will be reflected in the next heartbeat.

5.  **Simulate the Prop**:
    *   Use the radio buttons or buttons in the "Prop Simulation" section to change the prop's state.
    *   The simulator will independently send `/prop` heartbeats with the updated state.
