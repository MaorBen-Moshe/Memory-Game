namespace MemoryGameLogic
{
    public class Player
    {
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

        public void PlayTurn(GameBoard i_GameBoard, ref byte io_Line, ref byte io_Colom, out byte o_Value)
        {
            o_Value = i_GameBoard[io_Line, io_Colom].Content;
            i_GameBoard[io_Line, io_Colom].IsRevealed = true;
        }
    }
}