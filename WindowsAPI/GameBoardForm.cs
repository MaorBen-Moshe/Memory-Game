using System;
using System.Collections.Generic;
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
            r_GameControler = new GameManager(i_FirstPlayerName, i_SecondPlayerName, i_BoardSize.X, i_BoardSize.Y, i_IsAgainstComputer);
            r_Matrix = new Button[i_BoardSize.X, i_BoardSize.Y];
            initialBoardButtons();
            intialStatisticsPanel();
            r_GameControler.OnPlayerChooseCard += GameControler_OnPlayerChooseCard;
            r_GameControler.FirstPlayer.OnPlayerTurn += Player_OnPlayerTurn;
            if(i_IsAgainstComputer)
            {
                setGameLevel();
                r_GameControler.Computer.OnComputerChoose += Computer_OnComputerChoose;
            }
            else
            {
                r_GameControler.SecondPlayer.OnPlayerTurn += Player_OnPlayerTurn;
            }
        }

        private void GameControler_OnPlayerChooseCard(out byte i_Currentline, out byte i_Currentcolom)
        {
            i_Currentline = m_CurrentButtonLine;
            i_Currentcolom = m_CurrentButtonColom;
        }

        private void setGameLevel()
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
            r_GameControler.Computer.Ai = result == DialogResult.Yes;
        }
        
        private void Computer_OnComputerChoose(ref Player.Point i_FirstChoose, ref Player.Point i_SecondChoose)
        {
            cardChooseHandler(i_FirstChoose.Line, i_FirstChoose.Colom);
            cardChooseHandler(i_SecondChoose.Line, i_SecondChoose.Colom);
            m_FirstClicked = r_Matrix[i_FirstChoose.Line, i_FirstChoose.Colom];
            m_SecondClicked = r_Matrix[i_SecondChoose.Line, i_SecondChoose.Colom];
            TimerCards.Start();
        }

        private void initialBoardButtons()
        {
            int firstHorizonPos = k_CardStartHorizonPos;
            int firstVerticalPos = k_CardStartVerticalPos;

            for(int i = 0; i < r_GameControler.BoardLines; i++)
            {
                for(int j = 0; j < r_GameControler.BoardColoms; j++)
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

        private void Card_Click(object i_Sender, EventArgs i_E)
        {
            if(!(r_GameControler.CurrentPlayer is ComputerPlayer))
            {
                Button current = i_Sender as Button;
                string[] locationOfButton = current.Name.Split(' ');
                m_CurrentButtonLine = byte.Parse(locationOfButton[0]);
                m_CurrentButtonColom = byte.Parse(locationOfButton[1]);
                if (m_FirstClicked == null || m_SecondClicked == null)
                {
                    if (string.IsNullOrEmpty(r_Matrix[m_CurrentButtonLine, m_CurrentButtonColom].Text))
                    {
                        r_GameControler.RunGame();
                        if (m_FirstClicked == null)
                        {
                            m_FirstClicked = i_Sender as Button;
                            return;
                        }

                        m_SecondClicked = i_Sender as Button;
                        TimerCards.Start();
                    }
                }
            }
        }

        private void cardChooseHandler(byte i_CurrentLine, byte i_CurrentColom)
        {
            r_Matrix[i_CurrentLine, i_CurrentColom].BackColor = labelCurrentPlayer.BackColor;
            r_Matrix[i_CurrentLine, i_CurrentColom].Text = r_GameValues[r_GameControler[i_CurrentLine, i_CurrentColom]].ToString();
        }

        private void setStatisticsGamePanel()
        {
            labelFirstPlayer.Text = r_GameControler.FirstPlayer.Name;
            labelFirstPlayer.BackColor = Color.MediumPurple;
            labelFirstPlayerPairs.BackColor = labelFirstPlayer.BackColor;
            labelSecondPlayer.Text = r_GameControler.SecondPlayer.Name;
            labelSecondPlayer.BackColor = Color.MediumSeaGreen;
            labelSecondPlayerPairs.BackColor = labelSecondPlayer.BackColor;
            labelFirstPlayerPairs.Text = string.Format(format: @"{0} Pair(s)", r_GameControler.FirstPlayer.PairsCount);
            labelSecondPlayerPairs.Text = string.Format(format: @"{0} Pair(s)", r_GameControler.SecondPlayer.PairsCount);
            labelCurrentPlayerName.Text = r_GameControler.CurrentPlayer.Name;
            labelCurrentPlayerName.BackColor = r_GameControler.CurrentPlayer.Name.Equals(r_GameControler.FirstPlayer.Name)
                                                   ? labelFirstPlayer.BackColor
                                                   : labelSecondPlayer.BackColor;
            labelCurrentPlayer.BackColor = labelCurrentPlayerName.BackColor;
        }

        private void intialStatisticsPanel()
        {
            setStatisticsGamePanel();
            labelFirstPlayerPairs.Left = labelFirstPlayer.Left + labelFirstPlayer.Width + 10;
            labelSecondPlayerPairs.Left = labelSecondPlayer.Left + labelSecondPlayer.Width + 10;
            panelStatistics.Top = r_Matrix[r_GameControler.BoardLines - 1, 0].Bottom + 20;
            panelStatistics.Left += r_Matrix[r_GameControler.BoardLines - 1, 0].Left - 10;
        }

        private void Player_OnPlayerTurn(byte i_CurrentLine, byte i_CurrentColom)
        {
            cardChooseHandler(i_CurrentLine, i_CurrentColom);
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
            setStatisticsGamePanel();
            if (r_GameControler.IsGameEnds)
            {
                setWinnersMessage();
            }
            else if (r_GameControler.Computer != null && r_GameControler.CurrentPlayer is ComputerPlayer)
            {
                    r_GameControler.RunGame();
            }
        }

        private void setWinnersMessage()
        {
            setStatisticsGamePanel();
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
