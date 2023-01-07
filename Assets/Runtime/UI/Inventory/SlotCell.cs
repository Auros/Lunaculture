using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Lunaculture.UI.Inventory
{
    public class SlotCell : MonoBehaviour, IPointerClickHandler
    {
        public ItemStack AssignedStack { get; private set; }

        [SerializeField] private Image itemIcon;
        [SerializeField] private Image selectedIcon;
        [SerializeField] private TextMeshProUGUI countLabel;

        public void AssignItemStack(ItemStack stack = null)
        {
            AssignedStack = stack;

            if (stack == null || stack.ItemType == null || stack.Count == 0)
            {
                itemIcon.gameObject.SetActive(false);
                countLabel.gameObject.SetActive(false);
                return;
            }

            itemIcon.gameObject.SetActive(true);
            countLabel.gameObject.SetActive(true);

            itemIcon.sprite = stack.ItemType.Icon;

            if (stack.Count > 99)
            {
                countLabel.text = "99+";
            }
            else
            {
                countLabel.text = stack.Count.ToString();
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
