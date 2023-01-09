using Cysharp.Threading.Tasks;
using Lunaculture.UI;
using System;
using UnityEngine;

namespace Lunaculture
{
    public class TutorialToastController : MonoBehaviour
    {
        [SerializeField] private ToastNotificationController _toastNotificationController = null!;
        [SerializeField] private Sprite _tutorialSprite = null!;
        [SerializeField] private float _initialDelay = 5f;
        [SerializeField] private float _delayBetweenToasts = 10f;
        [SerializeField] private float _toastLifetime = 5f;

        private async UniTask Start()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_initialDelay), true);

            _toastNotificationController.SummonToast("Press E to open your inventory.", _tutorialSprite, _toastLifetime);

            await UniTask.Delay(TimeSpan.FromSeconds(_delayBetweenToasts), true);

            _toastNotificationController.SummonToast("Use the Hoe to prepare land for farming.", _tutorialSprite, _toastLifetime);

            await UniTask.Delay(TimeSpan.FromSeconds(_delayBetweenToasts), true);

            _toastNotificationController.SummonToast("Plots can only be prepared inside domes.", _tutorialSprite, _toastLifetime);

            await UniTask.Delay(TimeSpan.FromSeconds(_delayBetweenToasts), true);

            _toastNotificationController.SummonToast("Plant seeds on prepared plots to grow crops.", _tutorialSprite, _toastLifetime);

            await UniTask.Delay(TimeSpan.FromSeconds(_delayBetweenToasts), true);

            _toastNotificationController.SummonToast("Don't forget to water your crops!", _tutorialSprite, _toastLifetime);

            await UniTask.Delay(TimeSpan.FromSeconds(_delayBetweenToasts), true);

            _toastNotificationController.SummonToast("Once crops are grown, use the Harvester to collect your yield.", _tutorialSprite, _toastLifetime);

            await UniTask.Delay(TimeSpan.FromSeconds(_delayBetweenToasts), true);

            _toastNotificationController.SummonToast("Sell your crops to meet the weekly quotas.", _tutorialSprite, _toastLifetime);
        }
    }
}
