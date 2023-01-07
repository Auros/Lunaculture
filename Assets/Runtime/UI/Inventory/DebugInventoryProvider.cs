using UnityEngine;

namespace Lunaculture.Player.Inventory
{
    public class DebugInventoryProvider : MonoBehaviour
    {
        [SerializeField] private ItemStack[] startingInventory;
        [SerializeField] private InventoryService inventoryService;

        private void Start()
        {
            for (var i = 0; i < inventoryService.Inventory.Length; i++)
            {
                if (startingInventory.Length <= i) return;

                inventoryService.AddItem(startingInventory[i].ItemType, startingInventory[i].Count);
            }
        }
    }
}
