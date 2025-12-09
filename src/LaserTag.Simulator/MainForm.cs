using LaserTag.Simulator.Models;
using LaserTag.Simulator.Services;
using System.Text.Json;

namespace LaserTag.Simulator
{
    public partial class MainForm : Form
    {
        private string _targetUrl;
        private int _updateFrequency;
        private int _matchDuration;
        private int _countdownDuration; // New field
        private readonly MatchSimulatorService _matchSimulatorService;
        private readonly PropSimulatorService _propSimulatorService;
        private BindingSource _playerBindingSource = new BindingSource();
        private ContextMenuStrip _playerContextMenu;

        public MainForm()
        {
            InitializeComponent();
            _matchSimulatorService = new MatchSimulatorService();
            _matchSimulatorService.OnMatchStateChanged += (state, countdownTime, matchTime) => UpdateMatchStateDisplay(state, countdownTime, matchTime);
            _matchSimulatorService.OnCountdownUpdate += (time) => UpdateMatchCountdownDisplay(time);
            _propSimulatorService = new PropSimulatorService();
            _propSimulatorService.OnError += (message) => Log(message);
            _propSimulatorService.OnPropTimerUpdate += (time) => UpdatePropTimerDisplay(time);

            InitializeEventHandlers();
            InitializePlayerContextMenu();
            
            // Populate MatchState ComboBox
            matchStateComboBox.DataSource = Enum.GetValues(typeof(MatchState));

            UpdateConfig();
            LoadDefaultRoster(); // Automatically generate roster on startup
            playerDataGridView.DataSource = _playerBindingSource;

            matchStateLabel.Text = $"State: {MatchState.WaitingOnStart}";
            matchCountdownLabel.Text = "Countdown: --:--";
            matchTimerLabel.Text = "Match Time: --:--";
            // _propSimulatorService.Start(() => _targetUrl, (int)this.updateFreqNumericUpDown.Value); // Prop starts with app - REMOVED
        }

        private void UpdateMatchStateDisplay(MatchState state, long remainingCountdownTimeMs, long remainingMatchTimeMs)
        {
            if (matchStateLabel.InvokeRequired)
            {
                matchStateLabel.Invoke(new Action<MatchState, long, long>(UpdateMatchStateDisplay), state, remainingCountdownTimeMs, remainingMatchTimeMs);
                return;
            }

            matchStateLabel.Text = $"State: {state}";
            matchStateComboBox.SelectedItem = state;
            
            // Enable/Disable controls based on state
            startMatchButton.Enabled = (state == MatchState.WaitingOnStart || state == MatchState.Completed || state == MatchState.Idle);
            stopResetMatchButton.Enabled = (state == MatchState.Running || state == MatchState.Countdown || state == MatchState.WaitingOnFinalData);

            // Update countdown and match timers based on state
            if (state == MatchState.WaitingOnStart || state == MatchState.Countdown)
            {
                UpdateMatchCountdownDisplay(remainingCountdownTimeMs);
                matchTimerLabel.Text = "Match Time: --:--";
            }
            else if (state == MatchState.Running || state == MatchState.WaitingOnFinalData)
            {
                UpdateMatchCountdownDisplay(0); // Clear countdown display
                UpdateMatchTimerDisplay(remainingMatchTimeMs);
            }
            else // Completed, Idle, Cancelled
            {
                UpdateMatchCountdownDisplay(0);
                UpdateMatchTimerDisplay(0);
            }
        }

        private void UpdateMatchCountdownDisplay(long remainingTimeMs)
        {
            if (matchCountdownLabel.InvokeRequired)
            {
                matchCountdownLabel.Invoke(new Action<long>(UpdateMatchCountdownDisplay), remainingTimeMs);
                return;
            }
            TimeSpan t = TimeSpan.FromMilliseconds(remainingTimeMs);
            matchCountdownLabel.Text = $"Countdown: {t.Minutes:D2}:{t.Seconds:D2}";
        }

        private void UpdateMatchTimerDisplay(long remainingTimeMs)
        {
            if (matchTimerLabel.InvokeRequired)
            {
                matchTimerLabel.Invoke(new Action<long>(UpdateMatchTimerDisplay), remainingTimeMs);
                return;
            }
            TimeSpan t = TimeSpan.FromMilliseconds(remainingTimeMs);
            matchTimerLabel.Text = $"Match Time: {t.Minutes:D2}:{t.Seconds:D2}";
        }

