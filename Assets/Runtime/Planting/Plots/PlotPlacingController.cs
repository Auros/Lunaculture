using Lunaculture.Grids;
using Lunaculture.Grids.Objects;
using Lunaculture.Items;
using Lunaculture.Player.Inventory;
using UnityEngine;

namespace Lunaculture.Planting.Plots
{
    public class PlotPlacingController : MonoBehaviour
    {
        [SerializeField]
        private GridController _gridController = null!;

        [SerializeField]
        private GridObjectController _gridObjectController = null!;
        
        [SerializeField]
        private GridSelectionController _gridSelectionController = null!;
        
        [SerializeField]
        private PhysicalPlotController _plotPrefab = null!;

        [SerializeField]
        private InventoryService _inventoryService = null!;
        
        [SerializeField]
        private LayerMask _indoorLayer;

        [SerializeField]
        private Item _plotPlacingItem = null!;

        private PhysicalPlotController? _currentlyPlacing;
        
        private void StartPlaceNew()
        {
            var plot = Instantiate(_plotPrefab);
            plot.PrepareMovement();
            _gridSelectionController.StartSelection(plot.Placeable, cell =>
            {
                var gridObject = _gridObjectController.GetObjectAt(cell);
                if (gridObject != null) return false;
                
                var center = _gridController.GetCellWorldCenter(cell);
                var rayStart = new Vector3(center.x, _gridController.transform.position.y + 0.5f, center.y);
                var inside = Physics.Raycast(rayStart, Vector3.down, 10, _indoorLayer);
                _gridController.MoveGameObjectToCellCenter(cell, plot.gameObject);
                return inside && !plot.OverlapDetector.IsOverlapping();
            }, cell =>
            {
                var gridObject = _gridObjectController.GetObjectAt(cell);
                
                if (gridObject != null)
                {
                    if (plot)
                        Destroy(plot.gameObject);
                    _currentlyPlacing = null;
                    return;
                }
                
                plot.PreparePlacement();
                _currentlyPlacing = null;
                _gridObjectController.Register(new PlotGridObject
                {
                    Cell = cell,
                    Type = GridObjectType.Plot,
                    Controller = plot
                });
            }, () =>
            {
                if (plot)
                    Destroy(plot.gameObject);
                _currentlyPlacing = null;
            });
            _currentlyPlacing = plot;
        }

        private void Update()
        {
            if (_inventoryService.SelectedItem.AsNull() is null)
                return;

            var selectedItem = _inventoryService.SelectedItem!;
            if (selectedItem != _plotPlacingItem)
                return;

            if (_currentlyPlacing || !_gridSelectionController.Active)
                return;

            StartPlaceNew();
        }
    }
}