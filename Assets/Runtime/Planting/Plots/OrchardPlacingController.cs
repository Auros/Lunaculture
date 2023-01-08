using System;
using Lunaculture.Grids;
using Lunaculture.Grids.Objects;
using Lunaculture.Player.Inventory;
using UnityEngine;

namespace Lunaculture.Planting.Plots
{
    public class OrchardPlacingController : MonoBehaviour
    {
        [SerializeField]
        private GridController _gridController = null!;

        [SerializeField]
        private GridObjectController _gridObjectController = null!;
        
        [SerializeField]
        private GridSelectionController _gridSelectionController = null!;
        
        [SerializeField]
        private PhysicalOrchardController _orchardPrefab = null!;

        [SerializeField]
        private InventoryService _inventoryService = null!;
        
        [SerializeField]
        private LayerMask _indoorLayer;

        [SerializeField]
        private string _saplingTagName = "Sapling";

        private PhysicalOrchardController? _currentlyPlacing;
        
        private void StartPlaceNew()
        {
            var orchard = Instantiate(_orchardPrefab);
            orchard.PrepareMovement();
            _gridSelectionController.StartSelection(orchard.Placeable, cell =>
            {
                var center = _gridController.GetCellWorldCenter(cell);
                var rayStart = new Vector3(center.x, _gridController.transform.position.y + 0.5f, center.y);
                var inside = Physics.Raycast(rayStart, Vector3.down, 10, _indoorLayer);
                _gridController.MoveGameObjectToCellCenter(cell, orchard.gameObject);

                GridCell left = new(cell.X, cell.Y + 1);
                GridCell right = new(cell.X + 1, cell.Y);
                GridCell far = new(cell.X + 1, cell.Y + 1);

                // Check if adjacent in 2x2 are clear
                if (_gridObjectController.GetObjectAt(left) is not null ||
                    _gridObjectController.GetObjectAt(right) is not null ||
                    _gridObjectController.GetObjectAt(far) is not null)
                    return false;
                
                return inside && !orchard.OverlapDetector.IsOverlapping();
            }, cell =>
            {
                orchard.PreparePlacement();
                _currentlyPlacing = null;
                _gridObjectController.Register(new OrchardGridObject
                {
                    Cell = cell,
                    Type = GridObjectType.Orchard
                });
                
                GridCell left = new(cell.X, cell.Y + 1);
                GridCell right = new(cell.X + 1, cell.Y);
                GridCell far = new(cell.X + 1, cell.Y + 1);
                
                _gridObjectController.Register(new ChildGridObject
                {
                    Cell = left,
                    Parent = cell,
                    Type = GridObjectType.Child
                });
                
                _gridObjectController.Register(new ChildGridObject
                {
                    Cell = right,
                    Parent = cell,
                    Type = GridObjectType.Child
                });
                
                _gridObjectController.Register(new ChildGridObject
                {
                    Cell = far,
                    Parent = cell,
                    Type = GridObjectType.Child
                });
            }, () =>
            {
                Destroy(orchard);
                _currentlyPlacing = null;
            });
            _currentlyPlacing = orchard;
        }

        private void Update()
        {
            if (_inventoryService.SelectedItem.AsNull() is null)
                return;

            var selectedItem = _inventoryService.SelectedItem!;
            
            // Only allow saplings to be planted into orchards
            if (Array.IndexOf(selectedItem.Tags, _saplingTagName) == -1)
            {
                return;
            }

            if (_currentlyPlacing || !_gridSelectionController.Active)
                return;

            StartPlaceNew();
        }
    }
}