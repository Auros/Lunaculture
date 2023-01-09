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
                
                if (gridObject is null) return false;
                
                // handle plot
                if (gridObject.Type == GridObjectType.Plot)
                {
                    var plotGridObject = (gridObject as PlotGridObject)!;
                    return plotGridObject.GrowthStatus is PlantGrowthStatus.GrownAndReadyToPermaHarvest or PlantGrowthStatus.GrownAndReadyToNonPermaHarvest;
                }
                
                // handle trees
                if (gridObject.Type == GridObjectType.Orchard)
                {
                    var orchardGridObject = (gridObject as OrchardGridObject)!;
                    return orchardGridObject.GrowthStatus is PlantGrowthStatus.GrownAndReadyToPermaHarvest or PlantGrowthStatus.GrownAndReadyToNonPermaHarvest;
                }
                if (gridObject.Type == GridObjectType.Child)
                {
                    var childGridObject = (gridObject as ChildGridObject)!;
                    var parentGridObject = _gridObjectController.GetObjectAt(childGridObject.Parent);

                    if (parentGridObject != null && parentGridObject.Type == GridObjectType.Orchard)
                    {
                        var orchardGridObject = (parentGridObject as OrchardGridObject)!;

                        return orchardGridObject.GrowthStatus is PlantGrowthStatus.GrownAndReadyToPermaHarvest or PlantGrowthStatus.GrownAndReadyToNonPermaHarvest;
                    }
                }
                return false;
            }, cell =>
            {
                var gridObject = _gridObjectController.GetObjectAt(cell);
                if (gridObject is null) throw new InvalidOperationException("Could not find plot to water");
                
                Plant? plant = null;
                
                // handle plot
                if (gridObject.Type == GridObjectType.Plot)
                {
                    var plotGridObject = (gridObject as PlotGridObject)!;
                    plant = plotGridObject.Plant;
                }
                
                // handle trees
                if (gridObject.Type == GridObjectType.Orchard)
                {
                    var orchardGridObject = (gridObject as OrchardGridObject)!;
                    plant = orchardGridObject.Plant;
                }
                if (gridObject.Type == GridObjectType.Child)
                {
                    var childGridObject = (gridObject as ChildGridObject)!;
                    var parentGridObject = _gridObjectController.GetObjectAt(childGridObject.Parent);

                    if (parentGridObject.Type == GridObjectType.Orchard)
                    {
                        var orchardGridObject = (parentGridObject as OrchardGridObject)!;
                        plant = orchardGridObject.Plant;
                    }
                }

                if (plant is null) throw new InvalidOperationException("Could not find plot to water");
                
                for (int i = 0; i < plant.Drops.Length; i++)
                {
                    var drop = plant.Drops[i];
                    var dropPercentage = plant.DropPercentages[i];

                    var chance = Random.value;
                    if (dropPercentage < 1 && chance >= dropPercentage)
                        continue;
                    
                    _inventoryService.AddItem(drop);
                }

                if (plant.GrowthStatus == PlantGrowthStatus.GrownAndReadyToPermaHarvest)
                {
                    // too much code copy pasting but we just need to meet the deadline lmao
                    if (gridObject.Type == GridObjectType.Plot)
                    {
                        var plotGridObject = (gridObject as PlotGridObject)!;
                        plotGridObject.PlantedItem = null;
                        plotGridObject.Plant = null;
                        Destroy(plant.gameObject);
                    }
                
                    // handle trees
                    if (gridObject.Type == GridObjectType.Orchard)
                    {
                        var orchardGridObject = (gridObject as OrchardGridObject)!;
                        orchardGridObject.PlantedItem = null;
                        orchardGridObject.Plant = null;
                        Destroy(plant.gameObject);
                    }
                    if (gridObject.Type == GridObjectType.Child)
                    {
                        var childGridObject = (gridObject as ChildGridObject)!;
                        var parentGridObject = _gridObjectController.GetObjectAt(childGridObject.Parent);

                        if (parentGridObject.Type == GridObjectType.Orchard)
                        {
                            var orchardGridObject = (parentGridObject as OrchardGridObject)!;
                            orchardGridObject.PlantedItem = null;
                            orchardGridObject.Plant = null;
                            Destroy(plant.gameObject);
                        }
                    }
                }
                else
                {
                    // send harvest data to plant
                    plant.NonPermaHarvest();
                }
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