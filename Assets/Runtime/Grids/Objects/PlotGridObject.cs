using Lunaculture.Items;
using Lunaculture.Plants;

namespace Lunaculture.Grids.Objects
{
    public class PlotGridObject : GridObject
    {
        public bool Empty { get { return Plant == null; } }

        public PlantGrowthStatus GrowthStatus { get { return Plant?.GrowthStatus ?? PlantGrowthStatus.Empty; } }
        
        public Item? PlantedItem { get; set; }
        public Plant? Plant { get; set; }
        
        //public bool Watered { get; set; }
        
        //public Item? Planted { get; set; }
    }
}