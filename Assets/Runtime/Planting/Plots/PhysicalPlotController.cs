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
        private PlaceableObjectOverlapChecker _overlapChecker = null!;
    }
}