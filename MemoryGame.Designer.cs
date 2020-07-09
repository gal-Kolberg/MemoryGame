using System;

namespace MemoryGameUI
{
    partial class MemoryGame
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
            this.CurrentPlayer = new System.Windows.Forms.Label();
            this.Player1Name = new System.Windows.Forms.Label();
            this.Player2Name = new System.Windows.Forms.Label();
            this.TileButton = new System.Windows.Forms.Button();
            this.PicTimer = new System.Windows.Forms.Timer(this.components);
            this.ComputerTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // CurrentPlayer
            // 
            this.CurrentPlayer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CurrentPlayer.AutoSize = true;
            this.CurrentPlayer.BackColor = System.Drawing.Color.PaleGreen;
            this.CurrentPlayer.Location = new System.Drawing.Point(101, 506);
            this.CurrentPlayer.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.CurrentPlayer.Name = "CurrentPlayer";
            this.CurrentPlayer.Size = new System.Drawing.Size(205, 32);
            this.CurrentPlayer.TabIndex = 0;
            this.CurrentPlayer.Text = "Current Player:";
            // 
            // Player1Name
            // 
            this.Player1Name.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Player1Name.AutoSize = true;
            this.Player1Name.BackColor = System.Drawing.Color.PaleGreen;
            this.Player1Name.Location = new System.Drawing.Point(101, 579);
            this.Player1Name.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Player1Name.Name = "Player1Name";
            this.Player1Name.Size = new System.Drawing.Size(187, 32);
            this.Player1Name.TabIndex = 1;
            this.Player1Name.Text = "Player1Name";
            // 
            // Player2Name
            // 
            this.Player2Name.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Player2Name.AutoSize = true;
            this.Player2Name.BackColor = System.Drawing.Color.Plum;
            this.Player2Name.Location = new System.Drawing.Point(101, 646);
            this.Player2Name.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Player2Name.Name = "Player2Name";
            this.Player2Name.Size = new System.Drawing.Size(187, 32);
            this.Player2Name.TabIndex = 2;
            this.Player2Name.Text = "Player2Name";
            // 
            // TileButton
            // 
            this.TileButton.Location = new System.Drawing.Point(77, 48);
            this.TileButton.Margin = new System.Windows.Forms.Padding(5);
            this.TileButton.Name = "TileButton";
            this.TileButton.Size = new System.Drawing.Size(227, 203);
            this.TileButton.TabIndex = 3;
            this.TileButton.UseVisualStyleBackColor = true;
            // 
            // PicTimer
            // 
            this.PicTimer.Interval = 1000;
            this.PicTimer.Tick += new System.EventHandler(this.PicTimer_Tick);
            // 
            // ComputerTimer
            // 
            this.ComputerTimer.Interval = 1000;
            this.ComputerTimer.Tick += new System.EventHandler(this.ComputerTimer_Tick);
            // 
            // MemoryGame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(1418, 734);
            this.Controls.Add(this.TileButton);
            this.Controls.Add(this.Player2Name);
            this.Controls.Add(this.Player1Name);
            this.Controls.Add(this.CurrentPlayer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MaximizeBox = false;
            this.Name = "MemoryGame";
            this.Text = "MemoryGame";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label CurrentPlayer;
        private System.Windows.Forms.Label Player1Name;
        private System.Windows.Forms.Label Player2Name;
        private System.Windows.Forms.Button TileButton;
        private System.Windows.Forms.Timer PicTimer;
        private System.Windows.Forms.Timer ComputerTimer;
    }
}