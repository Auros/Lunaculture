using Lunaculture.Items;
using Lunaculture.Player.Inventory;
using UnityEngine;

namespace Lunaculture.UI.Inventory
{
    public class InventoryItemSelectionController : MonoBehaviour
    {
        [SerializeField] private GameUIInterconnect gameUIInterconnect = null!;

        private InventoryService inventoryService = null!;

        private void Start()
        {
            inventoryService = gameUIInterconnect.InventoryService;
        }

        private void OnSlotCellClicked(SlotCell cell)
        {
            if (cell.AssignedStack == null || cell.AssignedStack.Empty || inventoryService.SelectedItem == cell.AssignedStack.ItemType)
            {
                inventoryService.SelectItem((Item?)null);
                return;
            }

            inventoryService.SelectItem(cell.AssignedStack);
        }
    }
}
