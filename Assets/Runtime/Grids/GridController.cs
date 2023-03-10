using UnityEngine;

namespace Lunaculture.Grids
{
    public class GridController : MonoBehaviour
    {
        [field: Min(0.25f)]
        [field: SerializeField]
        public float CellSize { get; private set; } = 1f;
        
        [field: SerializeField]
        public Vector2 BottomLeftCorner { get; private set; }

        [field: SerializeField]
        public Vector2 TopRightCorner { get; private set; }

        public GridCell? GetCellAt(Vector2 worldPosition)
        {
            var (x, y) = worldPosition;
            var half = CellSize * 0.5f;
            x -= half;
            y -= half;

            var cellX = RoundWithPrecision(x, CellSize);
            var cellY = RoundWithPrecision(y, CellSize);

            // If the theoretical cell's computed X position is out of bounds, return null.
            if (cellX >= TopRightCorner.x || cellX < BottomLeftCorner.x)
                return null;
            
            // If the theoretical cell's computed Y position is out of bounds, return null.
            if (cellY >= TopRightCorner.y || cellY < BottomLeftCorner.y)
                return null;

            return new GridCell(cellX, cellY);
        }

        public Vector2 GetCellWorldCenter(GridCell gridCell)
        {
            var (x, y, _) = gridCell;
            var half = CellSize * 0.5f;
            x += half;
            y += half;
            return new Vector2(x, y);
        }
        
        public void MoveGameObjectToCellCenter(GridCell gridCell, GameObject objectToMove, GridCenterOverride? centerOverride = null)
        {
            var worldCenter = GetCellWorldCenter(gridCell);
            var moveTransform = objectToMove.transform;
            var gridY = transform.position.y;
            moveTransform.position = new Vector3(worldCenter.x, gridY, worldCenter.y);

            if (centerOverride.AsNull() is null)
                centerOverride = objectToMove.GetComponent<GridCenterOverride>();
            var overrideOffset = centerOverride.AsNull() is null
                ? Vector3.zero
                : moveTransform.position - centerOverride!.Offset.position;

            moveTransform.position += overrideOffset;
        }

        private static float RoundWithPrecision(float v, float precision)
        {
            return Mathf.RoundToInt(v / precision) * precision;
        }
    }
}