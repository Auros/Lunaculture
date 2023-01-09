using Cysharp.Threading.Tasks;
using Lunaculture.UI;
using System;
using UnityEngine;

namespace Lunaculture.UI.Tutorial
{
    public class TutorialToastController : MonoBehaviour
    {
        [SerializeField] private ToastNotificationController _toastNotificationController = null!;
        [SerializeField] private Sprite _tutorialSprite = null!;
        [SerializeField] private float _initialDelay = 5f;
        [SerializeField] private float _delayBetweenToasts = 10f;
        [SerializeField] private float _toastLifetime = 5f;

        private void Start()
            => Tutorial().AttachExternalCancellation(this.GetCancellationTokenOnDestroy()).Forget();

        private async UniTask Tutorial()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_initialDelay), true);

            _toastNotificationController.SummonToast("Press E to open your inventory.", _tutorialSprite, _toastLifetime);

            await UniTask.Delay(TimeSpan.FromSeconds(_delayBetweenToasts), true);

            _toastNotificationController.SummonToast("Use the Hoe to create plots for farming.", _tutorialSprite, _toastLifetime);

            await UniTask.Delay(TimeSpan.FromSeconds(_delayBetweenToasts), true);

            _toastNotificationController.SummonToast("Plots can only be prepared inside domes.", _tutorialSprite, _toastLifetime);

            await UniTask.Delay(TimeSpan.FromSeconds(_delayBetweenToasts), true);

            _toastNotificationController.SummonToast("Plant seeds on plots to grow crops.", _tutorialSprite, _toastLifetime);

            await UniTask.Delay(TimeSpan.FromSeconds(_delayBetweenToasts), true);

            _toastNotificationController.SummonToast("Don't forget to water your crops!", _tutorialSprite, _toastLifetime);

            await UniTask.Delay(TimeSpan.FromSeconds(_delayBetweenToasts), true);

            _toastNotificationController.SummonToast("Once crops are grown, collect them with the Harvester.", _tutorialSprite, _toastLifetime);

            await UniTask.Delay(TimeSpan.FromSeconds(_delayBetweenToasts), true);

            _toastNotificationController.SummonToast("Press R to open the shop.", _tutorialSprite, _toastLifetime);

            await UniTask.Delay(TimeSpan.FromSeconds(_delayBetweenToasts), true);

            _toastNotificationController.SummonToast("Sell your crops to meet the weekly quotas.", _tutorialSprite, _toastLifetime);

            await UniTask.Delay(TimeSpan.FromSeconds(_delayBetweenToasts), true);

            _toastNotificationController.SummonToast("Buy items from the shop to grow your farm.", _tutorialSprite, _toastLifetime);
        }
    }
}
