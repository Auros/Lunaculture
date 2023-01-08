using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lunaculture.Items
{
    [Serializable]
    public class WeekItemUnlockInfo
    {
        public int WeekNumber = 0;

        public List<Item> Items = new();
        
        public List<Sprite> ToastNotificationSprites = new();
    }
}