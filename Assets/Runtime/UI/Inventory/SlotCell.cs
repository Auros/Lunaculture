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

        [SerializeField] private Image itemIcon = null!;
        [SerializeField] private Image selectedIcon = null!;
        [SerializeField] private TextMeshProUGUI countLabel = null!;
        [SerializeField] private TooltipSource tooltipSource = null!;

        public void AssignItemStack(ItemStack? stack = null)
        {
            AssignedStack = stack;

            if (stack == null || stack.Empty)
            {
                itemIcon.gameObject.SetActive(false);
                countLabel.gameObject.SetActive(false);
                tooltipSource.enabled = false;
                return;
            }

            itemIcon.gameObject.SetActive(true);
            countLabel.gameObject.SetActive(true);
            tooltipSource.enabled = true;

            itemIcon.sprite = stack.ItemType!.Icon;

            tooltipSource.TooltipName = stack.ItemType!.Name;
            tooltipSource.TooltipText = stack.ItemType!.Tooltip;

            countLabel.text = LabelText;
        }

        protected virtual string LabelText => AssignedStack?.Count <= 99
            ? AssignedStack.Count.ToString()
            : "99+";

        public void ToggleSelectionIcon(bool isSelected)
            => selectedIcon.enabled = isSelected;

        public void OnPointerClick(PointerEventData eventData)
        {
            SendMessageUpwards("OnSlotCellClicked", this, SendMessageOptions.DontRequireReceiver);
        }
    }
}
