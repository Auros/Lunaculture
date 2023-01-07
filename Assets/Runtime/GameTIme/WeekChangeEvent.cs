namespace Lunaculture.GameTime
{
    public struct WeekChangeEvent
    {
        public int Week { get; private set; }

        public WeekChangeEvent(int week)
        {
            Week = week;
        }
    }
}
