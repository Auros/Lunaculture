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
        
        private GridCell? _mostRecentCell;
        private bool? _currentPlaceableValid;
        private GridPlaceable? _currentPlaceable;
        private GameObject? _currentHologramInstance;
        private Func<GridCell, bool>? _validityEvaluator;
        private GridCenterOverride? _currentHologramOverride;

        [UsedImplicitly]
        protected void OnSelection(InputAction.CallbackContext _)
        {
            if (!_mostRecentCell.HasValue)
                return;
            
            var isValid = _validityEvaluator?.Invoke(_mostRecentCell.Value) ?? true;
            if (!isValid)
                return;
            
            StopActiveSelection();
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
            var cellWorldCenter = _gridController.GetCellWorldCenter(gridCell.Value);
            SwitchHologramView(validPlacement);

            var placeable = _currentHologramInstance;
            if (placeable.AsNull() is null)
            {
                Debug.LogWarning("Unexpected: cannot find placeable hologram instance");
                return; // ??
            }
            
            var selectableTransform = placeable!.transform;
            var gridY = _gridController.transform.position.y;
            
            selectableTransform.position = new Vector3(cellWorldCenter.x, gridY, cellWorldCenter.y);
            
            var overrideOffset = _currentHologramOverride.AsNull() is null
                ? Vector3.zero
                : selectableTransform.position - _currentHologramOverride!.Offset.position;
            
            selectableTransform.position += overrideOffset;
            _mostRecentCell = gridCell;
        }

        public void StartSelection(GridPlaceable gridPlaceable, Func<GridCell, bool>? validityEvaluator)
        {
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
