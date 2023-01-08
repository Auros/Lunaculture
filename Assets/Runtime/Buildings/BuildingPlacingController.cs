using Lunaculture.Grids;
using Lunaculture.Grids.Objects;
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

        private PhysicalBuildingController? _currentlyPlacing;
        
        private void StartPlaceNew()
        {
            var building = Instantiate(_buildingPrefab);
            building.PrepareMovement();
            _gridSelectionController.StartSelection(building.Placeable, cell =>
            {
                _gridController.MoveGameObjectToCellCenter(cell, building.gameObject);
                return !building.OverlapDetector.IsOverlapping();
            }, cell =>
            {
                building.PreparePlacement();
                _currentlyPlacing = null;
                _gridObjectController.Register(new BuildingGridObject
                {
                    Cell = cell,
                    Type = GridObjectType.Building
                });
            }, () =>
            {
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
                return;

            if (_currentlyPlacing || !_gridSelectionController.Active)
                return;

            StartPlaceNew();
        }
    }
}