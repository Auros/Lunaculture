using Lunaculture.Items;
using Lunaculture.Plants;

namespace Lunaculture.Grids.Objects
{
    public class OrchardGridObject : GridObject
    {
        public bool Empty => !Plant;

        public PlantGrowthStatus GrowthStatus => Plant.AsNull()?.GrowthStatus ?? PlantGrowthStatus.Empty;

        public Item? PlantedItem { get; set; }
        public Plant? Plant { get; set; }
    }
}