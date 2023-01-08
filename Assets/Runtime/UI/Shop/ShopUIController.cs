using Lunaculture.Items;
using UnityEngine;

namespace Lunaculture.UI.Shop
{
    public class ShopUIController : MonoBehaviour
    {
        [SerializeField] private GameUIInterconnect gameUIInterconnect = null!;
        [SerializeField] private ShopSlotCell shopCellPrefab = null!;
        [SerializeField] private Item[] initialStoreItems = null!;

        private void Start()
        {
            for (var i = 0; i < initialStoreItems.Length; i++)
            {
                AddShopItem(initialStoreItems[i]);
            }
        }

        public void AddShopItem(Item item)
        {
            var shopCell = Instantiate(shopCellPrefab, transform);
            shopCell.AssignItemStack(new ItemStack(item, 1));
        }
    }
}
