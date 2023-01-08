using Lunaculture.Grids;
using Lunaculture.Grids.Objects;
using Lunaculture.Items;
using Lunaculture.Player.Inventory;
using UnityEngine;

namespace Lunaculture.Machines.Miner
{
    public class MinerPlacingController : MonoBehaviour
    {
        [SerializeField]
        private GridController _gridController = null!;

        [SerializeField]
        private GridObjectController _gridObjectController = null!;
        
        [SerializeField]
        private GridSelectionController _gridSelectionController = null!;
        
        [SerializeField]
        private PhysicalMinerController _minerPrefab = null!;

        [SerializeField]
        private InventoryService _inventoryService = null!;
        
        [SerializeField]
        private LayerMask _indoorLayer;

        [SerializeField]
        private Item _machineItem = null!;

        private PhysicalMinerController? _currentlyPlacing;
        private readonly GridCell[] _neighbors = new GridCell[8];
        
        private void StartPlaceNew()
        {
            var miner = Instantiate(_minerPrefab);
            miner.PrepareMovement();
            _gridSelectionController.StartSelection(miner.Placeable, cell =>
            {
                var center = _gridController.GetCellWorldCenter(cell);
                var rayStart = new Vector3(center.x, _gridController.transform.position.y + 0.5f, center.y);
                var inside = Physics.Raycast(rayStart, Vector3.down, 10, _indoorLayer);
                _gridController.MoveGameObjectToCellCenter(cell, miner.gameObject);

                _neighbors[0] = new GridCell(cell.X + 1, cell.Y);
                _neighbors[1] = new GridCell(cell.X - 1, cell.Y);
                _neighbors[2] = new GridCell(cell.X, cell.Y + 1);
                _neighbors[3] = new GridCell(cell.X, cell.Y - 1);
                _neighbors[4] = new GridCell(cell.X + 1, cell.Y + 1);
                _neighbors[5] = new GridCell(cell.X - 1, cell.Y + 1);
                _neighbors[6] = new GridCell(cell.X + 1, cell.Y - 1);
                _neighbors[7] = new GridCell(cell.X - 1, cell.Y - 1);

                // Check if neighbors are empty
                // ReSharper disable once LoopCanBeConvertedToQuery
                foreach (var neighbor in _neighbors)
                    if (_gridObjectController.GetObjectAt(neighbor) is not null)
                        return false;
                
                return inside && !miner.OverlapDetector.IsOverlapping();
            }, cell =>
            {
                miner.PreparePlacement();
                _currentlyPlacing = null;
                _gridObjectController.Register(new MinerGridObject
                {
                    Cell = cell,
                    Type = GridObjectType.Orchard
                });
                
                _neighbors[0] = new GridCell(cell.X + 1, cell.Y);
                _neighbors[1] = new GridCell(cell.X - 1, cell.Y);
                _neighbors[2] = new GridCell(cell.X, cell.Y + 1);
                _neighbors[3] = new GridCell(cell.X, cell.Y - 1);
                _neighbors[4] = new GridCell(cell.X + 1, cell.Y + 1);
                _neighbors[5] = new GridCell(cell.X - 1, cell.Y + 1);
                _neighbors[6] = new GridCell(cell.X + 1, cell.Y - 1);
                _neighbors[7] = new GridCell(cell.X - 1, cell.Y - 1);

                foreach (var neighbor in _neighbors)
                {
                    _gridObjectController.Register(new ChildGridObject
                    {
                        Cell = neighbor,
                        Parent = cell,
                        Type = GridObjectType.Child
                    });
                }
            }, () =>
            {
                Destroy(miner);
                _currentlyPlacing = null;
            });
            _currentlyPlacing = miner;
        }

        private void Update()
        {
            if (_inventoryService.SelectedItem.AsNull() is null)
                return;

            var selectedItem = _inventoryService.SelectedItem!;
            if (selectedItem != _machineItem)
            {
                return;
            }

            if (_currentlyPlacing || !_gridSelectionController.Active)
                return;

            StartPlaceNew();
        }
    }
}