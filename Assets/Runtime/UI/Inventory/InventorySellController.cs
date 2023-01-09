using Lunaculture.Player.Currency;
using Lunaculture.Player.Inventory;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Lunaculture.UI.Inventory
{
    public class InventorySellController : MonoBehaviour
    {
        [SerializeField] private ToastNotificationController toastNotificationController = null!;
        [SerializeField] private GameUIInterconnect gameUIInterconnect = null!;

        private CurrencyService currencyService = null!;
        private InventoryService inventoryService = null!;

        private bool massSell = false;

        private void Start()
        {
            currencyService = gameUIInterconnect.CurrencyService;
            inventoryService = gameUIInterconnect.InventoryService;
        }

        public void OnShift(InputAction.CallbackContext context) => massSell = context.performed;

        private void OnSlotCellClicked(SlotCell cell)
        {
            if (cell.AssignedStack is { Empty: false } && cell.AssignedStack.ItemType!.CanSell)
            {
                var count = massSell ? cell.AssignedStack.Count : 1;

                for (var i = 0; i < count; i++)
                {
                    inventoryService.RemoveItem(cell.AssignedStack.ItemType);
                }

                var sell = cell.AssignedStack.ItemType.SellPrice * count;
                currencyService.Currency += sell;

                toastNotificationController.SummonToast($"+{sell} credits.");
            }
        }

        private void OnSlotCellAssigned(SlotCell cell)
        {
            cell.Transparency = cell.AssignedStack.Empty || cell.AssignedStack.ItemType!.CanSell ? 1 : 0.2f;
        }
    }
}
