using Lunaculture.Items;
using System;
using UnityEngine;

namespace Lunaculture
{
    // TODO: Find a way to serialize/deserialize ItemType without saving the whole ScriptableObject
    [Serializable]
    public class ItemStack
    {
        [field: SerializeField]
        public Item ItemType { get; private set; }

        [field: SerializeField]
        public int Count { get; set; }

        public bool Empty => ItemType == null || Count == 0;

        public ItemStack(Item item, int count = 0)
        {
            ItemType = item;
            Count = count;
        }
    }
}
