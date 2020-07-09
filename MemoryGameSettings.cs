using System;
using System.Windows.Forms;
using System.Drawing;

namespace MemoryGameUI
{
    public class MemoryGameSettings : Form
    {
        private TextBox FirstPlayerTextBox;
        private Label SecondPlayerName;
        private TextBox SecondPlayerTextBox;
        private Button AgainstFriendButton;
        private Label BoardSizeLabel;
        private Button StartGameButton;
        private ButtonBoardSize BoardSizeButton;
        private Label FirstPLayerName;

        public MemoryGameSettings()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.FirstPLayerName = new System.Windows.Forms.Label();
            this.FirstPlayerTextBox = new System.Windows.Forms.TextBox();
            this.SecondPlayerName = new System.Windows.Forms.Label();
            this.SecondPlayerTextBox = new System.Windows.Forms.TextBox();
            this.AgainstFriendButton = new System.Windows.Forms.Button();
            this.BoardSizeLabel = new System.Windows.Forms.Label();
            this.StartGameButton = new System.Windows.Forms.Button();
            this.BoardSizeButton = new MemoryGameUI.ButtonBoardSize();
            this.SuspendLayout();
            // 
            // FirstPLayerName
            // 
            this.FirstPLayerName.AutoSize = true;
            this.FirstPLayerName.Location = new System.Drawing.Point(29, 39);
            this.FirstPLayerName.Name = "FirstPLayerName";
            this.FirstPLayerName.Size = new System.Drawing.Size(248, 32);
            this.FirstPLayerName.TabIndex = 0;
            this.FirstPLayerName.Text = "First Player Name:";
            // 
            // FirstPlayerTextBox
            // 
            this.FirstPlayerTextBox.Location = new System.Drawing.Point(345, 39);
            this.FirstPlayerTextBox.Name = "FirstPlayerTextBox";
            this.FirstPlayerTextBox.Size = new System.Drawing.Size(252, 38);
            this.FirstPlayerTextBox.TabIndex = 1;
            // 
            // SecondPlayerName
            // 
            this.SecondPlayerName.AutoSize = true;
            this.SecondPlayerName.Location = new System.Drawing.Point(29, 115);
            this.SecondPlayerName.Name = "SecondPlayerName";
            this.SecondPlayerName.Size = new System.Drawing.Size(290, 32);
            this.SecondPlayerName.TabIndex = 2;
            this.SecondPlayerName.Text = "Second Player Name:";
            // 
            // SecondPlayerTextBox
            // 
            this.SecondPlayerTextBox.BackColor = System.Drawing.SystemColors.ControlLight;
            this.SecondPlayerTextBox.Enabled = false;
            this.SecondPlayerTextBox.Location = new System.Drawing.Point(345, 109);
            this.SecondPlayerTextBox.Name = "SecondPlayerTextBox";
            this.SecondPlayerTextBox.Size = new System.Drawing.Size(252, 38);
            this.SecondPlayerTextBox.TabIndex = 3;
            this.SecondPlayerTextBox.Text = "- computer -";
            // 
            // AgainstFriendButton
            // 
            this.AgainstFriendButton.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.AgainstFriendButton.Location = new System.Drawing.Point(642, 93);
            this.AgainstFriendButton.Name = "AgainstFriendButton";
            this.AgainstFriendButton.Size = new System.Drawing.Size(241, 54);
            this.AgainstFriendButton.TabIndex = 4;
            this.AgainstFriendButton.Text = "&Against a Friend";
            this.AgainstFriendButton.UseVisualStyleBackColor = false;
            this.AgainstFriendButton.Click += new System.EventHandler(this.AgainstFriendButton_Click);
            // 
            // BoardSizeLabel
            // 
            this.BoardSizeLabel.AutoSize = true;
            this.BoardSizeLabel.Location = new System.Drawing.Point(29, 199);
            this.BoardSizeLabel.Name = "BoardSizeLabel";
            this.BoardSizeLabel.Size = new System.Drawing.Size(162, 32);
            this.BoardSizeLabel.TabIndex = 5;
            this.BoardSizeLabel.Text = "Board Size:";
            // 
            // StartGameButton
            // 
            this.StartGameButton.BackColor = System.Drawing.Color.LimeGreen;
            this.StartGameButton.Location = new System.Drawing.Point(689, 375);
            this.StartGameButton.Name = "StartGameButton";
            this.StartGameButton.Size = new System.Drawing.Size(194, 62);
            this.StartGameButton.TabIndex = 7;
            this.StartGameButton.Text = "&Start!";
            this.StartGameButton.UseVisualStyleBackColor = false;
            this.StartGameButton.Click += new System.EventHandler(this.StartGameButton_Click);
            // 
            // BoardSizeButton
            // 
            this.BoardSizeButton.BackColor = System.Drawing.Color.CornflowerBlue;
            this.BoardSizeButton.CurrentTextIndex = 0;
            this.BoardSizeButton.Location = new System.Drawing.Point(35, 250);
            this.BoardSizeButton.Name = "BoardSizeButton";
            this.BoardSizeButton.Size = new System.Drawing.Size(284, 187);
            this.BoardSizeButton.TabIndex = 8;
            this.BoardSizeButton.Text = this.BoardSizeButton.CurrentSizeString;
            this.BoardSizeButton.UseVisualStyleBackColor = false;
            this.BoardSizeButton.Click += new System.EventHandler(this.BoardSizeButton_Click);
            // 
            // MemoryGameSettings
            // 
            this.AcceptButton = this.StartGameButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(240F, 240F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(914, 459);
            this.Controls.Add(this.BoardSizeButton);
            this.Controls.Add(this.StartGameButton);
            this.Controls.Add(this.BoardSizeLabel);
            this.Controls.Add(this.AgainstFriendButton);
            this.Controls.Add(this.SecondPlayerTextBox);
            this.Controls.Add(this.SecondPlayerName);
            this.Controls.Add(this.FirstPlayerTextBox);
            this.Controls.Add(this.FirstPLayerName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MemoryGameSettings";
            this.ShowInTaskbar = false;
            this.Text = "Memory Game - Settings";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void StartGameButton_Click(object i_Sender, EventArgs i_E)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void AgainstFriendButton_Click(object i_Sender, EventArgs i_E)
        {
            if (SecondPlayerTextBox.Enabled == false) 
            {
                SecondPlayerTextBox.Text = string.Empty;
                SecondPlayerTextBox.BackColor = FirstPlayerTextBox.BackColor;
            }
            else
            {
                SecondPlayerTextBox.Text = "- computer -";
                SecondPlayerTextBox.BackColor = System.Drawing.SystemColors.ControlLight;
            }

            SecondPlayerTextBox.Enabled = !SecondPlayerTextBox.Enabled;
        }

        private void BoardSizeButton_Click(object i_Sender, EventArgs i_E)
        {
            ButtonBoardSize buttonBoardSize = i_Sender as ButtonBoardSize;
            buttonBoardSize.ChangeSize();
        }

        public Point BoardSize
        {
            get
            {
                return BoardSizeButton.CurrentSize;
            }
        }

        public string Player1Name
        {
            get
            {
                return FirstPlayerTextBox.Text;
            }
        }

        public string Player2Name
        {
            get
            {
                string player2Name;

                if(SecondPlayerTextBox.Enabled == true)
                {
                    player2Name = SecondPlayerTextBox.Text;
                }
                else
                {
                    player2Name = "Computer";
                }

                return player2Name;
            }
        }

        public bool IsPvP
        {
            get
            {
                return !SecondPlayerTextBox.Enabled;
            }
        }
    }
}
