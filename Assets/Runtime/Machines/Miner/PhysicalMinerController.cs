using Lunaculture.Grids;
using Lunaculture.Items;
using Lunaculture.Placement;
using System;
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
        private PhysicalMinerController? _template;

        [SerializeField]
        private Item _generatedResource = null!;

        [Header("Generation rate is measured in seconds.")]
        [SerializeField]
        private float _generationRate = 1.0f;

        [SerializeField]
        private int _maxResourceSize = 100;

        private float _generationTime = 0;
        private int _currentItems = 0;


        private void Start()
        {
            _ = _generatedResource;
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

                if (_template is not null)
                {
                    Instantiate(_template);
                }
            });
        }

        public void Place()
        {
            _visualContainer.SetActive(true);
            _overlapDetector.enabled = false;
            
            // TODO: Better cancel item generation while miner is in placement mode
            _currentItems = 0;
        }

        private void Update()
        {
            _generationTime += Time.deltaTime * Time.timeScale;

            if (_generationTime > _generationRate)
            {
                _generationTime = 0;
                _currentItems = Math.Min(_currentItems + 1, _maxResourceSize);
            }
        }
    }
}