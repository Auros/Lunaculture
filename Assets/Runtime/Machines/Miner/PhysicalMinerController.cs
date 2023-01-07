using Lunaculture.Grids;
using Lunaculture.Placement;
using UnityEngine;

namespace Lunaculture.Machines.Miner
{
    public class PhysicalMinerController : MonoBehaviour
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
        private PhysicalMinerController _template = null!;
        
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
                _gridController.MoveGameObjectToCellCenter(cell, gameObject);
                return !_overlapDetector.IsOverlapping();
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