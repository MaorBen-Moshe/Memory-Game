using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MemoryGame;

namespace WindowsAPI
{
    public partial class GameBoardForm : Form
    {
        private Point m_BoardSize;
        private readonly Random r_Rnd = new Random();
        private readonly Dictionary<byte, char> r_GameValues;
        private readonly Button[,] r_GameButtons;

        public GameBoardForm(string i_FirstPlayerName, string i_SecondPlayerName, Point i_BoardSize)
        {
            InitializeComponent();
            /// getting inputs from user
            m_BoardSize = i_BoardSize;
            r_GameValues = new Dictionary<byte, char>();
            r_GameButtons = new Button[m_BoardSize.X, m_BoardSize.Y];
            ///update my inputs
            labelFirstPlayer.Text = i_FirstPlayerName;
            labelFirstPlayer.BackColor = Color.DarkRed;
            labelSecondPlayer.Text = i_SecondPlayerName;
            labelSecondPlayer.BackColor = Color.DarkBlue;
            labelCurrentPlayer.Text = i_FirstPlayerName; // need to be random
            labelCurrentPlayer.BackColor = labelFirstPlayer.BackColor;
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
                    r_GameButtons[i, j] = new Button 
                                          { 
                                              Enabled = true,
                                              AutoSize = true,
                                              Size = new Size(new Point(80, 80)),
                                              Name = (j + (m_BoardSize.Y * i)).ToString(),
                                              Tag = new int[2] { i, j },
                                              Left = buttonWidth * j,
                                              Top = buttonHeight * i,
                                              Height = buttonHeight,
                                              Width = buttonWidth
                                          };
                    r_GameButtons[i, j].Click += card_Click;
                    Controls.Add(r_GameButtons[i, j]);
                }
            }
        }

        private void card_Click(object i_Sender, EventArgs i_E)
        {
        }

        internal void CreateBoardValues(GameBoard i_GameBoard)
        {
            int pairsOnBoard = (i_GameBoard.Lines * i_GameBoard.Coloms) / 2;
            List<char> inputValues = new List<char>(pairsOnBoard);
            char ch = 'A';
            for (int i = 0; i < pairsOnBoard; i++)
            {
                inputValues.Add(ch++);
            }

            r_GameValues.Clear();
            for (byte i = 0; i < i_GameBoard.Lines; i++)
            {
                for (byte j = 0; j < i_GameBoard.Coloms; j++)
                {
                    if (r_GameValues.ContainsKey(i_GameBoard[i, j].Content))
                    {
                        continue;
                    }

                    char currentLetter = pickRandomLetter(inputValues);
                    r_GameValues.Add(i_GameBoard[i, j].Content, currentLetter);
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
    }
}