        private void InitializeEventHandlers()
        {
            
            this.generateRosterButton.Click += GenerateRosterButton_Click;
            this.startMatchButton.Click += StartMatchButton_Click;
            this.stopResetMatchButton.Click += StopResetMatchButton_Click;

            this.targetUrlTextBox.TextChanged += (s, e) => UpdateConfig();
            this.updateFreqNumericUpDown.ValueChanged += (s, e) => UpdateConfig();
            this.matchDurationNumericUpDown.ValueChanged += (s, e) => UpdateConfig();
            this.countdownDurationNumericUpDown.ValueChanged += (s, e) => UpdateConfig();

            this.propStateIdleRadio.CheckedChanged += PropState_CheckedChanged;
            this.propStateActiveRadio.CheckedChanged += PropState_CheckedChanged;
            this.propStateArmingRadio.CheckedChanged += PropState_CheckedChanged;
            this.propStateArmedRadio.CheckedChanged += PropState_CheckedChanged;
            this.propStateDefusedRadio.CheckedChanged += PropState_CheckedChanged;
            this.propStateDetonatedRadio.CheckedChanged += PropState_CheckedChanged;

            this.propArmSuccessButton.Click += PropArmSuccessButton_Click;
            this.propArmFailButton.Click += PropArmFailButton_Click;
            this.startPropButton.Click += StartPropButton_Click;
            this.stopPropButton.Click += StopPropButton_Click;
        }

        private void StartPropButton_Click(object? sender, EventArgs e)
        {
            Log("Starting prop simulation...");
            _propSimulatorService.Start(() => _targetUrl, (int)this.updateFreqNumericUpDown.Value);
            Log("Prop simulation started.");
        }

        private void StopPropButton_Click(object? sender, EventArgs e)
        {
            Log("Stopping prop simulation...");
            _propSimulatorService.Stop();
            Log("Prop simulation stopped.");
        }

        private void InitializePlayerContextMenu()
        {
            _playerContextMenu = new ContextMenuStrip();
            ToolStripMenuItem shootPlayerMenuItem = new ToolStripMenuItem("Shoot Player");
            ToolStripMenuItem killPlayerMenuItem = new ToolStripMenuItem("Kill Player");
            ToolStripMenuItem registerKillMenuItem = new ToolStripMenuItem("Register Kill");

            shootPlayerMenuItem.Click += ShootPlayerMenuItem_Click;
            killPlayerMenuItem.Click += KillPlayerMenuItem_Click;
            registerKillMenuItem.Click += RegisterKillMenuItem_Click;

            _playerContextMenu.Items.AddRange(new ToolStripItem[] { shootPlayerMenuItem, killPlayerMenuItem, registerKillMenuItem });
            this.playerDataGridView.ContextMenuStrip = _playerContextMenu;
            this.playerDataGridView.CellMouseDown += PlayerDataGridView_CellMouseDown;
        }
        
