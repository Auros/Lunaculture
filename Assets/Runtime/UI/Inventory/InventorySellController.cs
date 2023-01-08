using Lunaculture.Player.Currency;
using Lunaculture.Player.Inventory;
using UnityEngine;

namespace Lunaculture.UI.Inventory
{
    public class InventorySellController : MonoBehaviour
    {
        [SerializeField] private ToastNotificationController toastNotificationController = null!;
        [SerializeField] private GameUIInterconnect gameUIInterconnect = null!;

        private CurrencyService currencyService = null!;
        private InventoryService inventoryService = null!;

        private void Start()
        {
            currencyService = gameUIInterconnect.CurrencyService;
            inventoryService = gameUIInterconnect.InventoryService;
        }

        private void OnSlotCellClicked(SlotCell cell)
        {
            if (cell.AssignedStack is { Empty: false } && cell.AssignedStack.ItemType!.CanSell)
            {
                inventoryService.RemoveItem(cell.AssignedStack.ItemType);

                var sell = cell.AssignedStack.ItemType.SellPrice;
                currencyService.Currency += sell;

                toastNotificationController.SummonToast($"+{sell} credits.");
            }
        }
    }
}
