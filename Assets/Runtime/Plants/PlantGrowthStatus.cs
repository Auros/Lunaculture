namespace Lunaculture.Plants
{
    public enum PlantGrowthStatus
    {
        Empty, // only used in PlotGridObject to denote a plant not existing
        NotWatered,
        Growing,
        GrownAndReadyToPermaHarvest, // normal plants, will destroy plant
        GrownAndReadyToNonPermaHarvest, // trees, will destroy fruit only
        GrownButNotWatered, // trees that need to be watered again
        GrownButGrowingAgain, // trees that are growing fruit again
    }
}