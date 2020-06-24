using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsAPI
{
    public partial class SettingsForm : Form
    {
        private const int k_MinimumBoardLines = 4;
        private const int k_MaximumBoardLines = 6;
        private const int k_MinimumBoardColoms = 4;
        private const int k_MaximumBoardColoms = 6;
        private readonly List<Point> r_BoardSizeList = new List<Point>();
        private byte m_CurrentBoardSizeIndex;
        private GameBoardForm m_BoardForm;

        internal SettingsForm()
        {
            intialBoardSizes();
            InitializeComponent();
        }

        private void intialBoardSizes()
        {
            for(int i = k_MinimumBoardLines; i <= k_MaximumBoardLines; i++)
            {
                for(int j = k_MinimumBoardColoms; j <= k_MaximumBoardColoms; j++)
                {
                    if((i * j) % 2 == 0)
                    {
                        r_BoardSizeList.Add(new Point(i, j));
                    }
                }
            }
        }

        private string FirstPlayerName
        {
            get
            {
                return this.textBoxFirstName.Text;
            }
        }

        private string SecondPlayerName
        {
            get
            {
                return this.textBoxSecondPlayer.Text;
            }
        }

        private Point BoardSize
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
                    this,
                    @"Player name cannot be empty!", 
                    @"Name Error",
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

        private void textBox_TextChanged(object i_Sender, EventArgs i_E)
        {
            TextBox current = i_Sender as TextBox;
            if(current?.Text.Length > 12)
            {
                MessageBox.Show(
                    this,
                    @"Name can has 12 letters at the most!",
                    @"Name Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
