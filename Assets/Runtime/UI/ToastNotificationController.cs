using UnityEngine;

namespace Lunaculture.UI
{
    public class ToastNotificationController : MonoBehaviour
    {
        [SerializeField] private ToastNotification toastNotificationPrefab = null!;
        [SerializeField] private Sprite plusSprite = null!;
        [SerializeField] private Sprite completeSprite = null!;
        [SerializeField] private Sprite failSprite = null!;

        public void SummonToast(string details, ToastType toastType = ToastType.Complete, float lifetime = 2)
        {
            var sprite = toastType switch
            {
                ToastType.Plus => plusSprite,
                ToastType.Fail => failSprite,
                _ => completeSprite
            };

            var toast = Instantiate(toastNotificationPrefab, transform);
            toast.AssignToastDetails(details, sprite, lifetime);
        }
        
        public void SummonToast(string details, Sprite sprite, float lifetime = 2)
        {
            var toast = Instantiate(toastNotificationPrefab, transform);
            toast.AssignToastDetails(details, sprite, lifetime);
        }

        public enum ToastType { Complete, Plus, Fail }
    }
}
