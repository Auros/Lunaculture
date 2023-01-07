namespace Lunaculture.GameTime
{
    public struct DayChangeEvent
    {
        public int Day { get; private set; }

        public DayChangeEvent(int day)
        { 
            Day = day;
        }
    }
}
