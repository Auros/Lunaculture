using Lunaculture.Grids;
using Lunaculture.Items;
using Lunaculture.Player.Inventory;
using UnityEngine;

namespace Lunaculture.Buildings
{
    public class BuildingPlacingController : MonoBehaviour
    {
        [SerializeField]
        private GridController _gridController = null!;

        [SerializeField]
        private GridObjectController _gridObjectController = null!;
        
        [SerializeField]
        private GridSelectionController _gridSelectionController = null!;
        
        [SerializeField]
        private PhysicalBuildingController _buildingPrefab = null!;

        [SerializeField]
        private InventoryService _inventoryService = null!;
        
        [SerializeField]
        private Item _buildingItem = null!;

        [SerializeField]
        private Item _resourceItem = null!;
        
        [SerializeField]
        private int _resourceCostPerBuilding = 100;

        private PhysicalBuildingController? _currentlyPlacing;
        
        private void StartPlaceNew()
        {
            var building = Instantiate(_buildingPrefab);
            building.PrepareMovement();
            _gridSelectionController.StartSelection(building.Placeable, cell =>
            {
                if (!HasEnoughResources())
                    return false;
                
                _gridController.MoveGameObjectToCellCenter(cell, building.gameObject);
                return !building.OverlapDetector.IsOverlapping();
            }, cell =>
            {
                building.PreparePlacement();
                _currentlyPlacing = null;
                /*_gridObjectController.Register(new BuildingGridObject
                {
                    Cell = cell,
                    Type = GridObjectType.Building
                });*/
                
                for (int i = 0; i < _resourceCostPerBuilding; i++)
                    _inventoryService.RemoveItem(_resourceItem);
                
            }, () =>
            {
                if (building)
                    Destroy(building.gameObject);
                _currentlyPlacing = null;
            });
            _currentlyPlacing = building;
        }

        private void Update()
        {
            if (_inventoryService.SelectedItem.AsNull() is null)
                return;

            var selectedItem = _inventoryService.SelectedItem!;
            if (selectedItem != _buildingItem)
            {
                if (!_currentlyPlacing)
                    return;
                
                Destroy(_currentlyPlacing!.gameObject);
                _currentlyPlacing = null;
                return;
            }

            if (_currentlyPlacing || !_gridSelectionController.Active)
                return;

            StartPlaceNew();
        }

        private bool HasEnoughResources()
        {
            var inventory = _inventoryService.Inventory;
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var stack in inventory)
            {
                if (stack is null || stack.ItemType != _resourceItem)
                    continue;

                return stack.Count >= _resourceCostPerBuilding;
            }
            return false;
        }
    }
}