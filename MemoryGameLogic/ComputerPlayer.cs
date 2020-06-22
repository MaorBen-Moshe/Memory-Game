using System;
using System.Collections.Generic;

namespace MemoryGameLogic
{
    public class ComputerPlayer : Player
    {
        public struct Point
        {
            private byte m_Line;
            private byte m_Colom;

            public Point(byte i_Line, byte i_Colom)
            {
                this.m_Colom = i_Colom;
                this.m_Line = i_Line;
            }

            public byte Line
            {
                get
                {
                    return m_Line;
                }
            }

            public byte Colom
            {
                get
                {
                    return m_Colom;
                }
            }
        }

        private readonly List<Point> r_UnknownCellsList;
        private readonly bool r_Ai;
        private readonly Dictionary<byte, Point> r_UnMatchedRevealedCells;
        private readonly Dictionary<Point, Point> r_MatchedRevealedCells;
        private readonly Random r_Rnd = new Random();

        public ComputerPlayer(byte i_Lines, byte i_Coloms, bool i_Ai = false)
        : base("Computer")
        {
            r_UnknownCellsList = new List<Point>(i_Lines * i_Coloms);
            r_UnMatchedRevealedCells = new Dictionary<byte, Point>();
            r_MatchedRevealedCells = new Dictionary<Point, Point>();
            r_Ai = i_Ai;
            initPointList(i_Lines, i_Coloms);
        }

        public void ComputerLearn(
            byte i_FirstLine,
            byte i_FirstColom,
            byte i_FirstValue,
            byte i_SecondLine,
            byte i_SecondColom,
            byte i_SecondValue)
        {
            Point firstPoint = new Point(i_FirstLine, i_FirstColom),
                  secondPoint = new Point(i_SecondLine, i_SecondColom);
            if (i_FirstValue.Equals(i_SecondValue))
            {
                deleteOpponentMatch(firstPoint, secondPoint, i_FirstValue, i_SecondValue);
            }
            else
            {
                if (r_Ai)
                {
                    if (r_UnknownCellsList.Contains(firstPoint))
                    {
                        r_UnknownCellsList.Remove(firstPoint);
                        computerLearnPerPoint(firstPoint, i_FirstValue);
                    }

                    if (r_UnknownCellsList.Contains(secondPoint))
                    {
                        r_UnknownCellsList.Remove(secondPoint);
                        computerLearnPerPoint(secondPoint, i_SecondValue);
                    }
                }
            }
        }

        public void PlayTurn(
           GameBoard i_GameBoard,
           out byte o_FirstLine,
           out byte o_FirstColom,
           out byte o_SecondLine,
           out byte o_SecondColom)
        {
            Point firstPoint, secondPoint;
            if (r_Ai)
            {
                if (r_MatchedRevealedCells.Keys.Count != 0)
                {
                    firstPoint = getNextMatchedDictionaryKey();
                    secondPoint = r_MatchedRevealedCells[firstPoint];
                    r_MatchedRevealedCells.Remove(firstPoint);
                }
                else
                {
                    firstPoint = pickRandomCell();
                    byte firstValue = i_GameBoard[firstPoint.Line, firstPoint.Colom].Content;
                    if (r_UnMatchedRevealedCells.ContainsKey(firstValue))
                    {
                        secondPoint = r_UnMatchedRevealedCells[firstValue];
                        this.r_UnMatchedRevealedCells.Remove(firstValue);
                    }
                    else
                    {
                        secondPoint = pickRandomCell();
                        byte secondValue = i_GameBoard[secondPoint.Line, secondPoint.Colom].Content;
                        if (!firstValue.Equals(secondValue))
                        {
                            computerLearnPerPoint(firstPoint, firstValue);
                            computerLearnPerPoint(secondPoint, secondValue);
                        }
                    }
                }
            }
            else
            {
                firstPoint = pickRandomCell();
                secondPoint = pickRandomCell();
                if (!firstPoint.Equals(secondPoint))
                {
                    r_UnknownCellsList.Add(firstPoint);
                    r_UnknownCellsList.Add(secondPoint);
                }
            }

            o_FirstLine = firstPoint.Line;
            o_FirstColom = firstPoint.Colom;
            o_SecondLine = secondPoint.Line;
            o_SecondColom = secondPoint.Colom;
        }

        private void initPointList(byte i_Lines, byte i_Coloms)
        {
            for (byte i = 0; i < i_Lines; i++)
            {
                for (byte j = 0; j < i_Coloms; j++)
                {
                    this.r_UnknownCellsList.Add(new Point(i, j));
                }
            }
        }

        private void deleteOpponentMatch(Point i_FirstPoint, Point i_SecondPoint, byte i_FirstValue, byte i_SecondValue)
        {
            if (r_UnknownCellsList.Contains(i_FirstPoint))
            {
                r_UnknownCellsList.Remove(i_FirstPoint);
            }

            if (r_UnknownCellsList.Contains(i_SecondPoint))
            {
                r_UnknownCellsList.Remove(i_SecondPoint);
            }

            if (r_UnMatchedRevealedCells.ContainsKey(i_FirstValue))
            {
                r_UnMatchedRevealedCells.Remove(i_FirstValue);
            }

            if (r_UnMatchedRevealedCells.ContainsKey(i_SecondValue))
            {
                r_UnMatchedRevealedCells.Remove(i_SecondValue);
            }

            if (r_MatchedRevealedCells.ContainsKey(i_FirstPoint))
            {
                r_MatchedRevealedCells.Remove(i_FirstPoint);
            }

            if (r_MatchedRevealedCells.ContainsKey(i_SecondPoint))
            {
                r_MatchedRevealedCells.Remove(i_SecondPoint);
            }
        }

        private Point getNextMatchedDictionaryKey()
        {
            Point returnPoint = new Point();

            foreach (Point newPoint in this.r_MatchedRevealedCells.Keys)
            {
                returnPoint = newPoint;
                break;
            }

            return returnPoint;
        }

        private void computerLearnPerPoint(Point i_Point, byte i_Value)
        {
            if (r_UnMatchedRevealedCells.ContainsKey(i_Value))
            {
                r_MatchedRevealedCells.Add(r_UnMatchedRevealedCells[i_Value], i_Point);
                r_UnMatchedRevealedCells.Remove(i_Value);
            }
            else if (r_MatchedRevealedCells.ContainsKey(i_Point))
            {
                r_MatchedRevealedCells.Remove(i_Point);
            }
            else
            {
                r_UnMatchedRevealedCells.Add(i_Value, i_Point);
            }
        }

        private Point pickRandomCell()
        {
            int randomPlace = r_Rnd.Next() % r_UnknownCellsList.Count;
            Point randomPoint = r_UnknownCellsList[randomPlace];
            r_UnknownCellsList.Remove(randomPoint);
            return randomPoint;
        }
    }
}