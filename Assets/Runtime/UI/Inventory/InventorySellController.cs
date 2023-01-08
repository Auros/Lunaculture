using Lunaculture.Player.Currency;
using Lunaculture.Player.Inventory;
using UnityEngine;

namespace Lunaculture.UI.Inventory
{
    public class InventorySellController : MonoBehaviour
    {
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
            if (cell.AssignedStack != null && !cell.AssignedStack.Empty && cell.AssignedStack.ItemType.CanSell)
            {
                inventoryService.RemoveItem(cell.AssignedStack.ItemType);
                currencyService.Currency += cell.AssignedStack.ItemType.SellPrice;
            }
        }
    }
}