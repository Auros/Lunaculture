using UnityEngine;

namespace Lunaculture.Grids
{
    public class GridPlaceable : MonoBehaviour
    {
        [field: SerializeField]
        public GameObject ValidHologramPrefab { get; private set; } = null!;
        
        [field: SerializeField]
        public GameObject InvalidHologramPrefab { get; private set; } = null!;
    }
}