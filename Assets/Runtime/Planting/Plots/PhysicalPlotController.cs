using Lunaculture.Grids;
using Lunaculture.Placement;
using UnityEngine;

namespace Lunaculture.Planting.Plots
{
    public class PhysicalPlotController : MonoBehaviour
    {
        [SerializeField]
        private GridController _gridController = null!;

        [SerializeField]
        private GridSelectionController _gridSelectionController = null!;

        [SerializeField]
        private GridPlaceable _placeable = null!;

        [SerializeField]
        private GameObject _visualContainer = null!;

        [SerializeField]
        private PlaceableObjectOverlapChecker _overlapDetector = null!;

        [SerializeField]
        private PhysicalPlotController _template = null!;

        [SerializeField]
        private LayerMask _indoorLayer;
        
        private void Start()
        {
            Move();
        }

        public void Move()
        {
            _overlapDetector.enabled = true;
            _visualContainer.SetActive(false);
            _gridSelectionController.StartSelection(_placeable,
                cell =>
                {
                    var center = _gridController.GetCellWorldCenter(cell);
                    var rayStart = new Vector3(center.x, _gridController.transform.position.y + 0.5f, center.y);
                    var inside = Physics.Raycast(rayStart, Vector3.down, 10, _indoorLayer);
                    _gridController.MoveGameObjectToCellCenter(cell, gameObject);
                    return inside && !_overlapDetector.IsOverlapping();
                },
                cell =>
                {
                    Place();
                    Instantiate(_template);
                });
        }

        public void Place()
        {
            _visualContainer.SetActive(true);
            _overlapDetector.enabled = false;
        }
    }
}