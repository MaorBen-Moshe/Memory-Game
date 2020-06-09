using System;

namespace MemoryGame
{

    // $G$ DSN-001 (-5) Game flow implementation does not belong in this class.
    internal class GameControl
    {
        internal enum eTypeOfGame
        {
            AgainstPlayer = 1,
            AgainstComputer = 2
        }

        private enum eRound
        {
            FirstRound,
            SecondRound,
            RoundEnds
        }

        private enum eWhatWinner
        {
            FirstPlayer,
            SecondPlayer,
            Draw
        }

        private int m_CurrStatePairsOnBoard;
        private bool m_IsShutDown;
        private string m_GameStatistics;
        private readonly Utils r_UtilesHandler;
        private readonly InputValidation r_Validator;
        private GameBoard m_GameBoard;
        private Player m_FirstPlayer;
        private Player m_SecondPlayer;
        private ComputerPlayer m_ComputerPlayer;

        public GameControl()
        {
            r_UtilesHandler = new Utils();
            r_Validator = new InputValidation();
        }

        public void GenerateGame()
        {
            bool isValidType;
            Console.WriteLine("Welcome to the Memory Game!");
            m_FirstPlayer = r_UtilesHandler.CreatePlayer();
            do
            {
                Ex02.ConsoleUtils.Screen.Clear();
                Console.WriteLine("Would you like to play against the computer or another player?");
                Console.WriteLine("Press 1 for Another Player Or 2 for Computer.");
                string option = Console.ReadLine();
                isValidType = r_Validator.IsValidTypeOfGame(option, out eTypeOfGame typeOfGame);
                if (isValidType)
                {
                    Ex02.ConsoleUtils.Screen.Clear();
                    controlTypeOfGame(typeOfGame);
                }
                else
                {
                    Console.WriteLine("You press an invalid option. please try again");
                    System.Threading.Thread.Sleep(Utils.k_ErrorMessageSleepSeconds);
                }
            }
            while (!isValidType);
        }

        private void controlTypeOfGame(eTypeOfGame i_TypeOfGame)
        {
            bool restartGame = false;
            do
            {
                r_UtilesHandler.GetBoardSize(out byte lines, out byte coloms);
                Ex02.ConsoleUtils.Screen.Clear();
                m_GameBoard = new GameBoard(lines, coloms);
                r_UtilesHandler.CreateBoardValues(m_GameBoard);

                string rivalName = string.Empty;
                switch (i_TypeOfGame)
                {
                    case eTypeOfGame.AgainstPlayer:
                        m_SecondPlayer = r_UtilesHandler.CreatePlayer();
                        rivalName = m_SecondPlayer.Name;
                        goto default;
                    case eTypeOfGame.AgainstComputer:
                        bool ai = r_UtilesHandler.PickComputerDiffculty();
                        m_ComputerPlayer = new ComputerPlayer(m_GameBoard.Lines, m_GameBoard.Coloms, ai);
                        rivalName = m_ComputerPlayer.Name;
                        goto default;
                    default:

                        gameHandler(rivalName);
                        m_FirstPlayer.PairsCount = 0;
                        if (m_IsShutDown == false)
                        {
                            restartGame = setNewGame();
                        }

                        break;
                }
            }
            while (restartGame && !m_IsShutDown);
        }

        private bool setNewGame()
        {
            Console.WriteLine("Would you like to restart the game or leave? (y/n)");
            string choice = Console.ReadLine();
            bool restartGame = char.TryParse(choice, out char result) &&
                               (result.Equals('y') || result.Equals('Y'));
            m_GameBoard.Clear();
            if (!restartGame)
            {
                Ex02.ConsoleUtils.Screen.Clear();
                Console.WriteLine("GoodBye!");
            }

            return restartGame;
        }


