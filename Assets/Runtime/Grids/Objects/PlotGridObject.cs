using Lunaculture.Items;

namespace Lunaculture.Grids.Objects
{
    public class PlotGridObject : GridObject
    {
        public bool Empty { get; set; }
        
        public bool Watered { get; set; }
        
        public Item? Planted { get; set; }
    }
}