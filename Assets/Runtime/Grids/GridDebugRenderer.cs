using UnityEngine;

namespace Lunaculture.Grids
{
    [RequireComponent(typeof(GridController))]
    public class GridDebugRenderer : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            var gridController = GetComponent<GridController>();
            var cellSize = gridController.CellSize;
            var (upperBoundX, upperBoundY) = gridController.BottomLeftCorner;
            var topRightCorner = gridController.TopRightCorner;
            
            var selfTransform = transform;
            var selfPos = selfTransform.position;

            var lowerBoundX = topRightCorner.x - cellSize;
            var lowerBoundY = topRightCorner.y - cellSize;

            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(selfPos, 0.15f);
            
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(selfPos + new Vector3(upperBoundX, 0, upperBoundY), 0.1f);
            Gizmos.DrawSphere(selfPos + new Vector3(topRightCorner.x, 0, topRightCorner.y), 0.1f);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(selfPos + new Vector3(upperBoundX, 0, upperBoundY), 0.08f);
            Gizmos.DrawSphere(selfPos + new Vector3(lowerBoundX, 0, lowerBoundY), 0.08f);
            Gizmos.color = Color.white;

            for (float x = upperBoundX; x <= lowerBoundX; x += cellSize)
            {
                for (float y = upperBoundY; y <= lowerBoundY; y += cellSize)
                {
                    Gizmos.DrawSphere(selfPos + new Vector3(x, 0, y), 0.05f);
                }
            }
        }
    }
}