using System;
using Lunaculture.UI.Tooltips;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Lunaculture.UI.Inventory
{
    public class SlotCell : MonoBehaviour, IPointerClickHandler
    {
        public ItemStack? AssignedStack { get; private set; }

        public float Transparency
        {
            get => transparency;
            set
            {
                transparency = value;
                itemIcon.color = itemIcon.color.WithAlpha(value);
                selectedIcon.color = selectedIcon.color.WithAlpha(value);
                countLabel.color = countLabel.color.WithAlpha(value);
            }
        }

        [SerializeField] private Image itemIcon = null!;
        [SerializeField] private Image selectedIcon = null!;
        [SerializeField] private TextMeshProUGUI countLabel = null!;
        [SerializeField] private TooltipSource tooltipSource = null!;

        private float transparency = 1f;

        public void AssignItemStack(ItemStack? stack = null)
        {
            AssignedStack = stack;

            if (stack == null || stack.Empty)
            {
                itemIcon.gameObject.SetActive(false);
                countLabel.gameObject.SetActive(false);
                tooltipSource.enabled = false;
                Transparency = 1f;
                return;
            }

            itemIcon.gameObject.SetActive(true);
            countLabel.gameObject.SetActive(true);
            tooltipSource.enabled = true;

            itemIcon.sprite = stack.ItemType!.Icon;

            tooltipSource.TooltipName = stack.ItemType!.Name;
            tooltipSource.TooltipText = stack.ItemType!.Tooltip;

            if (stack.ItemType!.CanSell)
            {
                tooltipSource.TooltipText += $"\nSells for {stack.ItemType!.SellPrice}cr";
            }

            countLabel.text = LabelText;
            SendMessageUpwards("OnSlotCellAssigned", this, SendMessageOptions.DontRequireReceiver);
        }

        protected virtual string LabelText
        {
            get
            {
                if (AssignedStack is null)
                    return "?";

                var item = AssignedStack.ItemType!;
                if (Array.IndexOf(item.Tags, "Infinite") != -1)
                    return string.Empty;
                
                return AssignedStack?.Count <= 999
                    ? AssignedStack.Count.ToString()
                    : "999+";
            }
        }

        public void ToggleSelectionIcon(bool isSelected)
            => selectedIcon.enabled = isSelected;

        public void OnPointerClick(PointerEventData eventData)
        {
            SendMessageUpwards("OnSlotCellClicked", this, SendMessageOptions.DontRequireReceiver);
        }
    }
}
