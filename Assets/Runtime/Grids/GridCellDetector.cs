using UnityEngine;

namespace Lunaculture.Grids
{
    public class GridCellDetector : MonoBehaviour
    {
        [SerializeField]
        private GridController _gridController = null!;

        private void Update()
        {
            var selfPos = transform.position;
            var cell = _gridController.GetCellAt(new Vector2(selfPos.x, selfPos.z));
            if (!cell.HasValue)
            {
                Debug.Log("Could not find cell!");
                return;
            }

            var value = cell.Value;
            Debug.Log($"Cell at ({value.X}, {value.Y}):({value.Id}):({Time.deltaTime})");
        }
    }
}