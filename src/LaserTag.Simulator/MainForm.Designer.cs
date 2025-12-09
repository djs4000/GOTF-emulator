namespace LaserTag.Simulator
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 900); // Increased height for better layout
            this.Text = "Laser Tag Simulator";

            // Main Table Layout
            this.mainLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.mainLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayoutPanel.ColumnCount = 2;
            this.mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.mainLayoutPanel.RowCount = 2;
            this.mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85F)); // Main content row
            this.mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F)); // Console row
            this.Controls.Add(this.mainLayoutPanel);
            
            // Player Roster DataGridView
            this.playerDataGridView = new System.Windows.Forms.DataGridView();
            this.playerDataGridView.AutoGenerateColumns = false;
            this.playerDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.playerDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                new System.Windows.Forms.DataGridViewTextBoxColumn() { Name = "Id", HeaderText = "Player ID", DataPropertyName = "Id", SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic },
                new System.Windows.Forms.DataGridViewTextBoxColumn() { Name = "Team", HeaderText = "Team", DataPropertyName = "Team", SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic },
                new System.Windows.Forms.DataGridViewTextBoxColumn() { Name = "Health", HeaderText = "Health", DataPropertyName = "Health", SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic },
                new System.Windows.Forms.DataGridViewTextBoxColumn() { Name = "Ammo", HeaderText = "Ammo", DataPropertyName = "Ammo", SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic },
                new System.Windows.Forms.DataGridViewTextBoxColumn() { Name = "State", HeaderText = "State", DataPropertyName = "State", SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic },
                new System.Windows.Forms.DataGridViewTextBoxColumn() { Name = "KillsCount", HeaderText = "Kills", DataPropertyName = "KillsCount", SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic },
                new System.Windows.Forms.DataGridViewTextBoxColumn() { Name = "Deaths", HeaderText = "Deaths", DataPropertyName = "Deaths", SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic }
            });
            this.mainLayoutPanel.Controls.Add(this.playerDataGridView, 0, 0);

            // Side Panel for GroupBoxes
            var sidePanelLayout = new System.Windows.Forms.TableLayoutPanel() { Dock = System.Windows.Forms.DockStyle.Fill, ColumnCount = 1, RowCount = 3 };
            sidePanelLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F)); // Config gets more space
            sidePanelLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F)); // Match
            sidePanelLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F)); // Prop
            this.mainLayoutPanel.Controls.Add(sidePanelLayout, 1, 0);

            // GroupBoxes
            this.configGroupBox = new System.Windows.Forms.GroupBox() { Text = "Configuration", Dock = System.Windows.Forms.DockStyle.Fill };
            this.matchGroupBox = new System.Windows.Forms.GroupBox() { Text = "Match Simulation", Dock = System.Windows.Forms.DockStyle.Fill };
            this.propGroupBox = new System.Windows.Forms.GroupBox() { Text = "Prop Simulation", Dock = System.Windows.Forms.DockStyle.Fill };
            sidePanelLayout.Controls.Add(this.configGroupBox, 0, 0);
            sidePanelLayout.Controls.Add(this.matchGroupBox, 0, 1);
            sidePanelLayout.Controls.Add(this.propGroupBox, 0, 2);

            // -- Configuration Controls --
            var configLayout = new System.Windows.Forms.FlowLayoutPanel() { Dock = System.Windows.Forms.DockStyle.Fill, FlowDirection = System.Windows.Forms.FlowDirection.TopDown, Padding = new System.Windows.Forms.Padding(10), AutoScroll = true };
            this.configGroupBox.Controls.Add(configLayout);

            this.targetUrlLabel = new System.Windows.Forms.Label() { Text = "Target URL:", AutoSize = true };
            this.targetUrlTextBox = new System.Windows.Forms.TextBox() { Text = "http://192.168.1.234:9055", Width = 300 };
            this.updateFreqLabel = new System.Windows.Forms.Label() { Text = "Update Frequency (ms):", AutoSize = true };
            this.updateFreqNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.updateFreqNumericUpDown.Minimum = 0;
            this.updateFreqNumericUpDown.Maximum = 10000;
            this.updateFreqNumericUpDown.Value = 100;
            this.matchDurationLabel = new System.Windows.Forms.Label() { Text = "Match Duration (s):", AutoSize = true };
            this.matchDurationNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.matchDurationNumericUpDown.Minimum = 0;
            this.matchDurationNumericUpDown.Maximum = 3600;
            this.matchDurationNumericUpDown.Value = 219;
            this.team1NameLabel = new System.Windows.Forms.Label() { Text = "Team 1 Name:", AutoSize = true };
            this.team1NameTextBox = new System.Windows.Forms.TextBox() { Text = "Team 1" };
            this.team2NameLabel = new System.Windows.Forms.Label() { Text = "Team 2 Name:", AutoSize = true };
            this.team2NameTextBox = new System.Windows.Forms.TextBox() { Text = "Team 2" };
            this.playersPerTeamLabel = new System.Windows.Forms.Label() { Text = "Players Per Team:", AutoSize = true };
            this.playersPerTeamNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.playersPerTeamNumericUpDown.Minimum = 1;
            this.playersPerTeamNumericUpDown.Maximum = 20;
            this.playersPerTeamNumericUpDown.Value = 5;
            this.generateRosterButton = new System.Windows.Forms.Button() { Text = "Generate Roster", AutoSize = true };

            configLayout.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.targetUrlLabel, this.targetUrlTextBox, this.updateFreqLabel, this.updateFreqNumericUpDown,
                this.matchDurationLabel, this.matchDurationNumericUpDown, this.team1NameLabel, this.team1NameTextBox,
                this.team2NameLabel, this.team2NameTextBox, this.playersPerTeamLabel, this.playersPerTeamNumericUpDown,
                this.generateRosterButton
            });

            // -- Match Controls --
            var matchLayout = new System.Windows.Forms.FlowLayoutPanel() { Dock = System.Windows.Forms.DockStyle.Fill, FlowDirection = System.Windows.Forms.FlowDirection.TopDown, Padding = new System.Windows.Forms.Padding(10) };
            this.matchGroupBox.Controls.Add(matchLayout);

            this.startMatchButton = new System.Windows.Forms.Button() { Text = "Start Match", AutoSize = true };
            this.pauseMatchButton = new System.Windows.Forms.Button() { Text = "Pause Match", AutoSize = true };
            this.stopResetMatchButton = new System.Windows.Forms.Button() { Text = "Stop/Reset Match", AutoSize = true };
            this.matchTimerLabel = new System.Windows.Forms.Label() { Text = "Match Time: 00:00", AutoSize = true };
            
            matchLayout.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.startMatchButton, this.pauseMatchButton, this.stopResetMatchButton, this.matchTimerLabel
            });

            // -- Prop Controls --
            var propLayout = new System.Windows.Forms.FlowLayoutPanel() { Dock = System.Windows.Forms.DockStyle.Fill, FlowDirection = System.Windows.Forms.FlowDirection.TopDown, Padding = new System.Windows.Forms.Padding(10) };
            this.propGroupBox.Controls.Add(propLayout);
            
            this.propStateGroupBox = new System.Windows.Forms.GroupBox() { Text = "Prop State", AutoSize = true, Dock = System.Windows.Forms.DockStyle.Top };
            var propStateLayout = new System.Windows.Forms.FlowLayoutPanel() { Dock = System.Windows.Forms.DockStyle.Fill, FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight, WrapContents = true };
            this.propStateGroupBox.Controls.Add(propStateLayout);

            this.propStateIdleRadio = new System.Windows.Forms.RadioButton() { Text = "Idle", AutoSize = true, Checked = true };
            this.propStateActiveRadio = new System.Windows.Forms.RadioButton() { Text = "Active", AutoSize = true };
            this.propStateArmingRadio = new System.Windows.Forms.RadioButton() { Text = "Arming", AutoSize = true };
            this.propStateArmedRadio = new System.Windows.Forms.RadioButton() { Text = "Armed", AutoSize = true };
            this.propStateDefusedRadio = new System.Windows.Forms.RadioButton() { Text = "Defused", AutoSize = true };
            this.propStateDetonatedRadio = new System.Windows.Forms.RadioButton() { Text = "Detonated", AutoSize = true };
            propStateLayout.Controls.AddRange(new System.Windows.Forms.Control[] { 
                this.propStateIdleRadio, this.propStateActiveRadio, this.propStateArmingRadio, 
                this.propStateArmedRadio, this.propStateDefusedRadio, this.propStateDetonatedRadio 
            });

            this.propArmingGroupBox = new System.Windows.Forms.GroupBox() { Text = "Arming Action", AutoSize = true, Dock = System.Windows.Forms.DockStyle.Top, Visible = false };
            var propArmingLayout = new System.Windows.Forms.FlowLayoutPanel() { Dock = System.Windows.Forms.DockStyle.Fill };
            this.propArmingGroupBox.Controls.Add(propArmingLayout);
            this.propArmSuccessButton = new System.Windows.Forms.Button() { Text = "Success" };
            this.propArmFailButton = new System.Windows.Forms.Button() { Text = "Fail" };
            propArmingLayout.Controls.AddRange(new System.Windows.Forms.Control[] { this.propArmSuccessButton, this.propArmFailButton });
            
            this.propTimerLabel = new System.Windows.Forms.Label() { Text = "Prop Timer: 00:00", AutoSize = true };

            this.startPropButton = new System.Windows.Forms.Button() { Text = "Start Prop", AutoSize = true };
            this.stopPropButton = new System.Windows.Forms.Button() { Text = "Stop Prop", AutoSize = true };
            propLayout.Controls.AddRange(new System.Windows.Forms.Control[] { this.startPropButton, this.stopPropButton, this.propStateGroupBox, this.propArmingGroupBox, this.propTimerLabel });

            // Console TextBox
            this.consoleTextBox = new System.Windows.Forms.TextBox() { 
                Dock = System.Windows.Forms.DockStyle.Fill, 
                Multiline = true, 
                ScrollBars = System.Windows.Forms.ScrollBars.Vertical, 
                ReadOnly = true
            };
            this.mainLayoutPanel.Controls.Add(this.consoleTextBox, 0, 1);
            this.mainLayoutPanel.SetColumnSpan(this.consoleTextBox, 2);

        }

        private System.Windows.Forms.TableLayoutPanel mainLayoutPanel;
        private System.Windows.Forms.DataGridView playerDataGridView;
        // private System.Windows.Forms.TableLayoutPanel bottomLayoutPanel; // No longer needed
        private System.Windows.Forms.GroupBox configGroupBox;
        private System.Windows.Forms.GroupBox matchGroupBox;
        private System.Windows.Forms.GroupBox propGroupBox;
        private System.Windows.Forms.TextBox consoleTextBox;

        // Config
        private System.Windows.Forms.Label targetUrlLabel;
        private System.Windows.Forms.TextBox targetUrlTextBox;
        private System.Windows.Forms.Label updateFreqLabel;
        private System.Windows.Forms.NumericUpDown updateFreqNumericUpDown;
        private System.Windows.Forms.Label matchDurationLabel;
        private System.Windows.Forms.NumericUpDown matchDurationNumericUpDown;
        private System.Windows.Forms.Label team1NameLabel;
        private System.Windows.Forms.TextBox team1NameTextBox;
        private System.Windows.Forms.Label team2NameLabel;
        private System.Windows.Forms.TextBox team2NameTextBox;
        private System.Windows.Forms.Label playersPerTeamLabel;
        private System.Windows.Forms.NumericUpDown playersPerTeamNumericUpDown;
        private System.Windows.Forms.Button generateRosterButton;
        
        // Match
        private System.Windows.Forms.Button startMatchButton;
        private System.Windows.Forms.Button pauseMatchButton;
        private System.Windows.Forms.Button stopResetMatchButton;
        private System.Windows.Forms.Label matchTimerLabel;

        // Prop
        private System.Windows.Forms.GroupBox propStateGroupBox;
        private System.Windows.Forms.RadioButton propStateIdleRadio;
        private System.Windows.Forms.RadioButton propStateActiveRadio;
        private System.Windows.Forms.RadioButton propStateArmingRadio;
        private System.Windows.Forms.RadioButton propStateArmedRadio;
        private System.Windows.Forms.RadioButton propStateDefusedRadio;
        private System.Windows.Forms.RadioButton propStateDetonatedRadio;
        private System.Windows.Forms.GroupBox propArmingGroupBox;
        private System.Windows.Forms.Button propArmSuccessButton;
        private System.Windows.Forms.Button propArmFailButton;
        private System.Windows.Forms.Label propTimerLabel;
        private System.Windows.Forms.Button startPropButton;
        private System.Windows.Forms.Button stopPropButton;

        #endregion
    }
}