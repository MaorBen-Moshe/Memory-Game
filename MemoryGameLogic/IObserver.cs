namespace MemoryGameLogic
{
    public interface IObserver
    {
        byte CurrentLineChosen
        {
            get;
        }

        byte CurrentColomChosen
        {
            get;
        }
    }
}