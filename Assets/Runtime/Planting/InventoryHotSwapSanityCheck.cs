using Lunaculture.Grids;
using Lunaculture.Player.Inventory;
using UnityEngine;

namespace Lunaculture.Planting
{
    public class InventoryHotSwapSanityCheck : MonoBehaviour
    {
        [SerializeField]
        private InventoryService _inventoryService = null!;

        [SerializeField]
        private GridSelectionController _gridSelectionController = null!;

        private void OnEnable()
        {
            _inventoryService.HotSwapEvent += OnHotSwap;
        }

        private void OnHotSwap()
        {
            _gridSelectionController.StopActiveSelection(true);
        }

        private void OnDisable()
        {
            _inventoryService.HotSwapEvent -= OnHotSwap;
        }
    }
}