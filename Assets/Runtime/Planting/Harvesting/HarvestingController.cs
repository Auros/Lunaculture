using System;
using Lunaculture.Grids;
using Lunaculture.Grids.Objects;
using Lunaculture.Items;
using Lunaculture.Plants;
using Lunaculture.Player.Inventory;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Lunaculture.Planting.Harvesting
{
    public class HarvestingController : MonoBehaviour
    {
        [SerializeField]
        private GridObjectController _gridObjectController = null!;
        
        [SerializeField]
        private GridSelectionController _gridSelectionController = null!;
        
        [SerializeField]
        private InventoryService _inventoryService = null!;
        
        [SerializeField]
        private Item _harvestingItem = null!;

        [SerializeField]
        private GridPlaceable _hologram = null!;

        private bool _tryingToHarvest;
        
        private void StartWateringProcess()
        {
            _gridSelectionController.StartSelection(_hologram, cell =>
            {
                var gridObject = _gridObjectController.GetObjectAt(cell);
                if (gridObject is null || gridObject.Type != GridObjectType.Plot)
                    return false;

                var plotGridObject = (gridObject as PlotGridObject)!;
                return plotGridObject.GrowthStatus == PlantGrowthStatus.GrownAndReadyToPermaHarvest;
            }, cell =>
            {
                var gridObject = _gridObjectController.GetObjectAt(cell);
                if (gridObject is null || gridObject.Type != GridObjectType.Plot)
                    throw new InvalidOperationException("Could not find plot to harvest");
                
                var plotGridObject = (gridObject as PlotGridObject)!;

                var plant = plotGridObject.Plant!;

                for (int i = 0; i < plant.Drops.Length; i++)
                {
                    var drop = plant.Drops[i];
                    var dropPercentage = plant.DropPercentages[i];

                    var chance = Random.value;
                    if (dropPercentage < 1 && chance >= dropPercentage)
                        continue;
                    
                    _inventoryService.AddItem(drop);
                }

                plotGridObject.PlantedItem = null;
                plotGridObject.Plant = null;
                Destroy(plant.gameObject);
                _tryingToHarvest = false;
            }, () =>
            {
                _tryingToHarvest = false;
            });
            _tryingToHarvest = true;
        }
        
        private void Update()
        {
            if (_inventoryService.SelectedItem.AsNull() is null)
                return;

            var selectedItem = _inventoryService.SelectedItem!;
            if (selectedItem != _harvestingItem)
                return;

            if (_tryingToHarvest || !_gridSelectionController.Active)
                return;

            StartWateringProcess();
        }
    }
}