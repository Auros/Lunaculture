namespace Lunaculture.Grids.Objects
{
    public abstract class GridObject
    {
        public GridCell Cell { get; set; }
        
        public GridObjectType Type { get; set; }
    }
}