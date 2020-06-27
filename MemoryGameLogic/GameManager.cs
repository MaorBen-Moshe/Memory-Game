using System;

namespace MemoryGameLogic
{
    public delegate void OnCurrentCardChosen(out byte i_CurrentLine, out byte i_CurrentColom);

    public class GameManager
    {
        public event OnCurrentCardChosen OnPlayerChooseCard;

        public event Action OnGameEnd;

        private enum eRound
        {
            FirstRound,
            SecondRound,
            EndRound
        }

        private readonly GameBoard r_GameBoard;
        private readonly Random r_Rnd;
        private readonly Player r_FirstPlayer;
        private readonly Player r_SecondPlayer; // can be computer or regular
        private Player m_CurrentPlayer;
        private bool m_GameToggle;
        private byte m_FirstLineChosen;
        private byte m_FirstColomChosen;
        private byte m_FirstValueFound;
        private byte m_SecondLineChosen;
        private byte m_SecondColomChosen;
        private byte m_SecondValueFound;
        private eRound m_CurrentRound = eRound.FirstRound;

        public GameManager(
                           string i_FirstPlayer,
                           string i_SecondPlayer,
                           int i_BoardLines,
                           int i_BoardColoms,
                           bool i_IsAgainstComputer)
        {
            r_GameBoard = new GameBoard((byte)i_BoardLines, (byte)i_BoardColoms);
            r_FirstPlayer = new Player(i_FirstPlayer);
            r_SecondPlayer = i_IsAgainstComputer ?
                                 new ComputerPlayer((byte)i_BoardLines, (byte)i_BoardColoms)
                                 : new Player(i_SecondPlayer);
            r_Rnd = new Random();
            setRandomlyCurrentPlayer();
        }

        public byte this[int i_Line, int i_Colom]
        {
            get
            {
                return r_GameBoard[(byte)i_Line, (byte)i_Colom].Content;
            }
        }

        public Player CurrentPlayer
        {
            get
            {
                return m_CurrentPlayer;
            }
        }

        public Player FirstPlayer
        {
            get
            {
                return r_FirstPlayer;
            }
        }

        public Player SecondPlayer
        {
            get
            {
                return r_SecondPlayer;
            } 
        }

        public ComputerPlayer Computer
        {
            get
            {
                ComputerPlayer toReturn = null;
                if(r_SecondPlayer is ComputerPlayer)
                {
                    toReturn = r_SecondPlayer as ComputerPlayer;
                }

                return toReturn;
            }
        }

        public bool IsGameEnds
        {
            get
            {
                return r_FirstPlayer.PairsCount + r_SecondPlayer.PairsCount == (r_GameBoard.Lines * r_GameBoard.Coloms) / 2;
            }
        }

        public int BoardLines
        {
            get
            {
                return r_GameBoard.Lines;
            }
        }

        public int BoardColoms
        {
            get
            {
                return r_GameBoard.Coloms;
            }
        }

        public string WinnerName
        {
            get
            {
                string winner = null; // if the game ends with a draw winner will stay null
                if (r_FirstPlayer.PairsCount > r_SecondPlayer.PairsCount)
                {
                    winner = r_FirstPlayer.Name;
                }
                else if (r_SecondPlayer.PairsCount > r_FirstPlayer.PairsCount)
                {
                    winner = r_SecondPlayer.Name;
                }

                return winner;
            }
        }

        public void RunGame()
        {
            bool playerFoundPair = m_GameToggle 
                                       ? playerTurn(r_FirstPlayer) 
                                       : setRivalTurn();
            if (m_CurrentRound == eRound.EndRound)
            {
                m_GameToggle = playerFoundPair ? m_GameToggle : !m_GameToggle; 
                m_CurrentPlayer = m_GameToggle ? r_FirstPlayer : r_SecondPlayer; 
                m_CurrentRound = eRound.FirstRound;
            }

            if(IsGameEnds)
            {
                OnGameEnd?.Invoke();
            }
        }

