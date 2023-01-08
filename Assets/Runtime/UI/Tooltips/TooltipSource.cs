using UnityEngine;
using UnityEngine.EventSystems;

namespace Lunaculture.UI.Tooltips
{
    public class TooltipSource : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [field: SerializeField]
        public string TooltipName { get; set; } = string.Empty;

        [field: SerializeField]
        public string TooltipText { get; set; } = string.Empty;

        public void OnPointerEnter(PointerEventData eventData)
            => SendMessageUpwards("OnTooltipEnter", this, SendMessageOptions.DontRequireReceiver);

        public void OnPointerExit(PointerEventData eventData)
            => SendMessageUpwards("OnTooltipExit", this, SendMessageOptions.DontRequireReceiver);

        private void OnDisable()
            => SendMessageUpwards("OnTooltipExit", this, SendMessageOptions.DontRequireReceiver);
    }
}
