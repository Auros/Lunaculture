using System;
using Lunaculture.Grids;
using Lunaculture.Plants;
using Lunaculture.Grids.Objects;
using Lunaculture.Items;
using Lunaculture.Player.Inventory;
using UnityEngine;

namespace Lunaculture.Planting.Seeding
{
    public class SeedPlantingController : MonoBehaviour
    {
        [SerializeField]
        private GridController _gridController = null!;

        [SerializeField]
        private GridObjectController _gridObjectController = null!;
        
        [SerializeField]
        private GridSelectionController _gridSelectionController = null!;
        
        [SerializeField]
        private InventoryService _inventoryService = null!;
        
        [SerializeField]
        private GridPlaceable _hologram = null!;
        
        [SerializeField]
        private string _seedTagName = "Seeds";

        private Item? _tryingToPlant;
        
        private void StartSeedPlantingProcess()
        {
            _gridSelectionController.StartSelection(_hologram, cell =>
            {
                var gridObject = _gridObjectController.GetObjectAt(cell);
                if (gridObject is null || gridObject.Type != GridObjectType.Plot)
                    return false;

                var plotGridObject = (gridObject as PlotGridObject)!;
                return plotGridObject.Empty;
            }, cell =>
            {
                var gridObject = _gridObjectController.GetObjectAt(cell);
                if (gridObject is null || gridObject.Type != GridObjectType.Plot)
                    throw new InvalidOperationException("Could not find plot to place seed in");
                
                var plotGridObject = (gridObject as PlotGridObject)!;
                plotGridObject.PlantedItem = _tryingToPlant;

                var plantPrefab = Instantiate(_tryingToPlant.PlacePrefab);
                _gridController.MoveGameObjectToCellCenter(cell, plantPrefab);
                plotGridObject.Plant = plantPrefab.GetComponent<Plant>();
                
                _inventoryService.RemoveItem(_tryingToPlant!);

                _tryingToPlant = null;
            }, () =>
            {
                _tryingToPlant = null;
            });
        }
        
        private void Update()
        {
            if (_inventoryService.SelectedItem.AsNull() is null)
                return;

            var selectedItem = _inventoryService.SelectedItem!;
            
            // Only allow saplings to be planted into orchards
            if (Array.IndexOf(selectedItem.Tags, _seedTagName) == -1)
                return;

            if (_tryingToPlant || !_gridSelectionController.Active)
                return;

            _tryingToPlant = selectedItem;
            StartSeedPlantingProcess();
        }
    }
}