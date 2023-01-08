using Lunaculture.Items;
using Lunaculture.GameTime;
using Lunaculture.UI;
using System.Collections.Generic;
using Lunaculture.Objectives;
using Lunaculture.Player.Inventory;
using UnityEngine;
using Lunaculture.UI.Shop;

namespace Lunaculture.Game
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] 
        private ToastNotificationController _toastNotificationController = null!;
        
        [SerializeField] 
        private TimeController _timeController = null!;
        
        [SerializeField] 
        private InventoryService _inventoryService = null!;
        
        [SerializeField] 
        private ObjectiveService _objectiveService = null!;

        [SerializeField]
        private ItemReferenceProvider _itemReferenceProvider = null!;

        [SerializeField]
        private ShopUIController _shopUIController = null!;
            
        [SerializeField] 
        private Sprite _iridiumSprite = null!;
        
        [SerializeField] 
        private int _seedsGrantedUponUnlock = 10;
        
        private int _weekNumber = 0;
        
        public void Start()
        {
            // give initial prompts. new prompts show up on the bottom
            UnlockWeek(_itemReferenceProvider.WeekItemUnlockInfos[0]);
            
            _toastNotificationController.SummonToast("You are on the desolate moon Iridium. Your goal is to sustain the colony by meeting food quotas.", _iridiumSprite, 15f);
            
            _objectiveService.OnObjectiveComplete += ObjectiveServiceOnOnObjectiveComplete;
        }

        private void ObjectiveServiceOnOnObjectiveComplete()
        {
            _weekNumber++;

            if (_itemReferenceProvider.WeekItemUnlockInfos.Count >= _weekNumber)
            {
                UnlockWeek(_itemReferenceProvider.WeekItemUnlockInfos[_weekNumber]);
            }
        }

        public void UnlockWeek(WeekItemUnlockInfo weekItemUnlockInfo)
        {
            for (int i = 0; i < weekItemUnlockInfo.Items.Count; i++)
            {
                var item = weekItemUnlockInfo.Items[i];
                var icon = weekItemUnlockInfo.ToastNotificationSprites[i];

                _toastNotificationController.SummonToast($"Received {_seedsGrantedUponUnlock} {item.Name}. {item.Tooltip}", icon, 15f);
                _inventoryService.AddItem(item, _seedsGrantedUponUnlock);
                _shopUIController.AddShopItem(item);
                if (item.CanBuy && !_itemReferenceProvider.ShopItems.Contains(item)) _itemReferenceProvider.ShopItems.Add(item);
            }
        }
    }
}