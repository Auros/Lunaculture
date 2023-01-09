using System;
using Lunaculture.Grids;
using Lunaculture.Grids.Objects;
using Lunaculture.Items;
using Lunaculture.Plants;
using Lunaculture.Player.Inventory;
using UnityEngine;

namespace Lunaculture
{
    public class WateringController : MonoBehaviour
    {
        [SerializeField]
        private GridObjectController _gridObjectController = null!;
        
        [SerializeField]
        private GridSelectionController _gridSelectionController = null!;
        
        [SerializeField]
        private InventoryService _inventoryService = null!;
        
        [SerializeField]
        private Item _wateringItem = null!;

        [SerializeField]
        private GridPlaceable _hologram = null!;

        private bool _tryingToWater;
        
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
                    return plotGridObject.GrowthStatus is PlantGrowthStatus.NotWatered or PlantGrowthStatus.GrownButNotWatered;
                }
                
                // handle trees
                if (gridObject.Type == GridObjectType.Orchard)
                {
                    var orchardGridObject = (gridObject as OrchardGridObject)!;
                    return orchardGridObject.GrowthStatus is PlantGrowthStatus.NotWatered or PlantGrowthStatus.GrownButNotWatered;
                }
                if (gridObject.Type == GridObjectType.Child)
                {
                    var childGridObject = (gridObject as ChildGridObject)!;
                    var parentGridObject = _gridObjectController.GetObjectAt(childGridObject.Parent);

                    if (parentGridObject != null && parentGridObject.Type == GridObjectType.Orchard)
                    {
                        var orchardGridObject = (parentGridObject as OrchardGridObject)!;

                        return orchardGridObject.GrowthStatus is PlantGrowthStatus.NotWatered or PlantGrowthStatus.GrownButNotWatered;
                    }
                }
                return false;
            }, cell =>
            {
                var gridObject = _gridObjectController.GetObjectAt(cell);
                
                if (gridObject is null) throw new InvalidOperationException("Could not find plot to water");
                
                // handle plot
                if (gridObject.Type == GridObjectType.Plot)
                {
                    var plotGridObject = (gridObject as PlotGridObject)!;
                    plotGridObject.Plant!.Water();
                }
                
                // handle trees
                if (gridObject.Type == GridObjectType.Orchard)
                {
                    var orchardGridObject = (gridObject as OrchardGridObject)!;
                    orchardGridObject.Plant!.Water();
                }
                if (gridObject.Type == GridObjectType.Child)
                {
                    var childGridObject = (gridObject as ChildGridObject)!;
                    var parentGridObject = _gridObjectController.GetObjectAt(childGridObject.Parent);

                    if (parentGridObject.Type == GridObjectType.Orchard)
                    {
                        var orchardGridObject = (parentGridObject as OrchardGridObject)!;
                        orchardGridObject.Plant!.Water();
                    }
                }
                
                _tryingToWater = false;
            }, () =>
            {
                _tryingToWater = false;
            });
            _tryingToWater = true;
        }
        
        private void Update()
        {
            if (_inventoryService.SelectedItem.AsNull() is null)
                return;

            var selectedItem = _inventoryService.SelectedItem!;
            if (selectedItem != _wateringItem)
                return;

            if (_tryingToWater || !_gridSelectionController.Active)
                return;

            StartWateringProcess();
        }
    }
}
