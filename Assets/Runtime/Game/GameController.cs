using Lunaculture.Items;
using Lunaculture.GameTime;
using Lunaculture.UI;
using System.Collections.Generic;
using Lunaculture.Objectives;
using Lunaculture.Player.Inventory;
using UnityEngine;
using Lunaculture.UI.Shop;
using Lunaculture.UI.Dialog;

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
        private LoreBoxController _loreBoxController = null!;
            
        [SerializeField] 
        private Sprite _iridiumSprite = null!;
        
        [SerializeField] 
        private int _seedsGrantedUponUnlock = 10;
        
        private int _weekNumber = 0;
        
        public void Start()
        {
            _ = _timeController;
            // give initial prompts. new prompts show up on the bottom
            UnlockWeek(_itemReferenceProvider.WeekItemUnlockInfos[0]);
            
            _toastNotificationController.SummonToast("You are on the desolate moon Iridium. Your goal is to sustain the colony by meeting food quotas.", _iridiumSprite, 15f);
            
            _objectiveService.OnObjectiveComplete += ObjectiveServiceOnOnObjectiveComplete;
        }

        private void ObjectiveServiceOnOnObjectiveComplete()
        {
            _weekNumber++;

            if (_itemReferenceProvider.WeekItemUnlockInfos.Count > _weekNumber)
            {
                UnlockWeek(_itemReferenceProvider.WeekItemUnlockInfos[_weekNumber]);
            }
            else if (_weekNumber == _itemReferenceProvider.WeekItemUnlockInfos.Count)
            {
                _loreBoxController.DumpLore("Well done, DATT MAMON!!\n\n" +
                    "You have reached the end of content for Lunaculture.\n\n" + 
                    "Thanks for playing! We hope you enjoyed the game and would appreciate a rating. Feel free to continue completing weekly quotas!");
            }
        }

        public void UnlockWeek(WeekItemUnlockInfo weekItemUnlockInfo)
        {
            for (int i = 0; i < weekItemUnlockInfo.Items.Count; i++)
            {
                var item = weekItemUnlockInfo.Items[i];
                var icon = weekItemUnlockInfo.ToastNotificationSprites[i];

                _toastNotificationController.SummonToast($"+{_seedsGrantedUponUnlock} {item.Name}.", icon, 15f);
                if (!item.DoNotGrantAutomatically)
                    _inventoryService.AddItem(item, _seedsGrantedUponUnlock);
                _shopUIController.AddShopItem(item);
                if (item.CanBuy && !_itemReferenceProvider.ShopItems.Contains(item)) _itemReferenceProvider.ShopItems.Add(item);
            }
        }
    }
}