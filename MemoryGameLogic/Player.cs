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

        public void PlayTurn(GameBoard i_GameBoard, byte i_Line, byte i_Colom)
        {
            i_GameBoard[i_Line, i_Colom].IsRevealed = true;
        }
    }
}