using System.Collections.Generic;

using UnityEngine;

namespace Lunaculture.Items
{
    public class ItemReferenceProvider : MonoBehaviour
    {
        [SerializeField]
        public List<Item> ShopItems = new();
            
        [SerializeField]
        public List<Item> Foods = null!;
        
        [SerializeField]
        public List<Item> Seeds = null!;

        [SerializeField]
        public List<WeekItemUnlockInfo> WeekItemUnlockInfos = null!;
    }
}