**Role:** Software Engineer (C\# / .NET 9 / WinForms)

**Task:** Create a new **Windows Forms (WinForms)** application named `LaserTag.Simulator`. This application will act as a "Test Harness" or "Emulator" that generates and pushes HTTP POST requests to the `GOTF-laser-tag` application.

**Context:**
The existing `GOTF-laser-tag` application listens for data on `/match` (from the Laser Tag Host) and `/prop` (from the physical bomb prop). This new simulator must emulate both of these hardware sources simultaneously to test the system without real hardware.

**Functional Requirements:**

### 1\. **Configuration & Setup (UI)**

  * **Target Settings:**
      * **Target URL:** TextBox (default: `http://127.0.0.1:5055`)
      * **Update Frequency:** NumericUpDown (default: `100` ms)
  * **Match Settings:**
      * **Match Duration:** NumericUpDown (seconds, default: `219`)
      * **Team Names:** Two TextBoxes (default: "Team 1", "Team 2")
      * **Players Per Team:** NumericUpDown (default: `5`)
  * **Roster Generation:**
      * A "Generate Roster" button that creates a list of players for both teams.
      * Players should have configurable defaults: `Health` (100), `Ammo` (Unlimited/Integer), `State` ("Active").

### 2\. **Match Simulation (The "Host")**

  * **Controls:** `Start Match`, `Pause`, `Stop/Reset`.
  * **Timer Logic:**
      * When "Start" is clicked, an internal timer counts down from the configured Match Duration in real-time.
      * **Heartbeat:** A background loop (using `Task.Run` or a generic `Timer`) must send a JSON payload to `POST {TargetUrl}/match` every X milliseconds (configured above).
  * **Payload Structure:**
      * Must match the `MatchSnapshotDto` structure exactly.
      * Fields: `id` (GUID), `timestamp` (Unix ms), `status` (Running/Completed), `remaining_time_ms`, `players` (array).

### 3\. **Player Interaction (Live Grid)**

  * Display the generated roster in a `DataGridView`.
  * **Actions:** Add context menu options or buttons for selected rows:
      * **"Shoot Player"**: Decrements Health by 25. If Health \<= 0, auto-set State to "Dead" and increment the `Deaths` counter.
      * **"Kill Player"**: Sets Health to 0, State to "Dead", and increments `Deaths`.
      * **"Register Kill"**: Increments the `Kills` counter for the selected player (simulating they shot someone else).
  * **Updates:** Any change to this grid must be immediately reflected in the next HTTP heartbeat.

### 4\. **Prop Simulation (The "Bomb")**

  * **Independent Heartbeat:** The prop sends data to `POST {TargetUrl}/prop` on its own interval (e.g., every 500ms).
  * **State Controls:** Provide a GroupBox with Radio Buttons or Buttons for states: `Idle`, `Active`, `Arming`, `Armed`, `Defused`, `Detonated`.
  * **Arming Simulation Logic:**
      * When switching to **"Arming"**, the UI should show two new buttons: `Success` and `Fail`.
      * **Clicking Success:** Transitions state to `Armed`.
      * **Clicking Fail:** Transitions state back to `Active`.
  * **Timer:** When state is `Armed`, an internal "Bomb Timer" (e.g., 45s) must tick down and be included in the JSON payload (`timer_ms`).

### 5\. **Technical Constraints**

  * **Framework:** .NET 9 (WinForms).
  * **Serialization:** Use `System.Text.Json` with `JsonNamingPolicy.CamelCase` to match the receiving app's expectations.
  * **Networking:** Use `HttpClient` for sending requests. Handle connection errors gracefully (e.g., log to a "Console" text box at the bottom of the window if the target is offline, but don't crash).

**Reference Data Structures (JSON):**

  * **/match Payload:**

    ```json
    {
      "id": "sim-match-001",
      "timestamp": 1712345678000,
      "status": "Running",
      "remaining_time_ms": 180000,
      "players": [
        {
          "id": "P1",
          "team": "Team 1",
          "state": "Active",
          "health": 100,
          "kills_count": 0,
          "deaths": 0,
          "ammo": 30
        }
      ]
    }
    ```

  * **/prop Payload:**

    ```json
    {
      "timestamp": 1712345678500,
      "state": "Armed",
      "uptime_ms": 50000,
      "timer_ms": 29500
    }
    ```

**Output:**
Please generate the `Program.cs`, `MainForm.Designer.cs`, `MainForm.cs`, and a simple `DtoModels.cs` file containing the classes needed for serialization.