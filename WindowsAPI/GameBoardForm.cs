using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MemoryGameLogic;

namespace WindowsAPI
{
    public partial class GameBoardForm : Form
    {
        // in usage when we build the Matrix Buttons
        private const int k_CardStartHorizonPos = 50;
        private const int k_CardStartVerticalPos = 10;
        private const int k_CardWidth = 80;
        private const int k_CardHeight = 77;

        private readonly Random r_Rnd = new Random();
        private readonly Dictionary<byte, char> r_GameValues;
        private readonly GameBoard r_GameBoard;
        private readonly GameManager r_GameControler;
        private readonly Button[,] r_Matrix;
        private Point m_BoardSize;

        public GameBoardForm(string i_FirstPlayerName, string i_SecondPlayerName, Point i_BoardSize, bool i_AgainstComputer)
        {
            InitializeComponent();
            m_BoardSize = i_BoardSize;
            r_GameValues = new Dictionary<byte, char>();
            r_GameBoard = new GameBoard((byte)m_BoardSize.X, (byte)m_BoardSize.Y);
            r_GameControler = new GameManager(i_FirstPlayerName, i_SecondPlayerName, r_GameBoard.Lines, r_GameBoard.Coloms, i_AgainstComputer);
            r_Matrix = new Button[m_BoardSize.X, m_BoardSize.Y];
            initialBoardButtons();
            setStatisticsPanel(i_FirstPlayerName, i_SecondPlayerName, i_AgainstComputer);
        }

        private int m_BoardCardRight = 90;
        private int m_BoardCardDown = 90;

        private void initialBoardButtons()
        {
            int firstHorizonPos = k_CardStartHorizonPos;
            int firstVerticalPos = k_CardStartVerticalPos;

            for(int i = 0; i < m_BoardSize.X; i++)
            {
                for(int j = 0; j < m_BoardSize.Y; j++)
                {
                    r_GameBoard[(byte)i, (byte)j].IsRevealed = false;
                    r_Matrix[i, j] = new Button
                                     {
                                         Name = i + " " + j
                                     };
                    setButtonDesign(r_Matrix[i, j], firstHorizonPos, firstVerticalPos);
                    firstHorizonPos += m_BoardCardRight;
                    this.Controls.Add(r_Matrix[i, j]);
                }

                firstHorizonPos = k_CardStartHorizonPos;
                firstVerticalPos += m_BoardCardDown;
            }

            this.Width = r_Matrix[m_BoardSize.X - 1, m_BoardSize.Y - 1].Right + r_Matrix[0, 0].Left;
            this.Height = r_Matrix[m_BoardSize.X - 1, m_BoardSize.Y - 1].Bottom + 170;
            CreateBoardValues();
        }

        private void setButtonDesign(Button i_CurrentButton, int i_HorizonPos, int i_VerticalPos)
        {
            i_CurrentButton.Location = new Point(i_HorizonPos, i_VerticalPos);
            i_CurrentButton.Size = new Size(k_CardWidth, k_CardHeight);
            i_CurrentButton.Click += card_Click;
        }

        private void setStatisticsPanel(string i_FirstPlayerName, string i_SecondPlayerName, bool i_AgainstComputer)
        {
            labelFirstPlayer.Text = i_FirstPlayerName;
            labelFirstPlayer.BackColor = Color.Turquoise;
            labelSecondPlayer.Text = i_AgainstComputer ? "Computer" : i_SecondPlayerName;
            labelSecondPlayer.BackColor = Color.BurlyWood;
            labelFirstPlayerPairs.Text = labelSecondPlayerPairs.Text = @"0 Pair(s)";
            labelCurrentPlayerName.Text = r_GameControler.CurrentPlayer.Name;
            labelCurrentPlayerName.BackColor = r_GameControler.CurrentPlayer.Name == i_FirstPlayerName
                                                   ? labelFirstPlayer.BackColor
                                                   : labelSecondPlayer.BackColor;
            labelCurrentPlayer.BackColor = labelCurrentPlayerName.BackColor;

            panelStatistics.Top = r_Matrix[m_BoardSize.X - 1, 0].Bottom + 20;
            panelStatistics.Left += r_Matrix[m_BoardSize.X - 1, 0].Left - 10;
        }

        private Button m_FirstClicked;
        private Button m_SecondClicked;

        private void card_Click(object i_Sender, EventArgs i_E)
        {
            Button current = i_Sender as Button;
            if(m_FirstClicked == null || m_SecondClicked == null)
            {
                if (current.Text == string.Empty)
                {
                    if (m_FirstClicked == null)
                    {
                        m_FirstClicked = i_Sender as Button;
                        cardClickHandler(m_FirstClicked);
                        return;
                    }

                    m_SecondClicked = i_Sender as Button;
                    cardClickHandler(m_SecondClicked);
                    checkForWinners();
                    TimerCards.Start();
                }
            }
        }

        private void cardClickHandler(Button i_Current)
        {
            string[] buttonPlace = i_Current.Name.Split(' ');
            byte line = byte.Parse(buttonPlace[0]);
            byte colom = byte.Parse(buttonPlace[1]);
            r_Matrix[line, colom].BackColor = labelFirstPlayer.BackColor;
           r_Matrix[line, colom].Text = r_GameValues[r_GameBoard[line, colom].Content].ToString();
        }

        private void checkForWinners()
        {
            foreach(Button current in r_Matrix)
            {
                if(current.Text == string.Empty)
                {
                    return;
                }
            }

            DialogResult dialogResult = MessageBox.Show(
                "Congrats! you won the game!",
                "Winner",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information);
            if (dialogResult == DialogResult.No)
            {
                this.Close();
            }
        }

        internal void CreateBoardValues()
        {
            int pairsOnBoard = (r_GameBoard.Lines * r_GameBoard.Coloms) / 2;
            List<char> inputValues = new List<char>(pairsOnBoard);
            char ch = 'A';
            for (int i = 0; i < pairsOnBoard; i++)
            {
                inputValues.Add(ch++);
            }

            r_GameValues.Clear();
            for (byte i = 0; i < r_GameBoard.Lines; i++)
            {
                for (byte j = 0; j < r_GameBoard.Coloms; j++)
                {
                    if (r_GameValues.ContainsKey(r_GameBoard[i, j].Content))
                    {
                        continue;
                    }

                    char currentLetter = pickRandomLetter(inputValues);
                    r_GameValues.Add(r_GameBoard[i, j].Content, currentLetter);
                }
            }
        }

        private char pickRandomLetter(List<char> i_LettersList)
        {
            int randomPlace = r_Rnd.Next() % i_LettersList.Count;
            char randomLetter = i_LettersList[randomPlace];
            i_LettersList.Remove(randomLetter);
            return randomLetter;
        }

        private void TimerCards_Tick(object i_Sender, EventArgs i_E)
        {
            TimerCards.Stop();
            if (m_FirstClicked.Text != m_SecondClicked.Text)
            {
                m_FirstClicked.BackColor = m_SecondClicked.BackColor = default;
                m_FirstClicked.Text = m_SecondClicked.Text = null;
            }

            m_FirstClicked = m_SecondClicked = null;
        }
    }
}
