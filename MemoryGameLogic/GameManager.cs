﻿using System;

namespace MemoryGameLogic
{
    public delegate void OnCurrentCardChosen(out byte i_CurrentLine, out byte i_CurrentColom);

    public delegate void OnAgainstComputer(out bool i_Ai);

    public class GameManager
    {
        public event OnCurrentCardChosen OnPlayerChooseCard;

        public event Action OnGameEnd;

        public event OnAgainstComputer OnAgainstComputer;

        public event Action OnPlayerTurnEnd;

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
        private eRound m_CurrentRound;

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

        public GameBoard.Cell this[int i_Line, int i_Colom]
        {
            get
            {
                return r_GameBoard[(byte)i_Line, (byte)i_Colom];
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

        public string CurrentPlayerName
        {
            get
            {
                return m_CurrentPlayer.Name;
            }
        }

        public string FirstPlayerName
        {
            get
            {
                return r_FirstPlayer.Name;
            }
        }

        public byte FirstPlayerPairsCount
        {
            get
            {
                return r_FirstPlayer.PairsCount;
            }
        }

        public string SecondPlayerName
        {
            get
            {
                return r_SecondPlayer.Name;
            }
        }

        public byte SecondPlayerPairsCount
        {
            get
            {
                return r_SecondPlayer.PairsCount;
            }
        }

        public bool IsCurrentComputer
        {
            get
            {
                return m_CurrentPlayer is ComputerPlayer;
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

        public void StartGame()
        {
            m_CurrentRound = eRound.FirstRound;
            if (r_SecondPlayer is ComputerPlayer)
            {
                OnGameAgainstComputer();
            }
        }

        public void RunGame()
        {
            bool playerFoundPair = m_GameToggle 
                                       ? playerTurn() 
                                       : setRivalTurn();
            if (m_CurrentRound == eRound.EndRound)
            {
                m_GameToggle = playerFoundPair ? m_GameToggle : !m_GameToggle; 
                m_CurrentPlayer = m_GameToggle ? r_FirstPlayer : r_SecondPlayer; 
                m_CurrentRound = eRound.FirstRound;
                OnTurnEnd();
            }

            if(IsGameEnds)
            {
                OnGameEnds();
            }
        }

        public void SetNewGameValues()
        {
            r_GameBoard.Clear();
            r_GameBoard.IntialBoardWithValues();
            r_FirstPlayer.PairsCount = 0;
            r_SecondPlayer.PairsCount = 0;
            if(r_SecondPlayer is ComputerPlayer)
            {
                bool ai = ((ComputerPlayer)r_SecondPlayer).Ai;
                ((ComputerPlayer)r_SecondPlayer).SetNewGameValues((byte)BoardLines, (byte)BoardColoms, ai);
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
                       : playerTurn();
        }

        private bool computerTurn()
        {
            ((ComputerPlayer)r_SecondPlayer).PlayTurn(
                r_GameBoard,
                out m_FirstLineChosen,
                out m_FirstColomChosen,
                out m_SecondLineChosen,
                out m_SecondColomChosen);

            m_CurrentRound = eRound.EndRound;
            return isPlayerFoundPair();
        }

        private bool playerTurn()
        {
            bool playerSucceeded = false;

            switch (m_CurrentRound)
            {
                case eRound.FirstRound:
                    playerTurnHandler(eRound.SecondRound, out m_FirstLineChosen, out m_FirstColomChosen, out m_FirstValueFound);
                    break;
                case eRound.SecondRound:
                    playerTurnHandler(eRound.EndRound, out m_SecondLineChosen, out m_SecondColomChosen, out m_SecondValueFound);
                    goto case eRound.EndRound; 
                case eRound.EndRound:
                    playerSucceeded = playerEndTurnHandler();
                    break;
            }

            return playerSucceeded;
        }

        private void playerTurnHandler(
                                       eRound i_NextRound,
                                       out byte o_LineChosen,
                                       out byte o_ColomChosen,
                                       out byte o_ValueOfCell)
        {
            OnCurrentCardChosen(out o_LineChosen, out o_ColomChosen);
            m_CurrentPlayer.PlayTurn(r_GameBoard, o_LineChosen, o_ColomChosen);
            o_ValueOfCell = r_GameBoard[o_LineChosen, o_ColomChosen].Content;
            m_CurrentRound = i_NextRound;
        }

        private bool playerEndTurnHandler()
        {
            (r_SecondPlayer as ComputerPlayer)?.ComputerLearn(
                m_FirstLineChosen,
                m_FirstColomChosen,
                m_FirstValueFound,
                m_SecondLineChosen,
                m_SecondColomChosen,
                m_SecondValueFound);

            return isPlayerFoundPair();
        }

        private bool isPlayerFoundPair()
        {
            bool playerSucceeded = false;
            if (r_GameBoard[m_FirstLineChosen, m_FirstColomChosen].Content == r_GameBoard[m_SecondLineChosen, m_SecondColomChosen].Content)
            {
                m_CurrentPlayer.PairsCount++;
                playerSucceeded = true;
            }
            else
            {
                r_GameBoard[m_FirstLineChosen, m_FirstColomChosen].IsRevealed = false;
                r_GameBoard[m_SecondLineChosen, m_SecondColomChosen].IsRevealed = false;
            }

            return playerSucceeded;
        }

        private void setRandomlyCurrentPlayer()
        {
            if(r_SecondPlayer is ComputerPlayer)
            {
                m_GameToggle = true; // if we play against the computer the human player starts the game
            }
            else
            {
                m_GameToggle = r_Rnd.Next(2) == 0; // randomly pick true or false, the start player will change
            }

            m_CurrentPlayer = m_GameToggle ? r_FirstPlayer : r_SecondPlayer;
        }

        protected virtual void OnCurrentCardChosen(out byte o_CurrentLine, out byte o_CurrentColom)
        {
            o_CurrentLine = o_CurrentColom = default;
            OnPlayerChooseCard?.Invoke(out o_CurrentLine, out o_CurrentColom);
            if(o_CurrentLine >= BoardLines || o_CurrentColom >= BoardColoms)
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        protected virtual void OnGameAgainstComputer()
        {
            bool ai = false;
            OnAgainstComputer?.Invoke(out ai);
            ((ComputerPlayer)r_SecondPlayer).Ai = ai;
        }

        protected virtual void OnGameEnds()
        {
            OnGameEnd?.Invoke();
        }

        protected virtual void OnTurnEnd()
        {
            OnPlayerTurnEnd?.Invoke();
        }
    }
}
