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
        private readonly MatchSimulatorService _matchSimulatorService;
        private readonly PropSimulatorService _propSimulatorService;
        private BindingSource _playerBindingSource = new BindingSource();
        private ContextMenuStrip _playerContextMenu;

        public MainForm()
        {
            InitializeComponent();
            _matchSimulatorService = new MatchSimulatorService();
            _matchSimulatorService.OnError += (message) => Log(message);
            _propSimulatorService = new PropSimulatorService();
            _propSimulatorService.OnError += (message) => Log(message);
            _propSimulatorService.OnPropTimerUpdate += (time) => UpdatePropTimerDisplay(time);

            InitializeEventHandlers();
            InitializePlayerContextMenu();
            UpdateConfig();
            LoadDefaultRoster(); // Automatically generate roster on startup
            playerDataGridView.DataSource = _playerBindingSource;

            // _propSimulatorService.Start(() => _targetUrl, (int)this.updateFreqNumericUpDown.Value); // Prop starts with app - REMOVED
        }

        private void InitializeEventHandlers()
        {
            this.generateRosterButton.Click += GenerateRosterButton_Click;
            this.startMatchButton.Click += StartMatchButton_Click;
            this.pauseMatchButton.Click += PauseMatchButton_Click;
            this.stopResetMatchButton.Click += StopResetMatchButton_Click;

            this.targetUrlTextBox.TextChanged += (s, e) => UpdateConfig();
            this.updateFreqNumericUpDown.ValueChanged += (s, e) => UpdateConfig();
            this.matchDurationNumericUpDown.ValueChanged += (s, e) => UpdateConfig();

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

            _playerBindingSource.DataSource = players;
        }

        private void GenerateRosterButton_Click(object? sender, EventArgs e)
        {
            LoadDefaultRoster();
        }

        private void StartMatchButton_Click(object? sender, EventArgs e)
        {
            Log("Starting match...");
            var players = _playerBindingSource.DataSource as List<PlayerDto>;
            if (players == null)
            {
                Log("Error: Roster is not generated.");
                return;
            }
            _matchSimulatorService.Start(() => _targetUrl, _updateFrequency, _matchDuration, players);
            Log("Match started.");
        }

        private void PauseMatchButton_Click(object? sender, EventArgs e)
        {
            Log("Pausing match...");
            _matchSimulatorService.Pause();
            Log("Match paused.");
        }

        private void StopResetMatchButton_Click(object? sender, EventArgs e)
        {
            Log("Stopping match...");
            _matchSimulatorService.Stop();
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