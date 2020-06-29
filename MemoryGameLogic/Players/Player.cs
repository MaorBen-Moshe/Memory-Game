using System;

namespace MemoryGameLogic
{
    public class Player
    {
        public event Action<byte, byte> OnPlayerMove;

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
        
        protected string m_Name;
        protected byte m_PairsCounter;

        public Player(string i_Name)
        {
            Name = i_Name;
        }

        public string Name
        {
            get
            {
                return this.m_Name;
            }

            set
            {
                if (value != null && !value.Equals(m_Name))
                {
                    m_Name = value;
                }
            }
        }

        public byte PairsCount
        {
            get
            {
                return this.m_PairsCounter;
            }

            set
            {
                this.m_PairsCounter = value;
            }
        }

        public void PlayTurn(GameBoard i_GameBoard, byte i_Line, byte i_Colom)
        {
            i_GameBoard[i_Line, i_Colom].IsRevealed = true;
            OnPlayerTurn(i_Line, i_Colom);
        }

        protected virtual void OnPlayerTurn(byte i_Line, byte i_Colom)
        {
            OnPlayerMove?.Invoke(i_Line, i_Colom);
        }
    }
}