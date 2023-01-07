using Lunaculture.Grids;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Lunaculture
{
    public class GridSelectionController : MonoBehaviour
    {
        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private LayerMask _gridLayer;
        
        [SerializeField]
        private GridController _gridController;
        
        public void OnSelection(InputAction.CallbackContext context)
        {
            
        }

        public void OnMousePositionChange(InputAction.CallbackContext context)
        {
            var value = context.ReadValue<Vector2>();
            var ray = _camera.ScreenPointToRay(value);

            if (!Physics.Raycast(ray, out var hit, 100f, _gridLayer))
                return;

            var (x, _, z) = hit.point;

            var gridCell = _gridController.GetCellAt(new Vector2(x, z));

            if (!gridCell.HasValue)
                return;
            
            print(gridCell.Value.Id);
        }
    }
}
