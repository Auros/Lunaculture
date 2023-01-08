using Lunaculture.Player.Currency;
using Lunaculture.Player.Inventory;
using Lunaculture.UI.Inventory;
using Lunaculture.UI;
using UnityEngine;

namespace Lunaculture
{
    public class ShopBuyController : MonoBehaviour
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
            var item = cell.AssignedStack.ItemType;

            if (item!.CanBuy)
            {
                var price = cell.AssignedStack.ItemType!.BuyPrice;

                if (currencyService.Currency >= price)
                {
                    inventoryService.AddItem(item);

                    currencyService.Currency -= price;
                }
                else
                {
                    toastNotificationController.SummonToast($"You need {price}cr to buy this.", ToastNotificationController.ToastType.Fail);
                }
            }
            else
            {
                toastNotificationController.SummonToast("Cannot buy this item.", ToastNotificationController.ToastType.Fail);
            }
        }
    }
}
