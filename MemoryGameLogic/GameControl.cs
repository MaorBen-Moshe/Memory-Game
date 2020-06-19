//using System;

//namespace MemoryGameLogic
//{
//    public class GameControl
//    {
//        private enum eRound
//        {
//            FirstRound,
//            SecondRound,
//            RoundEnds
//        }

//        private int m_CurrStatePairsOnBoard;
//        private GameBoard m_GameBoard;
//        private Player m_FirstPlayer;
//        private Player m_SecondPlayer;
//        private ComputerPlayer m_ComputerPlayer;


//        private void gameHandler()
//        {
//            bool gameToggle = r_Rnd.Next(2) == 0; // randomly pick true or false, the start player will change
//            do
//            {
//                bool playerFoundPair = gameToggle
//                                           ? playerTurn(m_FirstPlayer)
//                                           : setRivalTurn();

//                m_CurrStatePairsOnBoard = m_FirstPlayer.PairsCount + getRivalPairsFound();
//                gameToggle = playerFoundPair ? gameToggle : !gameToggle;
//            }
//            while (m_CurrStatePairsOnBoard < m_PairsOnBoard);
//        }

//        private byte getRivalPairsFound()
//        {
//            return m_SecondPlayer == null ? m_ComputerPlayer.PairsCount : m_SecondPlayer.PairsCount;
//        }

//        private bool setRivalTurn()
//        {
//            return m_SecondPlayer == null
//                       ? computerTurn()
//                       : playerTurn(m_SecondPlayer);
//        }

//        private bool computerTurn()
//        {
//            m_ComputerPlayer.PlayTurn(
//                m_GameBoard,
//                out byte firstLine,
//                out byte firstColom,
//                out byte secondLine,
//                out byte secondColom);

//            byte firstValue = m_GameBoard[firstLine, firstColom].Content;
//            byte secondValue = m_GameBoard[secondLine, secondColom].Content;

//            r_UtilesHandler.PrintComputerChoice(m_GameBoard, firstLine, firstColom, m_GameStatistics);
//            r_UtilesHandler.PrintComputerChoice(m_GameBoard, secondLine, secondColom, m_GameStatistics);

//            bool matchFound = turnResult(
//            firstLine,
//                firstColom,
//                secondLine,
//                secondColom,
//                firstValue,
//                secondValue);
//            if (matchFound)
//            {
//                m_ComputerPlayer.PairsCount++;
//            }

//            return matchFound;
//        }

//        private bool playerTurn(Player i_CurrPlayer)
//        {
//            byte firstLine = 0, secondLine = 0, firstColom = 0, secondColom = 0;
//            byte firstValue = 0, secondValue = 0;
//            bool playerSucceeded = false;

//            playerTurnHandler(
//                i_CurrPlayer,
//                ref firstLine,
//                ref firstColom,
//                ref secondLine,
//                ref secondColom,
//                ref firstValue,
//                ref secondValue);
//            if (m_IsShutDown != true)
//            {
//                playerSucceeded = turnResult(
//                    firstLine,
//                    firstColom,
//                    secondLine,
//                    secondColom,
//                    firstValue,
//                    secondValue);

//                if (playerSucceeded)
//                {
//                    i_CurrPlayer.PairsCount++;
//                }
//            }

//            return playerSucceeded;
//        }

//        private void playerTurnHandler(
//            Player i_CurrPlayer,
//            ref byte io_FirstLine,
//            ref byte io_FirstColom,
//            ref byte io_SecondLine,
//            ref byte io_SecondColom,
//            ref byte io_FirstValue,
//            ref byte io_SecondValue)
//        {
//            eRound roundChecker = eRound.FirstRound;
//            do
//            {
//                bool isValid = r_Validator.IsCellEmptyOrExist(m_GameBoard, out byte line, out byte colom, out m_IsShutDown);
//                if (m_IsShutDown)
//                {
//                    break;
//                }

//                if (isValid != true)
//                {
//                    r_UtilesHandler.CleanScreenAndPrintBoard(m_GameBoard);
//                    continue;
//                }

//                switch (roundChecker)
//                {
//                    case eRound.FirstRound:
//                        io_FirstLine = line;
//                        io_FirstColom = colom;
//                        i_CurrPlayer.PlayTurn(m_GameBoard, ref io_FirstLine, ref io_FirstColom, out io_FirstValue);
//                        roundChecker = eRound.SecondRound;
//                        break;
//                    case eRound.SecondRound:
//                        io_SecondLine = line;
//                        io_SecondColom = colom;
//                        i_CurrPlayer.PlayTurn(m_GameBoard, ref io_SecondLine, ref io_SecondColom, out io_SecondValue);
//                        roundChecker = eRound.RoundEnds;
//                        break;
//                }

//                r_UtilesHandler.CleanScreenAndPrintBoard(m_GameBoard);
//                Console.WriteLine(m_GameStatistics);
//            }
//            while (roundChecker != eRound.RoundEnds);

//            if (m_ComputerPlayer != null)
//            {
//                m_ComputerPlayer.ComputerLearn(
//                    io_FirstLine,
//                    io_FirstColom,
//                    io_FirstValue,
//                    io_SecondLine,
//                    io_SecondColom,
//                    io_SecondValue);
//            }
//        }

//        private bool turnResult(
//            byte i_FirstLine,
//            byte i_FirstColom,
//            byte i_SecondLine,
//            byte i_SecondColom,
//            byte i_FirstKey,
//            byte i_SecondKey)
//        {
//            bool playerSucceeded = false;
//            bool isExistFirstValue = r_UtilesHandler.FindLetterOfKey(i_FirstKey, out char firstValueLetter);
//            bool isExistSecondValueLetter = r_UtilesHandler.FindLetterOfKey(i_SecondKey, out char secondValueLetter);
//            if (isExistFirstValue && isExistSecondValueLetter)
//            {
//                if (firstValueLetter.Equals(secondValueLetter))
//                {
//                    playerSucceeded = true;
//                }
//                else
//                {
//                    m_GameBoard[i_FirstLine, i_FirstColom].IsRevealed = false;
//                    m_GameBoard[i_SecondLine, i_SecondColom].IsRevealed = false;
//                    System.Threading.Thread.Sleep(Utils.k_BoardSleepSeconds);
//                }
//            }

//            return playerSucceeded;
//        }

//        internal bool FindLetterOfKey(byte i_Key, out char o_Value)
//        {
//            bool isValueExist = r_GameValues.TryGetValue(i_Key, out o_Value);
//            return isValueExist;
//        }
//    }
//}