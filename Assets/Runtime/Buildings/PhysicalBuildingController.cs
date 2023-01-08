﻿using Lunaculture.Grids;
using Lunaculture.Placement;
using UnityEngine;

namespace Lunaculture.Buildings
{
    public class PhysicalBuildingController : MonoBehaviour
    {
        [field: SerializeField]
        public GridPlaceable Placeable { get; private set; } = null!;

        [SerializeField]
        private GameObject _visualContainer = null!;
        
        [field: SerializeField]
        public PlaceableObjectOverlapChecker OverlapDetector { get; private set; } = null!;

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
    }
}