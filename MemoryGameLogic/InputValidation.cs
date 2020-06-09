using System;

namespace MemoryGame
{
    // $G$ DSN-002 (-10) You should not make UI calls from your logic classes. 
    internal class InputValidation
    {
        private const char k_ShutDownGame = 'Q';

        internal bool IsBoardSizeCorrect(
            string i_LinesToCheck,
            string i_ColomsToCheck,
            out byte o_Line,
            out byte o_Colom)
        {
            bool isValid = false;
            bool isLine = byte.TryParse(i_LinesToCheck, out o_Line);
            bool isColom = byte.TryParse(i_ColomsToCheck, out o_Colom);
            if (isLine && isColom)
            {
                if (o_Line < 4 || o_Line > 6 || o_Colom < 4 || o_Colom > 6)
                {
                    Console.WriteLine("The number you entered is off the limit of the board rules!");
                    isValid = false;
                }
                else if (((o_Line * o_Colom) % 2) != 0)
                {
                    Console.WriteLine("The product result is odd, only an even result accepted!");
                    isValid = false;
                }
                else
                {
                    isValid = true;
                }
            }
            else
            {
                Console.WriteLine("You didn't enter a valid digit!");
            }

            return isValid;
        }

        internal bool IsValidTypeOfGame(string i_Option, out GameControl.eTypeOfGame i_TypeOfGame)
        {
            i_TypeOfGame = GameControl.eTypeOfGame.AgainstPlayer;
            bool isValid = int.TryParse(i_Option, out int type);
            if (isValid)
            {
                if (type.Equals(1) || type.Equals(2))
                {
                    i_TypeOfGame = (GameControl.eTypeOfGame)type;
                }
                else
                {
                    isValid = false;
                }
            }
            else
            {
                isValid = false;
            }

            return isValid;
        }

        internal bool IsCellEmptyOrExist(GameBoard i_GameBoard, out byte o_Line, out byte o_Colom, out bool o_IsLeaveGame)
        {
            Console.WriteLine("Please enter the cell you would like to reveal: (Format Example: A3) ");
            string cell = Console.ReadLine();
            bool isValid = false;
            o_Line = 0;
            o_Colom = 0;
            o_IsLeaveGame = isShutGameDown(cell);
            if (o_IsLeaveGame != true)
            {
                if (cell != null && cell.Length == 2)
                {
                    bool isLine = byte.TryParse(cell[1].ToString(), out o_Line);
                    bool isColom = char.IsLetter(cell[0]);
                    o_Colom = (byte)(char.IsUpper(cell[0]) ? cell[0] - 'A' : cell[0] - 'a');
                    o_Line--;
                    if (isLine && isColom)
                    {
                        if (o_Line < i_GameBoard.Lines && o_Colom < i_GameBoard.Coloms)
                        {
                            isValid = true;
                            if (i_GameBoard[o_Line, o_Colom].IsRevealed)
                            {
                                isValid = false;
                                bordersErrorMessage(true);
                            }
                        }
                        else
                        {
                            bordersErrorMessage();
                        }
                    }
                    else
                    {
                        bordersErrorMessage();
                    }
                }
                else
                {
                    bordersErrorMessage();
                }
            }

            return isValid;
        }

        private bool isShutGameDown(string i_IsShutDown)
        {
            bool isShutDown = char.TryParse(i_IsShutDown, out char result);
            if (isShutDown)
            {
                if (!result.Equals(k_ShutDownGame))
                {
                    isShutDown = false;
                }
            }

            return isShutDown;
        }

        internal bool CheckComputerDiffculty(string i_Option, out bool o_GameType)
        {
            o_GameType = false;
            bool isValid = int.TryParse(i_Option, out int type);
            if (isValid)
            {
                if (type.Equals(1) || type.Equals(2))
                {
                    if (type.Equals(2))
                    {
                        o_GameType = true;
                    }
                }
                else
                {
                    isValid = false;
                    Console.WriteLine("Invalid Input");
                    System.Threading.Thread.Sleep(Utils.k_ErrorMessageSleepSeconds);
                }
            }
            else
            {
                isValid = false;
                Console.WriteLine("Invalid Input");
                System.Threading.Thread.Sleep(Utils.k_ErrorMessageSleepSeconds);
            }

            return isValid;
        }

        private void bordersErrorMessage(bool i_IsTakenOrInValid = false)
        {
            Console.WriteLine(
                i_IsTakenOrInValid
                    ? "Cell is already taken. please try another one!"
                    : "Please Enter a valid place on the board");
            System.Threading.Thread.Sleep(Utils.k_ErrorMessageSleepSeconds);
        }
    }
}