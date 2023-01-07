using UnityEngine;

namespace Lunaculture.Grids
{
    public class GridCenterOverride : MonoBehaviour
    {
        [field: SerializeField]
        public Transform Offset { get; private set; } = null!;
    }
}