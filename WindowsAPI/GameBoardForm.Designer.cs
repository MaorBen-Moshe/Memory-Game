namespace WindowsAPI
{
    partial class GameBoardForm
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
            this.labelCurrentPlayer = new System.Windows.Forms.Label();
            this.labelFirstPlayer = new System.Windows.Forms.Label();
            this.labelSecondPlayer = new System.Windows.Forms.Label();
            this.labelCurrentPlayerName = new System.Windows.Forms.Label();
            this.labelFirstPlayerPairs = new System.Windows.Forms.Label();
            this.labelSecondPlayerPairs = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelCurrentPlayer
            // 
            this.labelCurrentPlayer.AutoSize = true;
            this.labelCurrentPlayer.Location = new System.Drawing.Point(34, 548);
            this.labelCurrentPlayer.Name = "labelCurrentPlayer";
            this.labelCurrentPlayer.Size = new System.Drawing.Size(113, 20);
            this.labelCurrentPlayer.TabIndex = 0;
            this.labelCurrentPlayer.Text = "Current Player:";
            // 
            // labelFirstPlayer
            // 
            this.labelFirstPlayer.AutoSize = true;
            this.labelFirstPlayer.Location = new System.Drawing.Point(34, 583);
            this.labelFirstPlayer.Name = "labelFirstPlayer";
            this.labelFirstPlayer.Size = new System.Drawing.Size(86, 20);
            this.labelFirstPlayer.TabIndex = 1;
            this.labelFirstPlayer.Text = "FirstName:";
            // 
            // labelSecondPlayer
            // 
            this.labelSecondPlayer.AutoSize = true;
            this.labelSecondPlayer.Location = new System.Drawing.Point(34, 616);
            this.labelSecondPlayer.Name = "labelSecondPlayer";
            this.labelSecondPlayer.Size = new System.Drawing.Size(110, 20);
            this.labelSecondPlayer.TabIndex = 2;
            this.labelSecondPlayer.Text = "SecondName:";
            // 
            // labelCurrentPlayerName
            // 
            this.labelCurrentPlayerName.AutoSize = true;
            this.labelCurrentPlayerName.Location = new System.Drawing.Point(151, 548);
            this.labelCurrentPlayerName.Name = "labelCurrentPlayerName";
            this.labelCurrentPlayerName.Size = new System.Drawing.Size(104, 20);
            this.labelCurrentPlayerName.TabIndex = 3;
            this.labelCurrentPlayerName.Text = "CurrentName";
            // 
            // labelFirstPlayerPairs
            // 
            this.labelFirstPlayerPairs.AutoSize = true;
            this.labelFirstPlayerPairs.Location = new System.Drawing.Point(155, 583);
            this.labelFirstPlayerPairs.Name = "labelFirstPlayerPairs";
            this.labelFirstPlayerPairs.Size = new System.Drawing.Size(126, 20);
            this.labelFirstPlayerPairs.TabIndex = 4;
            this.labelFirstPlayerPairs.Text = "First Player Pairs";
            // 
            // labelSecondPlayerPairs
            // 
            this.labelSecondPlayerPairs.AutoSize = true;
            this.labelSecondPlayerPairs.Location = new System.Drawing.Point(155, 616);
            this.labelSecondPlayerPairs.Name = "labelSecondPlayerPairs";
            this.labelSecondPlayerPairs.Size = new System.Drawing.Size(146, 20);
            this.labelSecondPlayerPairs.TabIndex = 5;
            this.labelSecondPlayerPairs.Text = "SecondPlayer Pairs";
            // 
            // GameBoardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(801, 652);
            this.Controls.Add(this.labelSecondPlayerPairs);
            this.Controls.Add(this.labelFirstPlayerPairs);
            this.Controls.Add(this.labelCurrentPlayerName);
            this.Controls.Add(this.labelSecondPlayer);
            this.Controls.Add(this.labelFirstPlayer);
            this.Controls.Add(this.labelCurrentPlayer);
            this.Name = "GameBoardForm";
            this.Text = "Memory Game";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelCurrentPlayer;
        private System.Windows.Forms.Label labelFirstPlayer;
        private System.Windows.Forms.Label labelSecondPlayer;
        private System.Windows.Forms.Label labelCurrentPlayerName;
        private System.Windows.Forms.Label labelFirstPlayerPairs;
        private System.Windows.Forms.Label labelSecondPlayerPairs;
    }
}