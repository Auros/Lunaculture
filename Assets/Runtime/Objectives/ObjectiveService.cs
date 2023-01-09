using Lunaculture.GameTime;
using Lunaculture.Player.Currency;
using System;
using UnityEngine;

namespace Lunaculture.Objectives
{
    public class ObjectiveService : MonoBehaviour
    {
        public event Action? OnObjectiveComplete;
        public event Action? OnObjectiveFail;
        public event Action<float>? OnObjectiveProgress;

        [field: SerializeField]
        public int SellTarget { get; private set; } = 100;

        [Header("Sell target increases by this amount after every complete objective.")]
        [SerializeField]
        private int sellTargetIncrease = 100;

        [SerializeField] private CurrencyService currencyService = null!;
        [SerializeField] private TimeController timeController = null!;

        private int sellProgress = 0;

        private void Start()
        {
            currencyService.OnCurrencyUpdate += CurrencyService_OnCurrencyUpdate;
            timeController.OnWeekChange += TimeController_OnWeekChange;
        }

        private void CurrencyService_OnCurrencyUpdate(CurrencyUpdateEvent obj)
        {
            if (obj.ChangeInCurrency > 0)
            {
                sellProgress += obj.ChangeInCurrency;

                OnObjectiveProgress?.Invoke(Mathf.Clamp01((float)sellProgress / SellTarget));
            }
        }

        private void TimeController_OnWeekChange(WeekChangeEvent obj)
        {
            if (sellProgress >= SellTarget)
            {
                SellTarget += sellTargetIncrease;
                sellProgress = 0;
                OnObjectiveComplete?.Invoke();
            }
            else
            {
                OnObjectiveFail?.Invoke();
            }

            OnObjectiveProgress?.Invoke(0);
        }

        private void OnDestroy()
        {
            currencyService.OnCurrencyUpdate -= CurrencyService_OnCurrencyUpdate;
            timeController.OnWeekChange -= TimeController_OnWeekChange;
        }
    }
}
