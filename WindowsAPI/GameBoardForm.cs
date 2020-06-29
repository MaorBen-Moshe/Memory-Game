using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MemoryGameLogic;

namespace WindowsAPI
{
    public partial class GameBoardForm : Form 
    {
        private const int k_CardStartHorizonPos = 50;
        private const int k_CardStartVerticalPos = 10;
        private const int k_CardWidth = 80;
        private const int k_CardHeight = 77;
        
        private readonly Random r_Rnd = new Random();
        private readonly Dictionary<byte, char> r_GameValues;
        private readonly GameManager r_GameControler;
        private readonly Button[,] r_Matrix;

        private int m_BoardCardRight = 90;
        private int m_BoardCardDown = 90;
        private Button m_FirstClicked;
        private Button m_SecondClicked;
        private byte m_CurrentButtonLine;
        private byte m_CurrentButtonColom;

        public GameBoardForm(
            string i_FirstPlayerName,
            string i_SecondPlayerName,
            Point i_BoardSize,
            bool i_IsAgainstComputer)
        {
            InitializeComponent();
            r_GameValues = new Dictionary<byte, char>();
            r_GameControler = new GameManager(
                i_FirstPlayerName,
                i_SecondPlayerName,
                i_BoardSize.X,
                i_BoardSize.Y,
                i_IsAgainstComputer);
            r_Matrix = new Button[i_BoardSize.X, i_BoardSize.Y];
            initialBoardButtons();
            initialStatisticsPanel();
            initialObservers();
            r_GameControler.StartGame();
        }

        private void initialObservers()
        {
            r_GameControler.OnPlayerChooseCard += GameControler_OnPlayerChooseCard;
            r_GameControler.FirstPlayer.OnPlayerMove += Player_OnPlayerMove;
            r_GameControler.SecondPlayer.OnPlayerMove += Player_OnPlayerMove;
            r_GameControler.OnGameEnd += GameContorler_OnGameEnd;
            r_GameControler.OnAgainstComputer += GameControler_OnAgainstComputer;
            r_GameControler.OnPlayerTurnEnd += GameControler_OnPlayerTurnEnd;
        }

        private void GameControler_OnPlayerTurnEnd()
        {
            setStatisticsGamePanel();
        }

        private void initialStatisticsPanel()
        {
            setStatisticsGamePanel();
            labelFirstPlayerPairs.Left = labelFirstPlayer.Left + labelFirstPlayer.Width + 10;
            labelSecondPlayerPairs.Left = labelSecondPlayer.Left + labelSecondPlayer.Width + 10;
            panelStatistics.Top = r_Matrix[r_GameControler.BoardLines - 1, 0].Bottom + 20;
            panelStatistics.Left += r_Matrix[r_GameControler.BoardLines - 1, 0].Left - 10;
        }

        private void setStatisticsGamePanel()
        {
            labelFirstPlayer.Text = r_GameControler.FirstPlayerName;
            labelFirstPlayer.BackColor = Color.MediumPurple;
            labelFirstPlayerPairs.BackColor = labelFirstPlayer.BackColor;
            labelSecondPlayer.Text = r_GameControler.SecondPlayerName;
            labelSecondPlayer.BackColor = Color.MediumSeaGreen;
            labelSecondPlayerPairs.BackColor = labelSecondPlayer.BackColor;
            labelFirstPlayerPairs.Text = string.Format(format: @"{0} Pair(s)", r_GameControler.FirstPlayerPairsCount);
            labelSecondPlayerPairs.Text = string.Format(format: @"{0} Pair(s)", r_GameControler.SecondPlayerPairsCount);
            labelCurrentPlayerName.Text = r_GameControler.CurrentPlayerName;
            labelCurrentPlayerName.BackColor = r_GameControler.CurrentPlayerName.Equals(r_GameControler.FirstPlayerName)
                                                   ? labelFirstPlayer.BackColor
                                                   : labelSecondPlayer.BackColor;
            labelCurrentPlayer.BackColor = labelCurrentPlayerName.BackColor;
        }

