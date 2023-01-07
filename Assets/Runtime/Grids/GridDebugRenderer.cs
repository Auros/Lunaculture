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
            Gizmos.DrawSphere(selfPos, 0.5f);
            
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(selfPos + new Vector3(upperBoundX, 0, upperBoundY), 0.3f);
            Gizmos.DrawSphere(selfPos + new Vector3(topRightCorner.x, 0, topRightCorner.y), 0.3f);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(selfPos + new Vector3(upperBoundX, 0, upperBoundY), 0.2f);
            Gizmos.DrawSphere(selfPos + new Vector3(lowerBoundX, 0, lowerBoundY), 0.2f);
            Gizmos.color = Color.white;

            //Gizmos.DrawCube(selfPos, Vector3.Distance(topRightCorner, gridController.BottomLeftCorner) * 0.5f * Vector3.one);
            
            /*for (float x = upperBoundX; x <= lowerBoundX; x += cellSize)
            {
                for (float y = upperBoundY; y <= lowerBoundY; y += cellSize)
                {
                    Gizmos.DrawSphere(selfPos + new Vector3(x, 0, y), 0.05f);
                }
            }*/
        }
    }
}