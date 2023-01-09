using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lunaculture
{
    public class ToastNotification : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI labelText = null!;
        [SerializeField] private Image image = null!;

        private bool assigned = false;

        public void AssignToastDetails(string details, Sprite sprite, float lifetime)
        {
            if (assigned)
            {
                throw new InvalidOperationException("This toast notification has already been assigned details.");
            }

            assigned = true;

            labelText.text = details;
            image.sprite = sprite;
            Lifetime(lifetime).AttachExternalCancellation(this.GetCancellationTokenOnDestroy()).Forget();
        }

        // TODO: Tween in/out of lifetime
        private async UniTask Lifetime(float lifetime)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(lifetime), true);

            Destroy(gameObject);
        }
    }
}
