using System;
using UnityEngine;

namespace Lunaculture.Player.Inventory
{
    public class DebugInventoryProvider : MonoBehaviour
    {
        [SerializeField] private ItemStack[] startingInventory = Array.Empty<ItemStack>();
        [SerializeField] private InventoryService inventoryService = null!;

        private void Awake()
        {
            for (var i = 0; i < inventoryService.Inventory.Length; i++)
            {
                if (startingInventory.Length <= i) return;
                var item = startingInventory[i];
                inventoryService.AddItem(item.ItemType, item.Count);
            }
        }
    }
}