        private void PlayerDataGridView_CellMouseDown(object? sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                playerDataGridView.ClearSelection();
                playerDataGridView.Rows[e.RowIndex].Selected = true;
                playerDataGridView.CurrentCell = playerDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex > 0 ? e.ColumnIndex : 0];
            }
        }

        private PlayerDto? GetSelectedPlayer()
        {
            if (playerDataGridView.SelectedRows.Count > 0)
            {
                return playerDataGridView.SelectedRows[0].DataBoundItem as PlayerDto;
            }
            return null;
        }

        private void ShootPlayerMenuItem_Click(object? sender, EventArgs e)
        {
            PlayerDto? player = GetSelectedPlayer();
            if (player != null)
            {
                player.Health -= 25;
                if (player.Health <= 0)
                {
                    player.Health = 0;
                    player.State = "Dead";
                    player.Deaths++;
                }
                playerDataGridView.Refresh(); // Refresh the grid to show changes
                Log($"Player {player.Id} shot. Health: {player.Health}, State: {player.State}");
            }
        }

        private void KillPlayerMenuItem_Click(object? sender, EventArgs e)
        {
            PlayerDto? player = GetSelectedPlayer();
            if (player != null)
            {
                player.Health = 0;
                player.State = "Dead";
                player.Deaths++;
                playerDataGridView.Refresh();
                Log($"Player {player.Id} killed. State: {player.State}");
            }
        }

        private void RegisterKillMenuItem_Click(object? sender, EventArgs e)
        {
            PlayerDto? player = GetSelectedPlayer();
            if (player != null)
            {
                player.KillsCount++;
                playerDataGridView.Refresh();
                Log($"Player {player.Id} registered a kill. Kills: {player.KillsCount}");
            }
        }

        private void UpdateConfig()
        {
            _targetUrl = this.targetUrlTextBox.Text;
            _updateFrequency = (int)this.updateFreqNumericUpDown.Value;
            _matchDuration = (int)this.matchDurationNumericUpDown.Value;
            _countdownDuration = (int)this.countdownDurationNumericUpDown.Value; // Read countdown duration
        }

        private void LoadDefaultRoster()
        {
            var team1Name = this.team1NameTextBox.Text;
            var team2Name = this.team2NameTextBox.Text;
            var playersPerTeam = (int)this.playersPerTeamNumericUpDown.Value;

            var players = new List<PlayerDto>();
            for (int i = 1; i <= playersPerTeam; i++)
            {
                players.Add(new PlayerDto { Id = i.ToString(), Team = team1Name, Health = 100, Ammo = 60, State = "Active", KillsCount = 0, Deaths = 0 });
                players.Add(new PlayerDto { Id = (i + playersPerTeam).ToString(), Team = team2Name, Health = 100, Ammo = 60, State = "Active", KillsCount = 0, Deaths = 0 });
            }

            players = players.OrderBy(p => int.Parse(p.Id)).ToList();
            _playerBindingSource.DataSource = players;
        }

        private void GenerateRosterButton_Click(object? sender, EventArgs e)
        {
            LoadDefaultRoster();
        }

        private void StartMatchButton_Click(object? sender, EventArgs e)
        {
            Log("Starting match countdown...");
            var players = _playerBindingSource.DataSource as List<PlayerDto>;
            if (players == null)
            {
                Log("Error: Roster is not generated.");
                return;
            }
            _matchSimulatorService.ConfigureMatch(() => _targetUrl, _updateFrequency, _matchDuration, _countdownDuration, players);
            _matchSimulatorService.BeginCountdown();
        }

        private async void StopResetMatchButton_Click(object? sender, EventArgs e)
        {
            Log("Stopping match and sending final payload...");
            await _matchSimulatorService.CompleteMatchAsync();
            Log("Match stopped.");
        }

        private void PropState_CheckedChanged(object? sender, EventArgs e)
        {
            if (sender is RadioButton rb && rb.Checked)
            {
                string newState = rb.Text;
                _propSimulatorService.UpdatePropState(newState);
                propArmingGroupBox.Visible = (newState == "Arming");
                Log($"Prop state set to: {newState}");
            }
        }

        private void PropArmSuccessButton_Click(object? sender, EventArgs e)
        {
            _propSimulatorService.UpdatePropState("Armed");
            propStateArmedRadio.Checked = true;
            propArmingGroupBox.Visible = false;
            Log("Prop armed successfully.");
        }

        private void PropArmFailButton_Click(object? sender, EventArgs e)
        {
            _propSimulatorService.UpdatePropState("Active");
            propStateActiveRadio.Checked = true;
            propArmingGroupBox.Visible = false;
            Log("Prop arming failed.");
        }
        
        // Removed old UpdateMatchTimerDisplay, it's now integrated into UpdateMatchStateDisplay

        // Original StartMatchButton_Click and associated logic has been heavily refactored above.

        // Prop Timer Display:
        private void UpdatePropTimerDisplay(long remainingTimeMs)
        {
            if (propTimerLabel.InvokeRequired)
            {
                propTimerLabel.Invoke(new Action<long>(UpdatePropTimerDisplay), remainingTimeMs);
                return;
            }
            TimeSpan t = TimeSpan.FromMilliseconds(remainingTimeMs);
            propTimerLabel.Text = $"Prop Timer: {t.Minutes:D2}:{t.Seconds:D2}";
        }


        private void Log(string message)
        {
            if (consoleTextBox.InvokeRequired)
            {
                consoleTextBox.Invoke(new Action<string>(Log), message);
                return;
            }
            consoleTextBox.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _matchSimulatorService.Dispose();
            _propSimulatorService.Dispose(); // Dispose prop service as well
            base.OnFormClosing(e);
        }
    }
}