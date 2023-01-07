using Lunaculture.Items;
using Lunaculture.Player.Inventory;
using UnityEngine;

namespace Lunaculture.UI.Inventory
{
    public class InventoryUIController : MonoBehaviour
    {
        [SerializeField] private SlotCell slotCellPrefab;
        [SerializeField] private Item debugItem;
        [SerializeField] private GameUIInterconnect gameUIInterconnect;

        private SlotCell[] inventorySlots;
        private InventoryService inventoryService;

        private void OnEnable()
        {
            if (inventorySlots == null)
            {
                inventoryService = gameUIInterconnect.InventoryService;

                inventorySlots = new SlotCell[inventoryService.Inventory.Length];
                for (var i = 0; i < inventorySlots.Length; i++)
                {
                    inventorySlots[i] = Instantiate(slotCellPrefab, transform);
                    inventorySlots[i].AssignItemStack(null);
                }

                inventoryService.InventoryUpdatedEvent += RebuildInventoryUI;
            }

            RebuildInventoryUI();
        }

        private void RebuildInventoryUI()
        {
            for (var i = 0; i < inventoryService.Inventory.Length; i++)
            {
                inventorySlots[i].AssignItemStack(inventoryService.Inventory[i]);

                var selectedSlot = inventorySlots[i].AssignedStack != null && inventorySlots[i].AssignedStack.ItemType == inventoryService.SelectedItem;
                inventorySlots[i].ToggleSelectionIcon(selectedSlot);
            }
        }

        private void OnDestroy()
        {
            inventoryService.InventoryUpdatedEvent -= RebuildInventoryUI;
        }
    }
}
