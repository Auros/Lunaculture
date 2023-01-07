using Lunaculture.Items;
using System;
using UnityEngine;

namespace Lunaculture.Player.Inventory
{
    public class InventoryService : MonoBehaviour
    {
        public event Action? InventoryUpdatedEvent;

        public ItemStack?[] Inventory { get; private set; } = Array.Empty<ItemStack?>();

        public Item? SelectedItem { get; private set; }

        [SerializeField] private int numberOfSlots;

        private void OnEnable()
        {
            Inventory = new ItemStack[numberOfSlots];
        }

        public void AddItem(Item? item, int count = 1)
        {
            for (var i = 0; i < Inventory.Length; i++)
            {
                if (Inventory[i] != null && Inventory[i]!.ItemType == item)
                {
                    Inventory[i]!.Count += count;
                    InventoryUpdatedEvent?.Invoke();
                    return;
                }
            }

            // No item stack currently exists in the inventory. Time to add a new stack.
            for (var i = 0; i < Inventory.Length; i++)
            {
                if (Inventory[i] == null || Inventory[i]!.Empty)
                {
                    Inventory[i] = new ItemStack(item, count);
                    InventoryUpdatedEvent?.Invoke();
                    return;
                }
            }

            throw new InvalidOperationException("Inventory is full.");
        }

        public bool TryRemoveItem(Item item)
        {
            for (var i = 0; i < Inventory.Length; i++)
            {
                if (Inventory[i] != null && Inventory[i]!.ItemType == item && Inventory[i]!.Count > 0)
                {
                    Inventory[i]!.Count--;
                    InventoryUpdatedEvent?.Invoke();
                    return true;
                }
            }

            return false;
        }

        public void RemoveItem(Item item)
        {
            for (var i = 0; i < Inventory.Length; i++)
            {
                if (Inventory[i] != null && Inventory[i]!.ItemType == item && Inventory[i]!.Count > 0)
                {
                    Inventory[i]!.Count--;
                    InventoryUpdatedEvent?.Invoke();
                    return;
                }
            }

            throw new InvalidOperationException("No item stack for given item found.");
        }

        public void SwapItemStack(ItemStack target, int newIndex)
        {
            for (var i = 0; i < Inventory.Length; i++)
            {
                if (Inventory[i] == target)
                {
                    // If the target item stack is empty, simply move the stack over
                    if (Inventory[newIndex] == null || Inventory[newIndex]!.Empty)
                    {
                        Inventory[newIndex] = Inventory[i];
                        Inventory[i] = new ItemStack(null, 0);
                    }
                    // If both stacks are of the same item type, simply combine the stacks
                    else if (Inventory[i]!.ItemType == Inventory[newIndex]!.ItemType)
                    {
                        Inventory[newIndex]!.Count += Inventory[i]!.Count;
                        Inventory[i] = new ItemStack(null, 0);
                    }
                    // Simply swap items around
                    else
                    {
                        var temp = Inventory[newIndex];
                        Inventory[newIndex] = target;
                        Inventory[i] = temp;
                    }

                    InventoryUpdatedEvent?.Invoke();
                    return;
                }
            }

            throw new InvalidOperationException("Target item stack is not present in the inventory.");
        }

        public void SelectItem(Item? item)
        {
            SelectedItem = item;
            InventoryUpdatedEvent?.Invoke();
        }

        public void SelectItem(ItemStack? target)
        {
            if (target != null && !target.Empty)
            {
                SelectItem(target.ItemType);
                return;
            }

            SelectItem((Item?)null);
        }
    }
}