        private void GameControler_OnAgainstComputer()
        {
            string askForLevel = string.Format(
                format: @"Would you like the play in:
1.Hard Mode - press Yes
2.Easy Mode - press No");
            DialogResult result = MessageBox.Show(
                askForLevel,
                @"Game Level",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            r_GameControler.Ai = result == DialogResult.Yes;
        }

        private void GameControler_OnPlayerChooseCard(out byte i_Currentline, out byte i_Currentcolom)
        {
            i_Currentline = m_CurrentButtonLine;
            i_Currentcolom = m_CurrentButtonColom;
        }

        private void Card_Click(object i_Sender, EventArgs i_E)
        {
            if (!r_GameControler.IsCurrentComputer)
            {
                Button current = i_Sender as Button;
                string[] locationOfButton = current.Name.Split(' ');
                m_CurrentButtonLine = byte.Parse(locationOfButton[0]);
                m_CurrentButtonColom = byte.Parse(locationOfButton[1]);
                if (m_FirstClicked == null || m_SecondClicked == null)
                {
                    if (r_GameControler.IsCellRevealed(m_CurrentButtonLine, m_CurrentButtonColom) == false)
                    {
                        r_GameControler.RunGame();
                    }
                }
            }
        }

        private void Player_OnPlayerMove(byte i_CurrentLine, byte i_CurrentColom)
        {
            cardChooseHandler(i_CurrentLine, i_CurrentColom);
            if (m_FirstClicked == null)
            {
                m_FirstClicked = r_Matrix[i_CurrentLine, i_CurrentColom];
                return;
            }

            m_SecondClicked = r_Matrix[i_CurrentLine, i_CurrentColom];
            TimerCards.Start();
        }

        private void TimerCards_Tick(object i_Sender, EventArgs i_E)
        {
            TimerCards.Stop();
            if (m_FirstClicked.Text != m_SecondClicked.Text)
            {
                m_FirstClicked.BackColor = m_SecondClicked.BackColor = default;
                m_FirstClicked.Text = m_SecondClicked.Text = null;
            }
            else
            {
                System.Threading.Thread.Sleep(1500);
            }

            m_FirstClicked = m_SecondClicked = null;
            if (!r_GameControler.IsGameEnds && r_GameControler.IsCurrentComputer)
            {
                    r_GameControler.RunGame();
            }
        }

        private void GameContorler_OnGameEnd()
        {
            string winnerName = r_GameControler.WinnerName;
            StringBuilder winnerMessage = new StringBuilder();
            winnerMessage.Append(winnerName != null
                                       ? string.Format(
                                           format: @"Congrats! {0} won the game.
", 
                                           winnerName)
                                       : "The game ends with a draw.\n");
            winnerMessage.Append("Would you like to have another game?");
            DialogResult dialogResult = MessageBox.Show(
                this,
                winnerMessage.ToString(),
                @"Winner",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information);

            if (dialogResult == DialogResult.No)
            {
                MessageBox.Show(@"Thank you for using our app! hope to see you soon!", @"Goodbye");
                this.Close();
            }
            else
            {
                setNewGame();
            }
        }

        private void cardChooseHandler(byte i_CurrentLine, byte i_CurrentColom)
        {
            r_Matrix[i_CurrentLine, i_CurrentColom].BackColor = labelCurrentPlayer.BackColor;
            r_Matrix[i_CurrentLine, i_CurrentColom].Text = r_GameValues[r_GameControler[i_CurrentLine, i_CurrentColom]].ToString();
        }

        private void setNewGame()
        {
            m_FirstClicked = m_SecondClicked = null;
            r_GameControler.setNewGameValues();
            createBoardValues();
            cleanButtons();
            setStatisticsGamePanel();
        }

        private void cleanButtons()
        {
            foreach (Button currentButton in r_Matrix)
            {
                currentButton.Text = null;
                currentButton.BackColor = default;
            }
        }

        private void initialBoardButtons()
        {
            int firstHorizonPos = k_CardStartHorizonPos;
            int firstVerticalPos = k_CardStartVerticalPos;

            for (int i = 0; i < r_GameControler.BoardLines; i++)
            {
                for (int j = 0; j < r_GameControler.BoardColoms; j++)
                {
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

            this.Width = r_Matrix[r_GameControler.BoardLines - 1, r_GameControler.BoardColoms - 1].Right + r_Matrix[0, 0].Left;
            this.Height = r_Matrix[r_GameControler.BoardLines - 1, r_GameControler.BoardColoms - 1].Bottom + 170;
            createBoardValues();
        }

        private void setButtonDesign(Button i_CurrentButton, int i_HorizonPos, int i_VerticalPos)
        {
            i_CurrentButton.Location = new Point(i_HorizonPos, i_VerticalPos);
            i_CurrentButton.Size = new Size(k_CardWidth, k_CardHeight);
            i_CurrentButton.Click += Card_Click;
        }

        private void createBoardValues()
        {
            int pairsOnBoard = (r_GameControler.BoardLines * r_GameControler.BoardColoms) / 2;
            List<char> inputValues = new List<char>(pairsOnBoard);
            char ch = 'A';
            for (int i = 0; i < pairsOnBoard; i++)
            {
                inputValues.Add(ch++);
            }

            r_GameValues.Clear();
            for (byte i = 0; i < r_GameControler.BoardLines; i++)
            {
                for (byte j = 0; j < r_GameControler.BoardColoms; j++)
                {
                    if (r_GameValues.ContainsKey(r_GameControler[i, j]))
                    {
                        continue;
                    }

                    char currentLetter = pickRandomLetter(inputValues);
                    r_GameValues.Add(r_GameControler[i, j], currentLetter);
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
