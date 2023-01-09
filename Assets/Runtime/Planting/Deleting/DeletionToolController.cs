using System;
using Lunaculture.Grids;
using Lunaculture.Grids.Objects;
using Lunaculture.Items;
using Lunaculture.Player.Inventory;
using UnityEngine;

namespace Lunaculture.Planting.Deleting
{
    public class DeletionToolController : MonoBehaviour
    {
        [SerializeField]
        private GridObjectController _gridObjectController = null!;
        
        [SerializeField]
        private GridSelectionController _gridSelectionController = null!;
        
        [SerializeField]
        private InventoryService _inventoryService = null!;
        
        [SerializeField]
        private Item _deletionIcon = null!;

        [SerializeField]
        private GridPlaceable _hologram = null!;

        private bool _tryingToDelete;
        
        private void StartDeletionProcess()
        {
            _gridSelectionController.StartSelection(_hologram, cell =>
            {
                var gridObject = _gridObjectController.GetObjectAt(cell);
                
                // ReSharper disable once ConvertIfStatementToSwitchStatement
                if (gridObject is null)
                    return false;

                if (gridObject is ChildGridObject child)
                    gridObject = _gridObjectController.GetObjectAt(child.Parent);

                var type = gridObject?.Type;
                return type is GridObjectType.Miner or GridObjectType.Orchard or GridObjectType.Plot;
            }, cell =>
            {
                var gridObject = _gridObjectController.GetObjectAt(cell);
                
                switch (gridObject)
                {
                    case null:
                        throw new InvalidOperationException("Could not find plot to delete");
                    case ChildGridObject child:
                        gridObject = _gridObjectController.GetObjectAt(child.Parent);
                        break;
                }

                var type = gridObject?.Type;
                if (gridObject is null || type is not (GridObjectType.Miner or GridObjectType.Orchard or GridObjectType.Plot))
                    return;

                _gridObjectController.Unregister(gridObject);
                switch (gridObject)
                {
                    case MinerGridObject miner:
                        Destroy(miner.Controller.gameObject);

                        var neighbors = new GridCell[8];
                        cell = gridObject.Cell;
                        neighbors[0] = new GridCell(cell.X + 1, cell.Y);
                        neighbors[1] = new GridCell(cell.X - 1, cell.Y);
                        neighbors[2] = new GridCell(cell.X, cell.Y + 1);
                        neighbors[3] = new GridCell(cell.X, cell.Y - 1);
                        neighbors[4] = new GridCell(cell.X + 1, cell.Y + 1);
                        neighbors[5] = new GridCell(cell.X - 1, cell.Y + 1);
                        neighbors[6] = new GridCell(cell.X + 1, cell.Y - 1);
                        neighbors[7] = new GridCell(cell.X - 1, cell.Y - 1);
                        
                        foreach (var neighbor in neighbors)
                            _gridObjectController.Unregister(_gridObjectController.GetObjectAt(neighbor)!);
                        
                        break;
                    case PlotGridObject plot:
                    {
                        if (plot.Plant)
                            Destroy(plot.Plant!.gameObject);
                    
                        Destroy(plot.Controller.gameObject);
                        
                        break;
                    }
                    case OrchardGridObject orchard:
                    {
                        if (orchard.Plant)
                            Destroy(orchard.Plant!.gameObject);
                        Destroy(orchard.Controller.gameObject);
                        
                        cell = gridObject.Cell;
                        GridCell left = new(cell.X, cell.Y + 1);
                        GridCell right = new(cell.X + 1, cell.Y);
                        GridCell far = new(cell.X + 1, cell.Y + 1);
                        
                        _gridObjectController.Unregister(_gridObjectController.GetObjectAt(left)!);
                        _gridObjectController.Unregister(_gridObjectController.GetObjectAt(right)!);
                        _gridObjectController.Unregister(_gridObjectController.GetObjectAt(far)!);
                        
                        break;
                    }
                }
                
                _tryingToDelete = false;
            }, () =>
            {
                _tryingToDelete = false;
            });
            _tryingToDelete = true;
        }
        
        private void Update()
        {
            if (_inventoryService.SelectedItem.AsNull() is null)
                return;

            var selectedItem = _inventoryService.SelectedItem!;
            if (selectedItem != _deletionIcon)
                return;

            if (_tryingToDelete || !_gridSelectionController.Active)
                return;

            StartDeletionProcess();
        }
    }
}