        // $G$ DSN-002 (-20) No separation between the logical part of the game and the UI.
        // $G$ NTT-007 (-10) There's no need to re-instantiate the Random instance each time it is used.
        private void gameHandler(string i_RivalName)
        {
            Random rand = new Random();
            int pairsOnBoard = (m_GameBoard.Lines * m_GameBoard.Coloms) / 2;
            bool gameToggle = rand.Next(2) == 0; // randomly pick true or false, the start player will change
            do
            {
                r_UtilesHandler.CleanScreenAndPrintBoard(m_GameBoard);
                m_GameStatistics = r_UtilesHandler.StatisticsFormat(
                    m_FirstPlayer,
                    i_RivalName,
                    getRivalPairsFound(),
                    gameToggle ? m_FirstPlayer.Name : i_RivalName);
                Console.WriteLine(m_GameStatistics);
                bool playerFoundPair = gameToggle
                                           ? playerTurn(m_FirstPlayer)
                                           : setRivalTurn();
                if (m_IsShutDown)
                {
                    r_UtilesHandler.ShutDown();
                    break;
                }

                m_CurrStatePairsOnBoard = m_FirstPlayer.PairsCount + getRivalPairsFound();
                gameToggle = playerFoundPair ? gameToggle : !gameToggle;
            }
            while (m_CurrStatePairsOnBoard < pairsOnBoard);

            if (m_IsShutDown == false)
            {
                r_UtilesHandler.CleanScreenAndPrintBoard(m_GameBoard);
                setWinner();
            }
        }

        private byte getRivalPairsFound()
        {
            return m_SecondPlayer == null ? m_ComputerPlayer.PairsCount : m_SecondPlayer.PairsCount;
        }

        private bool setRivalTurn()
        {
            return m_SecondPlayer == null
                       ? computerTurn()
                       : playerTurn(m_SecondPlayer);
        }

        private bool computerTurn()
        {
            m_ComputerPlayer.PlayTurn(
                m_GameBoard,
                out byte firstLine,
                out byte firstColom,
                out byte secondLine,
                out byte secondColom);

            byte firstValue = m_GameBoard[firstLine, firstColom].Content;
            byte secondValue = m_GameBoard[secondLine, secondColom].Content;

            r_UtilesHandler.PrintComputerChoice(m_GameBoard, firstLine, firstColom, m_GameStatistics);
            r_UtilesHandler.PrintComputerChoice(m_GameBoard, secondLine, secondColom, m_GameStatistics);

            bool matchFound = turnResult(
            firstLine,
                firstColom,
                secondLine,
                secondColom,
                firstValue,
                secondValue);
            if (matchFound)
            {
                m_ComputerPlayer.PairsCount++;
            }

            return matchFound;
        }

        private bool playerTurn(Player i_CurrPlayer)
        {
            byte firstLine = 0, secondLine = 0, firstColom = 0, secondColom = 0;
            byte firstValue = 0, secondValue = 0;
            bool playerSucceeded = false;

            playerTurnHandler(
                i_CurrPlayer,
                ref firstLine,
                ref firstColom,
                ref secondLine,
                ref secondColom,
                ref firstValue,
                ref secondValue);
            if (m_IsShutDown != true)
            {
                playerSucceeded = turnResult(
                    firstLine,
                    firstColom,
                    secondLine,
                    secondColom,
                    firstValue,
                    secondValue);

                if (playerSucceeded)
                {
                    i_CurrPlayer.PairsCount++;
                }
            }

            return playerSucceeded;
        }

