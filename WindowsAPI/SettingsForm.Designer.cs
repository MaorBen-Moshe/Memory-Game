namespace WindowsAPI
{
    public partial class SettingsForm
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
            this.buttonStart = new System.Windows.Forms.Button();
            this.buttonAgainst = new System.Windows.Forms.Button();
            this.labelSecondPlayer = new System.Windows.Forms.Label();
            this.labelFirstPlayerName = new System.Windows.Forms.Label();
            this.textBoxFirstName = new System.Windows.Forms.TextBox();
            this.textBoxSecondPlayer = new System.Windows.Forms.TextBox();
            this.labelBoardSize = new System.Windows.Forms.Label();
            this.buttonBoardSize = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            this.buttonStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.buttonStart.Location = new System.Drawing.Point(444, 214);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(134, 44);
            this.buttonStart.TabIndex = 7;
            this.buttonStart.Text = "Start!";
            this.buttonStart.UseVisualStyleBackColor = false;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // buttonAgainst
            // 
            this.buttonAgainst.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAgainst.Location = new System.Drawing.Point(432, 61);
            this.buttonAgainst.Name = "buttonAgainst";
            this.buttonAgainst.Size = new System.Drawing.Size(171, 37);
            this.buttonAgainst.TabIndex = 2;
            this.buttonAgainst.Text = "Against a friend";
            this.buttonAgainst.UseVisualStyleBackColor = true;
            this.buttonAgainst.Click += new System.EventHandler(this.buttonAgainst_Click);
            // 
            // labelSecondPlayer
            // 
            this.labelSecondPlayer.AutoSize = true;
            this.labelSecondPlayer.Location = new System.Drawing.Point(12, 78);
            this.labelSecondPlayer.Name = "labelSecondPlayer";
            this.labelSecondPlayer.Size = new System.Drawing.Size(161, 20);
            this.labelSecondPlayer.TabIndex = 2;
            this.labelSecondPlayer.Text = "Second Player Name:";
            // 
            // labelFirstPlayerName
            // 
            this.labelFirstPlayerName.AutoSize = true;
            this.labelFirstPlayerName.Location = new System.Drawing.Point(12, 34);
            this.labelFirstPlayerName.Name = "labelFirstPlayerName";
            this.labelFirstPlayerName.Size = new System.Drawing.Size(137, 20);
            this.labelFirstPlayerName.TabIndex = 0;
            this.labelFirstPlayerName.Text = "First Player Name:";
            // 
            // textBoxFirstName
            // 
            this.textBoxFirstName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFirstName.Location = new System.Drawing.Point(179, 31);
            this.textBoxFirstName.Name = "textBoxFirstName";
            this.textBoxFirstName.Size = new System.Drawing.Size(247, 26);
            this.textBoxFirstName.TabIndex = 1;
            this.textBoxFirstName.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // textBoxSecondPlayer
            // 
            this.textBoxSecondPlayer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSecondPlayer.Enabled = false;
            this.textBoxSecondPlayer.Location = new System.Drawing.Point(179, 72);
            this.textBoxSecondPlayer.Name = "textBoxSecondPlayer";
            this.textBoxSecondPlayer.Size = new System.Drawing.Size(247, 26);
            this.textBoxSecondPlayer.TabIndex = 3;
            this.textBoxSecondPlayer.Text = "-Computer-";
            this.textBoxSecondPlayer.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // labelBoardSize
            // 
            this.labelBoardSize.AutoSize = true;
            this.labelBoardSize.Location = new System.Drawing.Point(49, 139);
            this.labelBoardSize.Name = "labelBoardSize";
            this.labelBoardSize.Size = new System.Drawing.Size(91, 20);
            this.labelBoardSize.TabIndex = 5;
            this.labelBoardSize.Text = "Board Size:";
            // 
            // buttonBoardSize
            // 
            this.buttonBoardSize.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBoardSize.BackColor = System.Drawing.Color.MediumSlateBlue;
            this.buttonBoardSize.Font = new System.Drawing.Font("Agency FB", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonBoardSize.Location = new System.Drawing.Point(53, 172);
            this.buttonBoardSize.Name = "buttonBoardSize";
            this.buttonBoardSize.Size = new System.Drawing.Size(150, 104);
            this.buttonBoardSize.TabIndex = 6;
            this.buttonBoardSize.Text = "4x4";
            this.buttonBoardSize.UseVisualStyleBackColor = false;
            this.buttonBoardSize.Click += new System.EventHandler(this.buttonBoardSize_Clicked);
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.buttonStart;
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(606, 288);
            this.Controls.Add(this.buttonBoardSize);
            this.Controls.Add(this.labelBoardSize);
            this.Controls.Add(this.textBoxSecondPlayer);
            this.Controls.Add(this.textBoxFirstName);
            this.Controls.Add(this.labelSecondPlayer);
            this.Controls.Add(this.labelFirstPlayerName);
            this.Controls.Add(this.buttonAgainst);
            this.Controls.Add(this.buttonStart);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(688, 441);
            this.MinimumSize = new System.Drawing.Size(628, 344);
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Memory Game - Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button buttonAgainst;
        private System.Windows.Forms.Label labelSecondPlayer;
        private System.Windows.Forms.Label labelFirstPlayerName;
        private System.Windows.Forms.TextBox textBoxFirstName;
        private System.Windows.Forms.TextBox textBoxSecondPlayer;
        private System.Windows.Forms.Label labelBoardSize;
        private System.Windows.Forms.Button buttonBoardSize;
    }
}