using JetBrains.Annotations;
using Lunaculture.Items;
using Lunaculture.Player.Inventory;
using UnityEngine;

namespace Lunaculture.UI.Inventory
{
    public class InventoryUIController : MonoBehaviour
    {
        [SerializeField] private SlotCell slotCellPrefab = null!;
        [SerializeField] private GameUIInterconnect gameUIInterconnect = null!;
        
        [SerializeField]
        [UsedImplicitly]
        private Item debugItem = null!;

        private SlotCell[]? inventorySlots;
        private InventoryService? inventoryService;

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
            for (var i = 0; i < inventoryService!.Inventory.Length; i++)
            {
                var slot = inventorySlots![i];
                slot.AssignItemStack(inventoryService.Inventory[i]);

                var assignedStack = slot.AssignedStack;
                var selectedSlot = assignedStack != null && assignedStack.ItemType == inventoryService.SelectedItem;
                inventorySlots[i].ToggleSelectionIcon(selectedSlot);
            }
        }

        private void OnDestroy()
        {
            inventoryService!.InventoryUpdatedEvent -= RebuildInventoryUI;
        }
    }
}
