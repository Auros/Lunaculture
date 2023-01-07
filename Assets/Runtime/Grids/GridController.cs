using UnityEngine;

namespace Lunaculture.Grids
{
    public class GridController : MonoBehaviour
    {
        [field: Min(0.5f)]
        [field: SerializeField]
        public float CellSize { get; private set; } = 1f;
        
        [field: SerializeField]
        public Vector2 BottomLeftCorner { get; private set; }

        [field: SerializeField]
        public Vector2 TopRightCorner { get; private set; }
    }
}