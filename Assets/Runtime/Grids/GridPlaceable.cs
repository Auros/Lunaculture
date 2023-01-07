using UnityEngine;
using UnityEngine.Events;

namespace Lunaculture.Grids
{
    public class GridPlaceable : MonoBehaviour
    {
        [field: SerializeField]
        public GameObject ValidHologramPrefab { get; private set; }
        
        [field: SerializeField]
        public GameObject InvalidHologramPrefab { get; private set; }

        [field: SerializeField]
        public UnityEvent<bool> ValidPlacementEvent { get; private set; }
    }
}