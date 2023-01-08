namespace Lunaculture.Plants
{
    public struct PlantStatusEvent
    {
        public PlantGrowthStatus GrowthStatus { get; private set; }

        public PlantStatusEvent(PlantGrowthStatus growthStatus)
        {
            GrowthStatus = growthStatus;
        }
    }
}