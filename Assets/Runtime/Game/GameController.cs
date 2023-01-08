using Lunaculture.Items;
using Lunaculture.GameTime;
using Lunaculture.UI;
using System.Collections.Generic;
using Lunaculture.Player.Inventory;
using UnityEngine;

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
        private ItemReferenceProvider _itemReferenceProvider = null!;
            
        [SerializeField] 
        private Sprite _iridiumSprite = null!;
        
        [SerializeField] 
        private int _seedsGrantedUponUnlock = 10;

        public void Start()
        {
            // give initial prompts. new prompts show up on the bottom
            UnlockWeek(_itemReferenceProvider.WeekItemUnlockInfos[0]);
            
            _toastNotificationController.SummonToast("You are on the desolate moon Iridium. Your goal is to sustain the colony by meeting food quotas.", _iridiumSprite, 15f);
            
            _timeController.OnWeekChange += TimeControllerOnOnWeekChange;
        }

        public void UnlockWeek(WeekItemUnlockInfo weekItemUnlockInfo)
        {
            for (int i = 0; i < weekItemUnlockInfo.Items.Count; i++)
            {
                var item = weekItemUnlockInfo.Items[i];
                var icon = weekItemUnlockInfo.ToastNotificationSprites[i];
                
                _toastNotificationController.SummonToast($"Received {_seedsGrantedUponUnlock} {item.Name}. {item.Tooltip}", icon, 15f);
                _inventoryService.AddItem(item, _seedsGrantedUponUnlock);
            }
        }

        private void TimeControllerOnOnWeekChange(WeekChangeEvent weekChangeEvent)
        {
            Debug.Log(weekChangeEvent.Week);
        }
    }
}