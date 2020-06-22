using System;

namespace MemoryGameLogic
{
    public class GameManager
    {
        public event Action OnComputerTurn;

        private enum eRound
        {
            FirstRound,
            SecondRound,
            EndRound
        }

        private readonly IObserver r_Observer;
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
                           IObserver i_Observer,
                           bool i_AgainstComputer = false)
        {
            r_GameBoard = new GameBoard((byte)i_BoardLines, (byte)i_BoardColoms);
            r_FirstPlayer = new Player(i_FirstPlayer);
            r_SecondPlayer = i_AgainstComputer ?
                                 new ComputerPlayer((byte)i_BoardLines, (byte)i_BoardColoms)
                                 : new Player(i_SecondPlayer);
            r_Observer = i_Observer;
            r_Rnd = new Random();
            m_GameToggle = r_Rnd.Next(2) == 0; // randomly pick true or false, the start player will change
            m_CurrentPlayer = m_GameToggle ? r_FirstPlayer : r_SecondPlayer;
        }

        public Player CurrentPlayer
        {
            get
            {
                return m_CurrentPlayer;
            }

            set
            {
                m_CurrentPlayer = value;
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

        public GameBoard GameBoard
        {
            get
            {
                return r_GameBoard;
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

        public int FirstPlayerPairs
        {
            get
            {
                return r_FirstPlayer.PairsCount;
            }

            set
            {
                r_FirstPlayer.PairsCount = 0;
            }
        }

        public int SecondPlayerPairs
        {
            get
            {
                return r_SecondPlayer.PairsCount;
            }

            set
            {
                r_SecondPlayer.PairsCount = 0;
            }
        }

        public string FirstPlayerName
        {
            get
            {
                return r_FirstPlayer.Name;
            }
        }

        public string SecondPlayerName
        {
            get
            {
                return r_SecondPlayer.Name;
            }
        }

        public bool PlayGame()
        {
            bool playerFoundPair = m_GameToggle
                                       ? playerTurn(r_FirstPlayer)
                                       : setRivalTurn();
            if(m_CurrentRound == eRound.EndRound)
            {
                m_GameToggle = playerFoundPair ? m_GameToggle : !m_GameToggle;
                m_CurrentRound = eRound.FirstRound;
                m_CurrentPlayer = m_GameToggle ? r_FirstPlayer : r_SecondPlayer;
            }

            return isGameEnds();
        }

        public string Winner()
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

        public void setNewBoard()
        {
            r_GameBoard.Clear();
            r_GameBoard.intialBoardWithValues();
            r_FirstPlayer.PairsCount = 0;
            r_SecondPlayer.PairsCount = 0;
            m_GameToggle = r_Rnd.Next(2) == 0; // randomly pick true or false, the start player will change
            m_CurrentPlayer = m_GameToggle ? r_FirstPlayer : r_SecondPlayer;
        }

        public void OnComputerPlay()
        {
            OnComputerTurn?.Invoke();
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
            }

            return playerSucceeded;
        }

        private void playerTurnHandler(Player i_CurrPlayer, eRound i_NextRound, out byte o_LineChosen, out byte o_ColomChosen, out byte o_ValueOfCell)
        {
            o_LineChosen = r_Observer.CurrentLineChosen;
            o_ColomChosen = r_Observer.CurrentColomChosen;
            i_CurrPlayer.PlayTurn(r_GameBoard, o_LineChosen, o_ColomChosen);
            o_ValueOfCell = r_GameBoard[o_LineChosen, o_ColomChosen].Content;
            m_CurrentRound = i_NextRound;
        }

        private bool isGameEnds()
        {
            return FirstPlayerPairs + SecondPlayerPairs == (r_GameBoard.Lines * r_GameBoard.Coloms) / 2;
        }
    }
}
