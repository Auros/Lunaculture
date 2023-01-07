using UnityEngine;
using UnityEngine.InputSystem;

namespace Lunaculture.Grids
{
    public class GridSelectionController : MonoBehaviour
    {
        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private LayerMask _gridLayer;
        
        [SerializeField]
        private GridController _gridController;
        
        [SerializeField]
        private GridCenterOverride _selectable;

        private GridPlaceable _currentPlaceable;
        
        public void OnSelection(InputAction.CallbackContext context)
        {
            
        }

        public void OnPositionChange(InputAction.CallbackContext context)
        {
            var value = context.ReadValue<Vector2>();
            var ray = _camera.ScreenPointToRay(value);

            if (!Physics.Raycast(ray, out var hit, 100f, _gridLayer))
                return;

            var (x, _, z) = hit.point;

            var gridCell = _gridController.GetCellAt(new Vector2(x, z));
            
            if (!gridCell.HasValue)
                return;

            var cellWorldCenter = _gridController.GetCellWorldCenter(gridCell.Value);

            var selectableTransform = _selectable.transform;
            var gridY = _gridController.transform.position.y;
            var overrideOffset = selectableTransform.position - _selectable.Offset.position;
            selectableTransform.position = new Vector3(cellWorldCenter.x, gridY, cellWorldCenter.y) + overrideOffset;
        }

        public void StartSelection(GridPlaceable gridPlaceable)
        {
            
        }
    }
}