        private void playerTurnHandler(
            Player i_CurrPlayer,
            ref byte io_FirstLine,
            ref byte io_FirstColom,
            ref byte io_SecondLine,
            ref byte io_SecondColom,
            ref byte io_FirstValue,
            ref byte io_SecondValue)
        {
            eRound roundChecker = eRound.FirstRound;
            do
            {
                bool isValid = r_Validator.IsCellEmptyOrExist(m_GameBoard, out byte line, out byte colom, out m_IsShutDown);
                if (m_IsShutDown)
                {
                    break;
                }

                if (isValid != true)
                {
                    r_UtilesHandler.CleanScreenAndPrintBoard(m_GameBoard);
                    continue;
                }

                switch (roundChecker)
                {
                    case eRound.FirstRound:
                        io_FirstLine = line;
                        io_FirstColom = colom;
                        i_CurrPlayer.PlayTurn(m_GameBoard, ref io_FirstLine, ref io_FirstColom, out io_FirstValue);
                        roundChecker = eRound.SecondRound;
                        break;
                    case eRound.SecondRound:
                        io_SecondLine = line;
                        io_SecondColom = colom;
                        i_CurrPlayer.PlayTurn(m_GameBoard, ref io_SecondLine, ref io_SecondColom, out io_SecondValue);
                        roundChecker = eRound.RoundEnds;
                        break;
                }

                r_UtilesHandler.CleanScreenAndPrintBoard(m_GameBoard);
                Console.WriteLine(m_GameStatistics);
            }
            while (roundChecker != eRound.RoundEnds);

            if (m_ComputerPlayer != null)
            {
                m_ComputerPlayer.ComputerLearn(
                    io_FirstLine,
                    io_FirstColom,
                    io_FirstValue,
                    io_SecondLine,
                    io_SecondColom,
                    io_SecondValue);
            }
        }

        private bool turnResult(
            byte i_FirstLine,
            byte i_FirstColom,
            byte i_SecondLine,
            byte i_SecondColom,
            byte i_FirstKey,
            byte i_SecondKey)
        {
            bool playerSucceeded = false;
            bool isExistFirstValue = r_UtilesHandler.FindLetterOfKey(i_FirstKey, out char firstValueLetter);
            bool isExistSecondValueLetter = r_UtilesHandler.FindLetterOfKey(i_SecondKey, out char secondValueLetter);
            if (isExistFirstValue && isExistSecondValueLetter)
            {
                if (firstValueLetter.Equals(secondValueLetter))
                {
                    playerSucceeded = true;
                }
                else
                {
                    m_GameBoard[i_FirstLine, i_FirstColom].IsRevealed = false;
                    m_GameBoard[i_SecondLine, i_SecondColom].IsRevealed = false;
                    System.Threading.Thread.Sleep(Utils.k_BoardSleepSeconds);
                }
            }

            return playerSucceeded;
        }

        private void setWinner()
        {
            eWhatWinner whatTypeOfEndGame = setTypeOfWinner();
            Console.WriteLine("Game Over!");
            switch (whatTypeOfEndGame)
            {
                case eWhatWinner.FirstPlayer:
                    Console.WriteLine(r_UtilesHandler.WinnerFormat(m_FirstPlayer.Name, m_FirstPlayer.PairsCount));
                    break;
                case eWhatWinner.SecondPlayer:
                    Console.WriteLine(
                        m_ComputerPlayer == null
                            ? r_UtilesHandler.WinnerFormat(m_SecondPlayer.Name, m_SecondPlayer.PairsCount)
                            : r_UtilesHandler.WinnerFormat(m_ComputerPlayer.Name, m_ComputerPlayer.PairsCount));
                    break;
                case eWhatWinner.Draw:
                    Console.WriteLine("The game ends in a draw!");
                    break;
            }
        }

        private eWhatWinner setTypeOfWinner()
        {
            eWhatWinner typeOfWinner;
            int winnerCompares = m_FirstPlayer.PairsCount.CompareTo(
                    m_ComputerPlayer != null
                    ? m_ComputerPlayer.PairsCount
                    : m_SecondPlayer.PairsCount);

            if (winnerCompares > 0)
            {
                typeOfWinner = eWhatWinner.FirstPlayer;
            }
            else if (winnerCompares < 0)
            {
                typeOfWinner = eWhatWinner.SecondPlayer;
            }
            else
            {
                typeOfWinner = eWhatWinner.Draw;
            }

            return typeOfWinner;
        }
    }
}