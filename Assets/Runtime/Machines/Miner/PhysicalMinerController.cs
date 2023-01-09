using Lunaculture.Grids;
using Lunaculture.Items;
using Lunaculture.Placement;
using System;
using UnityEngine;

namespace Lunaculture.Machines.Miner
{
    public class PhysicalMinerController : MonoBehaviour
    {
        [field: SerializeField]
        public GridPlaceable Placeable { get; private set; } = null!;

        [SerializeField]
        private GameObject _visualContainer = null!;
        
        [field: SerializeField]
        public PlaceableObjectOverlapChecker OverlapDetector { get; private set; } = null!;

        [SerializeField]
        private Item _generatedResource = null!;

        [Header("Generation rate is measured in seconds.")]
        [SerializeField]
        private float _generationRate = 1.0f;

        [SerializeField]
        private int _maxResourceSize = 100;

        private float _generationTime = 0;
        private int _currentItems = 0;

        private void Update()
        {
            if (!_visualContainer.activeSelf)
                return;
            
            _generationTime += Time.deltaTime * Time.timeScale;

            if (_generationRate >= _generationTime)
                return;
            
            _generationTime = 0;
            _currentItems = Math.Min(_currentItems + 1, _maxResourceSize);
        }
        
        public void PrepareMovement()
        {
            OverlapDetector.enabled = true;
            _visualContainer.SetActive(false);
        }

        public void PreparePlacement()
        {
            _visualContainer.SetActive(true);
            OverlapDetector.enabled = false;
        }

        public Item GetItem() => _generatedResource;
        
        public int GetItemCount() => _currentItems;

        public void Clear() => _currentItems = 0;
    }
}