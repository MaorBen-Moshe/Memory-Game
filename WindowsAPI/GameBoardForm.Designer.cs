namespace WindowsAPI
{
    public partial class GameBoardForm
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
            this.labelCurrentPlayer = new System.Windows.Forms.Label();
            this.labelFirstPlayer = new System.Windows.Forms.Label();
            this.labelSecondPlayer = new System.Windows.Forms.Label();
            this.labelCurrentPlayerName = new System.Windows.Forms.Label();
            this.labelFirstPlayerPairs = new System.Windows.Forms.Label();
            this.labelSecondPlayerPairs = new System.Windows.Forms.Label();
            this.TimerCards = new System.Windows.Forms.Timer(this.components);
            this.panelStatistics = new System.Windows.Forms.Panel();
            this.panelStatistics.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelCurrentPlayer
            // 
            this.labelCurrentPlayer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.labelCurrentPlayer.AutoSize = true;
            this.labelCurrentPlayer.Location = new System.Drawing.Point(12, 18);
            this.labelCurrentPlayer.Name = "labelCurrentPlayer";
            this.labelCurrentPlayer.Size = new System.Drawing.Size(113, 20);
            this.labelCurrentPlayer.TabIndex = 0;
            this.labelCurrentPlayer.Text = "Current Player:";
            // 
            // labelFirstPlayer
            // 
            this.labelFirstPlayer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.labelFirstPlayer.AutoSize = true;
            this.labelFirstPlayer.Location = new System.Drawing.Point(12, 55);
            this.labelFirstPlayer.Name = "labelFirstPlayer";
            this.labelFirstPlayer.Size = new System.Drawing.Size(86, 20);
            this.labelFirstPlayer.TabIndex = 1;
            this.labelFirstPlayer.Text = "FirstName:";
            // 
            // labelSecondPlayer
            // 
            this.labelSecondPlayer.AutoSize = true;
            this.labelSecondPlayer.Location = new System.Drawing.Point(12, 96);
            this.labelSecondPlayer.Name = "labelSecondPlayer";
            this.labelSecondPlayer.Size = new System.Drawing.Size(110, 20);
            this.labelSecondPlayer.TabIndex = 2;
            this.labelSecondPlayer.Text = "SecondName:";
            // 
            // labelCurrentPlayerName
            // 
            this.labelCurrentPlayerName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.labelCurrentPlayerName.AutoSize = true;
            this.labelCurrentPlayerName.Location = new System.Drawing.Point(131, 18);
            this.labelCurrentPlayerName.Name = "labelCurrentPlayerName";
            this.labelCurrentPlayerName.Size = new System.Drawing.Size(104, 20);
            this.labelCurrentPlayerName.TabIndex = 3;
            this.labelCurrentPlayerName.Text = "CurrentName";
            // 
            // labelFirstPlayerPairs
            // 
            this.labelFirstPlayerPairs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.labelFirstPlayerPairs.AutoSize = true;
            this.labelFirstPlayerPairs.Location = new System.Drawing.Point(131, 55);
            this.labelFirstPlayerPairs.Name = "labelFirstPlayerPairs";
            this.labelFirstPlayerPairs.Size = new System.Drawing.Size(126, 20);
            this.labelFirstPlayerPairs.TabIndex = 4;
            this.labelFirstPlayerPairs.Text = "First Player Pairs";
            // 
            // labelSecondPlayerPairs
            // 
            this.labelSecondPlayerPairs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.labelSecondPlayerPairs.AutoSize = true;
            this.labelSecondPlayerPairs.Location = new System.Drawing.Point(131, 96);
            this.labelSecondPlayerPairs.Name = "labelSecondPlayerPairs";
            this.labelSecondPlayerPairs.Size = new System.Drawing.Size(146, 20);
            this.labelSecondPlayerPairs.TabIndex = 5;
            this.labelSecondPlayerPairs.Text = "SecondPlayer Pairs";
            // 
            // TimerCards
            // 
            this.TimerCards.Interval = 750;
            this.TimerCards.Tick += new System.EventHandler(this.TimerCards_Tick);
            // 
            // panelStatistics
            // 
            this.panelStatistics.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panelStatistics.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelStatistics.Controls.Add(this.labelCurrentPlayer);
            this.panelStatistics.Controls.Add(this.labelSecondPlayerPairs);
            this.panelStatistics.Controls.Add(this.labelSecondPlayer);
            this.panelStatistics.Controls.Add(this.labelFirstPlayerPairs);
            this.panelStatistics.Controls.Add(this.labelCurrentPlayerName);
            this.panelStatistics.Controls.Add(this.labelFirstPlayer);
            this.panelStatistics.Location = new System.Drawing.Point(19, 372);
            this.panelStatistics.MaximumSize = new System.Drawing.Size(291, 137);
            this.panelStatistics.Name = "panelStatistics";
            this.panelStatistics.Size = new System.Drawing.Size(291, 137);
            this.panelStatistics.TabIndex = 6;
            // 
            // GameBoardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(772, 521);
            this.Controls.Add(this.panelStatistics);
            this.MaximizeBox = false;
            this.Name = "GameBoardForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Memory Game";
            this.panelStatistics.ResumeLayout(false);
            this.panelStatistics.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelCurrentPlayer;
        private System.Windows.Forms.Label labelFirstPlayer;
        private System.Windows.Forms.Label labelSecondPlayer;
        private System.Windows.Forms.Label labelCurrentPlayerName;
        private System.Windows.Forms.Label labelFirstPlayerPairs;
        private System.Windows.Forms.Label labelSecondPlayerPairs;
        private System.Windows.Forms.Timer TimerCards;
        private System.Windows.Forms.Panel panelStatistics;
    }
}