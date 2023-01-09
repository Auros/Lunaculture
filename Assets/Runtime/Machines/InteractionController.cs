using Lunaculture.Grids;
using Lunaculture.Grids.Objects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Lunaculture.Machines
{
    public abstract class InteractionController : MonoBehaviour
    {
        [SerializeField]
        protected GridObjectController _gridObjectController = null!;
        
        [SerializeField]
        protected GridSelectionController _gridSelectionController = null!;
        
        private bool _active = true;
        private GridObject? _lastGridObject;
        
        protected abstract void OnGridObjectClicked(GridObject gridObject);
        
        protected abstract void OnHoveredGridObjectChange(GridObject? gridObject);

        private void Update()
        {
            var cell = _gridSelectionController.CurrentCell;
            if (!cell.HasValue)
                return;

            var gridObject = _gridObjectController.GetObjectAt(cell.Value);
            if (gridObject is ChildGridObject child)
                gridObject = _gridObjectController.GetObjectAt(child.Parent);
            
            if (_lastGridObject == gridObject || !_active)
                return;
            
            _lastGridObject = gridObject;
            OnHoveredGridObjectChange(gridObject);
        }

        public void OnSelect(InputAction.CallbackContext ctx)
        {
            if (!ctx.performed || _lastGridObject is null || !_active)
                return;

            OnGridObjectClicked(_lastGridObject);
        }
        
        public void Disable(bool state)
        {
            _active = !state;
        }
    }
}