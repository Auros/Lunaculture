using System;
using UnityEngine;

namespace Lunaculture.Items
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Lunaculture/Item")]
    public class Item : ScriptableObject
    {
        [field: Header("Basic Item Properties")]
        [field: SerializeField]
        public Sprite Icon { get; private set; } = null!;

        [field: SerializeField]
        public string Name { get; private set; } = string.Empty;
        
        [field: SerializeField] 
        public string Tooltip { get; private set; } = null!;

        [field: Header("Money!")]
        [field: SerializeField]
        public bool CanBuy { get; private set; }
        
        [field: SerializeField]
        public int BuyPrice { get; private set; }
        
        [field: SerializeField]
        public bool CanSell { get; private set; }
        
        [field: SerializeField]
        public int SellPrice { get; private set; }

        [field: SerializeField]
        public bool CanPlace { get; private set; }

        [field: SerializeField]
        public GameObject PlacePrefab { get; private set; } = null!;

        [field: SerializeField]
        public string[] Tags { get; private set; } = Array.Empty<string>();
        
        [field: SerializeField]
        public bool DoNotGrantAutomatically { get; set; }
    }
}
