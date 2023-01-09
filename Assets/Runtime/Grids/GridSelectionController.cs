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

        private Action? _onCancel;
        private GridCell? _mostRecentCell;
        private Action<GridCell>? _onPlaced;
        private bool? _currentPlaceableValid;
        private GridPlaceable? _currentPlaceable;
        private GameObject? _currentHologramInstance;
        private Func<GridCell, bool>? _validityEvaluator;
        private GridCenterOverride? _currentHologramOverride;

        private bool _holdingDownSelect;
        public bool Active { get; private set; } = true;

        public GridCell? CurrentCell => _mostRecentCell;

        [UsedImplicitly]
        public void OnSelection(InputAction.CallbackContext ctx)
        {
            _holdingDownSelect = ctx.performed || ctx.started;
            if (!Active)
                return;

            // Only repond to Mouse Down
            if (!ctx.performed)
                return;
            
            TryPlace();
        }

        private void Update()
        {
            if (!_holdingDownSelect || !Active)
                return;
            
            TryPlace();
        }

        private void TryPlace()
        {
            if (!_mostRecentCell.HasValue)
                return;

            var cell = _mostRecentCell.Value;
            var isValid = _validityEvaluator?.Invoke(cell) ?? true;
            if (!isValid)
                return;
            
            _onPlaced?.Invoke(cell);
            StopActiveSelection();
        }

        [UsedImplicitly]
        public void OnPositionChange(InputAction.CallbackContext ctx)
        {
            _mostRecentCell = null;
            var tryingToPlace = _currentPlaceable.AsNull() is null;
            var value = ctx.ReadValue<Vector2>();

            if (_camera == null) return;

            var ray = _camera.ScreenPointToRay(value);

            if (!Physics.Raycast(ray, out var hit, 100f, _gridLayer))
            {
                if (tryingToPlace)
                    DeleteHologramView();
                return;
            }

            var (x, _, z) = hit.point;
            var gridCell = _gridController.GetCellAt(new Vector2(x, z));
            _mostRecentCell = gridCell;

            if (tryingToPlace)
                return;

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
        }

        public void StartSelection(GridPlaceable gridPlaceable, Func<GridCell, bool>? validityEvaluator, Action<GridCell>? onPlaced, Action? onCancel = null)
        {
            _onPlaced = onPlaced;
            _onCancel = onCancel;
            _currentPlaceable = gridPlaceable;
            _validityEvaluator = validityEvaluator;
        }

        public void StopActiveSelection(bool sendCancelEvent = false)
        {
            DeleteHologramView();
            
            if (sendCancelEvent)
                _onCancel?.Invoke();
            
            _onPlaced = null;
            _onCancel = null;
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

        public void Disable(bool state)
        {
            if (state)
                StopActiveSelection(true);
            Active = !state;
        }
    }
}
