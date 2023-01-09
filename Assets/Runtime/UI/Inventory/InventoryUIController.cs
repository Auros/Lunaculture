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
        
        private bool _ready = false;

        private void Start()
        {
            _ = debugItem;
            if (!_ready)
            {
                inventoryService = gameUIInterconnect.InventoryService;

                inventorySlots = new SlotCell[inventoryService.Inventory.Length];
                for (var i = 0; i < inventorySlots.Length; i++)
                {
                    inventorySlots[i] = Instantiate(slotCellPrefab, transform);
                    inventorySlots[i].AssignItemStack(null);
                }

                inventoryService.InventoryUpdatedEvent += RebuildInventoryUI;

                _ready = true;
            }

            RebuildInventoryUI();
        }

        private void OnEnable()
        {
            if (_ready) RebuildInventoryUI();   
        }

        private void RebuildInventoryUI()
        {
            if (!_ready) return;

            for (var i = 0; i < inventoryService!.Inventory.Length; i++)
            {
                var slot = inventorySlots![i];
                slot.AssignItemStack(inventoryService.Inventory[i]);

                var assignedStack = slot.AssignedStack;
                var selectedSlot = assignedStack != null && !assignedStack.Empty && assignedStack.ItemType == inventoryService.SelectedItem;
                inventorySlots[i].ToggleSelectionIcon(selectedSlot);
            }
        }

        private void OnDestroy()
        {
            inventoryService!.InventoryUpdatedEvent -= RebuildInventoryUI;
        }
    }
}
