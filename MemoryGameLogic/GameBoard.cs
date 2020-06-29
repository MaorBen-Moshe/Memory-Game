using System;
using System.Collections.Generic;

namespace MemoryGameLogic
{
    public class GameBoard
    {
        public class Cell
        {
            private byte m_Content;
            private bool m_IsRevealed;

            public byte Content
            {
                get
                {
                    return m_Content;
                }

                set
                {
                    if (value > 0)
                    {
                        m_Content = value;
                    }
                }
            }

            internal bool IsRevealed
            {
                get
                {
                    return m_IsRevealed;
                }

                set
                {
                    m_IsRevealed = value;
                }
            }
        }

        private readonly byte r_Lines;
        private readonly byte r_Coloms;
        private readonly Cell[,] r_Board;
        private readonly Random r_Rand;

        public byte Lines
        {
            get
            {
                return r_Lines;
            }
        }

        public byte Coloms
        {
            get
            {
                return r_Coloms;
            }
        }

        public Cell this[byte i_Line, byte i_Colom]
        {
            get
            {
                return this.r_Board[i_Line, i_Colom];
            }
        }

        public GameBoard(byte i_Lines, byte i_Coloms)
        {
            this.r_Lines = i_Lines;
            this.r_Coloms = i_Coloms;
            this.r_Board = new Cell[i_Lines, i_Coloms];
            this.r_Rand = new Random();
            IntialBoardWithValues();
        }

        internal void Clear()
        {
            foreach (Cell current in r_Board)
            {
                current.IsRevealed = false;
                current.Content = 0;
            }
        }

        internal void IntialBoardWithValues()
        {
            List<Cell> cellsList = new List<Cell>(r_Lines * r_Coloms);
            for (byte i = 0; i < r_Lines; i++)
            {
                for (byte j = 0; j < r_Coloms; j++)
                {
                    r_Board[i, j] = new Cell();
                    cellsList.Add(r_Board[i, j]);
                }
            }

            byte[] boardValues = createPairsOfBoard((this.r_Lines * this.r_Coloms) / 2);
            foreach (byte current in boardValues)
            {
                updateRandomCell(cellsList, current);
                updateRandomCell(cellsList, current);
            }
        }

        private byte[] createPairsOfBoard(int i_PairsOnBoard)
        {
            byte[] inputValues = new byte[i_PairsOnBoard];

            for (byte i = 0; i < inputValues.Length; i++)
            {
                inputValues[i] = (byte)(i + 1);
            }

            return inputValues;
        }

        private void updateRandomCell(List<Cell> i_CellsList, byte i_ToInsert)
        {
            int randomPlace = r_Rand.Next() % i_CellsList.Count;
            i_CellsList[randomPlace].Content = i_ToInsert;
            i_CellsList.Remove(i_CellsList[randomPlace]);
        }
    }
}