        public void setNewGameValues()
        {
            r_GameBoard.Clear();
            r_GameBoard.intialBoardWithValues();
            r_FirstPlayer.PairsCount = 0;
            r_SecondPlayer.PairsCount = 0;
            if(r_SecondPlayer is ComputerPlayer)
            {
                (r_SecondPlayer as ComputerPlayer).SetNewGameValues((byte)BoardLines, (byte)BoardColoms, Computer.Ai);
            }

            setRandomlyCurrentPlayer();
        }

        public bool IsCellRevealed(byte i_Line, byte i_Colom)
        {
            return r_GameBoard[i_Line, i_Colom].IsRevealed;
        }

        private bool setRivalTurn()
        {
            return r_SecondPlayer is ComputerPlayer
                       ? computerTurn()
                       : playerTurn(r_SecondPlayer);
        }

        private bool computerTurn()
        {
            bool matchFound = false;
            (r_SecondPlayer as ComputerPlayer).PlayTurn(
                r_GameBoard,
                out byte firstLine,
                out byte firstColom,
                out byte secondLine,
                out byte secondColom);
            if (r_GameBoard[firstLine, firstColom].Content == r_GameBoard[secondLine, secondColom].Content)
            {
                r_SecondPlayer.PairsCount++;
                matchFound = true;
            }
            else
            {
                r_GameBoard[firstLine, firstColom].IsRevealed = false;
                r_GameBoard[secondLine, secondColom].IsRevealed = false;
            }

            m_CurrentRound = eRound.EndRound;
            return matchFound;
        }

        private bool playerTurn(Player i_CurrPlayer)
        {
            bool playerSucceeded = false;

            switch (m_CurrentRound)
            {
                case eRound.FirstRound:
                    playerTurnHandler(i_CurrPlayer, eRound.SecondRound, out m_FirstLineChosen, out m_FirstColomChosen, out m_FirstValueFound);
                    break;
                case eRound.SecondRound:
                    playerTurnHandler(i_CurrPlayer, eRound.EndRound, out m_SecondLineChosen, out m_SecondColomChosen, out m_SecondValueFound);
                    break;
            }

            if(m_CurrentRound == eRound.EndRound)
            {
                (r_SecondPlayer as ComputerPlayer)?.ComputerLearn(
                    m_FirstLineChosen,
                    m_FirstColomChosen,
                    m_FirstValueFound,
                    m_SecondLineChosen,
                    m_SecondColomChosen,
                    m_SecondValueFound);

                if (r_GameBoard[m_FirstLineChosen, m_FirstColomChosen].Content == r_GameBoard[m_SecondLineChosen, m_SecondColomChosen].Content)
                {
                    i_CurrPlayer.PairsCount++;
                    playerSucceeded = true;
                }
                else
                {
                    r_GameBoard[m_FirstLineChosen, m_FirstColomChosen].IsRevealed = false;
                    r_GameBoard[m_SecondLineChosen, m_SecondColomChosen].IsRevealed = false;
                }
            }

            return playerSucceeded;
        }

        private void playerTurnHandler(Player i_CurrPlayer, eRound i_NextRound, out byte o_LineChosen, out byte o_ColomChosen, out byte o_ValueOfCell)
        {
            OnCurrentCardChosen(out o_LineChosen, out o_ColomChosen);
            i_CurrPlayer.PlayTurn(r_GameBoard, o_LineChosen, o_ColomChosen);
            o_ValueOfCell = r_GameBoard[o_LineChosen, o_ColomChosen].Content;
            m_CurrentRound = i_NextRound;
        }

        private void setRandomlyCurrentPlayer()
        {
            if(Computer != null)
            {
                m_GameToggle = true;
                m_CurrentPlayer = r_FirstPlayer;
            }
            else
            {
                m_GameToggle = r_Rnd.Next(2) == 0; // randomly pick true or false, the start player will change
                m_CurrentPlayer = m_GameToggle ? r_FirstPlayer : r_SecondPlayer;
            }
        }

        protected virtual void OnCurrentCardChosen(out byte i_CurrentLine, out byte i_CurrentColom)
        {
            i_CurrentLine = i_CurrentColom = default;
            OnPlayerChooseCard?.Invoke(out i_CurrentLine, out i_CurrentColom);
        }
    }
}
