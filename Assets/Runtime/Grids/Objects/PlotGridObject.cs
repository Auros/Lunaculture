using Lunaculture.Items;
using Lunaculture.Planting.Plots;
using Lunaculture.Plants;

namespace Lunaculture.Grids.Objects
{
    public class PlotGridObject : GridObject
    {
        public bool Empty => !Plant;

        public PlantGrowthStatus GrowthStatus => Plant.AsNull()?.GrowthStatus ?? PlantGrowthStatus.Empty;

        public PhysicalPlotController Controller { get; set; } = null!;
        public Item? PlantedItem { get; set; }
        public Plant? Plant { get; set; }
        
        //public bool Watered { get; set; }
        
        //public Item? Planted { get; set; }
    }
}