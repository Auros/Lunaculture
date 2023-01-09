using Lunaculture.Player.Inventory;
using System;
using TMPro;
using UnityEngine;

namespace Lunaculture.UI.Inventory
{
    public class SelectedItemController : MonoBehaviour
    {
        [SerializeField] private GameUIInterconnect gameUIInterconnect = null!;
        [SerializeField] private TextMeshProUGUI selectedItemText = null!;

        private InventoryService inventoryService = null!;

        private void Start()
        {
            inventoryService = gameUIInterconnect.InventoryService;

            inventoryService.InventoryUpdatedEvent += InventoryService_InventoryUpdatedEvent;
        }

        private void InventoryService_InventoryUpdatedEvent()
        {
            // lmao this sucks but oh well
            var selectedItem = inventoryService.SelectedItem;

            if (selectedItem != null)
            {
                var count = 1;
                for (var i = 0; i < inventoryService.Inventory.Length; i++)
                {
                    if (inventoryService.Inventory[i] != null && inventoryService.Inventory[i].ItemType == selectedItem)
                    {
                        count = inventoryService.Inventory[i].Count;
                    }
                }

                selectedItemText.text = $"Selected: {selectedItem.Name}";

                if (Array.IndexOf(selectedItem.Tags, "Infinite") != -1)
                    selectedItemText.text += $" x{count}";
            }
            else
            {
                selectedItemText.text = "Selected: None";
            }
        }

        private void OnDestroy()
        {
            inventoryService.InventoryUpdatedEvent -= InventoryService_InventoryUpdatedEvent;
        }
    }
}
