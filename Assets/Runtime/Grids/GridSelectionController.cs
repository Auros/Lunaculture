using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Lunaculture.Grids
{
    public class GridSelectionController : MonoBehaviour
    {
        [SerializeField]
        private Camera _camera = null!;

        [SerializeField]
        private LayerMask _gridLayer;
        
        [SerializeField]
        private GridController _gridController = null!;
        
        [SerializeField]
        private GridPlaceable _startWithSelection = null!;

        private GridCell? _mostRecentCell;
        private Action<GridCell>? _onPlaced;
        private bool? _currentPlaceableValid;
        private GridPlaceable? _currentPlaceable;
        private GameObject? _currentHologramInstance;
        private Func<GridCell, bool>? _validityEvaluator;
        private GridCenterOverride? _currentHologramOverride;

        private void Start()
        {
            if (_startWithSelection.AsNull() is null)
                return;
            
            StartSelection(_startWithSelection, cell => cell.X >= 0 && cell.Y >= 0, null);
        }

        [UsedImplicitly]
        protected void OnSelection(InputAction.CallbackContext _)
        {
            if (!_mostRecentCell.HasValue)
                return;

            var cell = _mostRecentCell.Value;
            var isValid = _validityEvaluator?.Invoke(cell) ?? true;
            if (!isValid)
                return;
            
            StopActiveSelection();
            _onPlaced?.Invoke(cell);
        }

        [UsedImplicitly]
        protected void OnPositionChange(InputAction.CallbackContext ctx)
        {
            // If we're not trying to place something, do nothing.
            if (_currentPlaceable.AsNull() is null)
                return;
            
            _mostRecentCell = null;
            var value = ctx.ReadValue<Vector2>();
            var ray = _camera.ScreenPointToRay(value);

            if (!Physics.Raycast(ray, out var hit, 100f, _gridLayer))
            {
                DeleteHologramView();
                return;
            }

            var (x, _, z) = hit.point;

            var gridCell = _gridController.GetCellAt(new Vector2(x, z));

            if (!gridCell.HasValue)
            {
                DeleteHologramView();
                return;
            }
            var validPlacement = _validityEvaluator?.Invoke(gridCell.Value) ?? true;
            SwitchHologramView(validPlacement);

            var placeable = _currentHologramInstance;
            if (placeable.AsNull() is null)
            {
                Debug.LogWarning("Unexpected: cannot find placeable hologram instance");
                return; // ??
            }
            
            _gridController.MoveGameObjectToCellCenter(gridCell.Value, placeable!, _currentHologramOverride);
            _mostRecentCell = gridCell;
        }

        public void StartSelection(GridPlaceable gridPlaceable, Func<GridCell, bool>? validityEvaluator, Action<GridCell>? onPlaced)
        {
            _onPlaced = onPlaced;
            _currentPlaceable = gridPlaceable;
            _validityEvaluator = validityEvaluator;
        }

        public void StopActiveSelection()
        {
            DeleteHologramView();
            _mostRecentCell = null;
            _currentPlaceable = null;
            _validityEvaluator = null;
        }

        private void SwitchHologramView(bool valid)
        {
            if (_currentPlaceable.AsNull() is null)
                return;

            if (valid)
            {
                // If the hologram is already set to valid.
                if (_currentPlaceableValid.HasValue && _currentPlaceableValid.Value)
                    return;

                if (_currentHologramInstance.AsNull() is not null)
                    Destroy(_currentHologramInstance);

                _currentPlaceableValid = true;
                _currentHologramInstance = Instantiate(_currentPlaceable!.ValidHologramPrefab);
                _currentHologramOverride = _currentHologramInstance!.GetComponent<GridCenterOverride>();
            }
            else
            {
                // If the hologram is already set to invalid.
                if (_currentPlaceableValid.HasValue && !_currentPlaceableValid.Value)
                    return;
                
                if (_currentHologramInstance.AsNull() is not null)
                    Destroy(_currentHologramInstance);

                _currentPlaceableValid = false;
                _currentHologramInstance = Instantiate(_currentPlaceable!.InvalidHologramPrefab);
                _currentHologramOverride = _currentHologramInstance!.GetComponent<GridCenterOverride>();
            }
        }

        private void DeleteHologramView()
        {
            if (_currentHologramInstance.AsNull() is not null)
                Destroy(_currentHologramInstance);

            _currentPlaceableValid = null;
            _currentHologramInstance = null;
            _currentHologramOverride = null;
        }
    }
}