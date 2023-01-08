using Lunaculture.Machines.Miner;

namespace Lunaculture.Grids.Objects
{
    public class MinerGridObject : GridObject
    {
        public PhysicalMinerController Controller { get; set; } = null!;
    }
}