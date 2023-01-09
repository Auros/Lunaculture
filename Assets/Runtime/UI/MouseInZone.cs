using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Lunaculture
{
    public class MouseInZone : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private UnityEvent<bool>? _onEnter;

        public void OnPointerEnter(PointerEventData eventData)
        {
            _onEnter?.Invoke(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _onEnter?.Invoke(false);
        }
    }
}