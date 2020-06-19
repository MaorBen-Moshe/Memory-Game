using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MemoryGameLogic;

namespace WindowsAPI
{
    public partial class GameBoardForm : Form
    {
        private Point m_BoardSize;
        private readonly Random r_Rnd = new Random();
        private readonly Dictionary<byte, char> r_GameValues;
        private readonly GameBoard r_GameBoard;
        private readonly GameManager r_GameControler;

        public GameBoardForm(string i_FirstPlayerName, string i_SecondPlayerName, Point i_BoardSize, bool i_AgainstComputer)
        {
            InitializeComponent();
            m_BoardSize = i_BoardSize;
            r_GameValues = new Dictionary<byte, char>();
            r_GameBoard = new GameBoard((byte)m_BoardSize.X, (byte)m_BoardSize.Y);
            //r_GameControler = new GameManager();
            labelFirstPlayer.Text = i_FirstPlayerName;
            labelFirstPlayer.BackColor = Color.Turquoise;
            labelSecondPlayer.Text = i_AgainstComputer ? "Computer" : i_SecondPlayerName;
            labelSecondPlayer.BackColor = Color.BurlyWood;
            labelFirstPlayerPairs.Text = labelSecondPlayerPairs.Text = @"0 Pair(s)";
            createBoard();
        }

        private void createBoard()
        {
            int boardSize = m_BoardSize.X * m_BoardSize.Y;
            int buttonHeight = Height / boardSize;
            int buttonWidth = Width / boardSize;

            for(int i = 0; i < m_BoardSize.X; i++)
            {
                for(int j = 0; j < m_BoardSize.Y; j++)
                {
                    Button current = new Button 
                                          { 
                                              Enabled = true,
                                              AutoSize = true,
                                              Size = new Size(new Point(100, 100)),
                                              Name = String.Format(format: "{0},{1}", i, j),
                                              Tag = new int[2] { i, j },
                                              Left = buttonWidth * j,
                                              Top = buttonHeight * i,
                                              Height = buttonHeight,
                                              Width = buttonWidth
                                          };
                    
                    current.Click += card_Click;
                    Matrix.Controls.Add(current);
                }
            }

            CreateBoardValues();
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
            string[] buttonPlace = i_Current.Name.Split(',');
            byte line = byte.Parse(buttonPlace[0]);
            byte colom = byte.Parse(buttonPlace[1]);
            Matrix.Controls[colom + (m_BoardSize.Y * line)].BackColor = labelFirstPlayer.BackColor;
            Matrix.Controls[colom + (m_BoardSize.Y * line)].Text = r_GameValues[r_GameBoard[line, colom].Content].ToString();
        }

        private void checkForWinners()
        {
            foreach(Button current in Matrix.Controls)
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
