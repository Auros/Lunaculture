using System;
using Lunaculture.Grids;
using Lunaculture.Items;
using Lunaculture.Grids.Objects;
using Lunaculture.Plants;
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
        
        private Item? _tryingToPlant;

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

                GridCell self = new(cell.X, cell.Y);
                GridCell left = new(cell.X, cell.Y + 1);
                GridCell right = new(cell.X + 1, cell.Y);
                GridCell far = new(cell.X + 1, cell.Y + 1);

                // Check if adjacent in 2x2 are clear
                if (_gridObjectController.GetObjectAt(left) is not null ||
                    _gridObjectController.GetObjectAt(right) is not null ||
                    _gridObjectController.GetObjectAt(far) is not null ||
                    _gridObjectController.GetObjectAt(self) is not null)
                    return false;
                
                return inside && !orchard.OverlapDetector.IsOverlapping();
            }, cell =>
            {
                GridCell left = new(cell.X, cell.Y + 1);
                GridCell right = new(cell.X + 1, cell.Y);
                GridCell far = new(cell.X + 1, cell.Y + 1);
                GridCell self = new(cell.X, cell.Y);
                // Check if adjacent in 2x2 are clear
                if ( _gridObjectController.GetObjectAt(left) is not null ||
                    _gridObjectController.GetObjectAt(right) is not null ||
                    _gridObjectController.GetObjectAt(far) is not null ||
                    _gridObjectController.GetObjectAt(self) is not null)
                {
                    if (orchard)
                        Destroy(orchard.gameObject);
                    _currentlyPlacing = null;
                    return;
                }
                
                orchard.PreparePlacement();
                _currentlyPlacing = null;
                
                var plantComponent = orchard.gameObject.GetComponent<Plant>();
                plantComponent.enabled = true;
                //plotGridObject.Plant = plantPrefab.GetComponent<Plant>();

                _gridObjectController.Register(new OrchardGridObject
                {
                    Cell = cell,
                    Type = GridObjectType.Orchard,
                    Plant = plantComponent,
                    Controller = orchard
                });
                
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
                
                _inventoryService.RemoveItem(_tryingToPlant!);

                _tryingToPlant = null;
            }, () =>
            {
                Destroy(orchard.gameObject);
                _currentlyPlacing = null;
                _tryingToPlant = null;
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
                return;

            if (_currentlyPlacing || !_gridSelectionController.Active)
                return;

            _tryingToPlant = selectedItem;
            _orchardPrefab = selectedItem.PlacePrefab.GetComponent<PhysicalOrchardController>();
            
            StartPlaceNew();
        }
    }
}