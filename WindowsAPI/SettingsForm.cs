using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsAPI
{
    public partial class SettingsForm : Form
    {
        private readonly List<Point> r_BoardSizeList = new List<Point>();
        private byte m_CurrentBoardSizeIndex;
        private GameBoardForm m_BoardForm;

        public SettingsForm()
        {
            intialBoardSizes();
            InitializeComponent();
        }

        private void intialBoardSizes()
        {
            for(int i = 4; i <= 6; i++)
            {
                for(int j = 4; j <= 6; j++)
                {
                    if((i * j) % 2 == 0)
                    {
                        r_BoardSizeList.Add(new Point(i, j));
                    }
                }
            }
        }

        public string FirstPlayerName
        {
            get
            {
                return this.textBoxFirstName.Text;
            }
        }

        public string SecondPlayerName
        {
            get
            {
                return this.textBoxSecondPlayer.Text;
            }
        }

        public Point BoardSize
        {
            get
            {
                string[] boardSizeAsString = buttonBoardSize.Text.Split('x');
                return new Point(int.Parse(boardSizeAsString[0]), int.Parse(boardSizeAsString[1]));
            }
        }

        private void buttonBoardSize_Clicked(object i_Sender, EventArgs i_E)
        {
            m_CurrentBoardSizeIndex++;
            if (m_CurrentBoardSizeIndex >= r_BoardSizeList.Count)
            {
                m_CurrentBoardSizeIndex = 0;
            }

            Point current = r_BoardSizeList[m_CurrentBoardSizeIndex];
            buttonBoardSize.Text = string.Format(format: "{0}x{1}", current.X, current.Y);
        }

        private void buttonAgainst_Click(object i_Sender, EventArgs i_)
        {
            Button buttonAgainst = i_Sender as Button;
            if(textBoxSecondPlayer.Enabled == false)
            {
                buttonAgainst.Text = @"Against a computer";
                textBoxSecondPlayer.Enabled = true;
                textBoxSecondPlayer.Text = string.Empty;
            }
            else
            {
                textBoxSecondPlayer.Enabled = false;
                textBoxSecondPlayer.Text = @"- Computer -";
                buttonAgainst.Text = @"Against a friend";
            }
        }

        private void buttonStart_Click(object i_Sender, EventArgs i_E)
        {
            if(ensureNameHasValue())
            {
                MessageBox.Show(
                    @"Name cannot be empty!", 
                    @"Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else
            {
                bool isAgainstComputer = textBoxSecondPlayer.Enabled == false;
                m_BoardForm = new GameBoardForm(FirstPlayerName, SecondPlayerName, BoardSize, isAgainstComputer);
                Hide();
                m_BoardForm.ShowDialog();
            }
        }

        private bool ensureNameHasValue()
        {
            return string.IsNullOrWhiteSpace(textBoxFirstName.Text) 
                   || string.IsNullOrWhiteSpace(textBoxSecondPlayer.Text);
        }
    }
}
