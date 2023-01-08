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
                if (gridObject is null || gridObject.Type != GridObjectType.Plot)
                    return false;

                var plotGridObject = (gridObject as PlotGridObject)!;
                return plotGridObject.GrowthStatus == PlantGrowthStatus.NotWatered || plotGridObject.GrowthStatus == PlantGrowthStatus.GrownButNotWatered;
            }, cell =>
            {
                var gridObject = _gridObjectController.GetObjectAt(cell);
                if (gridObject is null || gridObject.Type != GridObjectType.Plot)
                    throw new InvalidOperationException("Could not find plot to water");
                
                var plotGridObject = (gridObject as PlotGridObject)!;
                plotGridObject.Plant.Water();
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
