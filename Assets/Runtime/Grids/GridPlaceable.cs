using UnityEngine;
using UnityEngine.Events;

namespace Lunaculture.Grids
{
    public class GridPlaceable : MonoBehaviour
    {
        [field: SerializeField]
        public GameObject ValidHologramPrefab { get; private set; } = null!;
        
        [field: SerializeField]
        public GameObject InvalidHologramPrefab { get; private set; } = null!;

        [field: SerializeField]
        public UnityEvent<bool> ValidPlacementEvent { get; private set; } = null!;
    }
}