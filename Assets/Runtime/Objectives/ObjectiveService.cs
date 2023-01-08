using Lunaculture.Player.Currency;
using System;
using UnityEngine;

namespace Lunaculture.Objectives
{
    public class ObjectiveService : MonoBehaviour
    {
        public event Action OnObjectiveComplete;
        public event Action<float> OnObjectiveProgress;

        [field: SerializeField]
        public int SellTarget { get; private set; } = 100;

        [SerializeField] private CurrencyService currencyService = null!;

        private int sellProgress = 0;

        private void Start()
        {
            currencyService.OnCurrencyUpdate += CurrencyService_OnCurrencyUpdate;
        }

        private void CurrencyService_OnCurrencyUpdate(CurrencyUpdateEvent obj)
        {
            if (obj.ChangeInCurrency > 0)
            {
                sellProgress += obj.ChangeInCurrency;

                OnObjectiveProgress?.Invoke((float)sellProgress / SellTarget);

                if (sellProgress >= SellTarget)
                {
                    SellTarget += 100;
                    sellProgress = 0;
                    OnObjectiveComplete?.Invoke();
                }
            }
        }

        private void OnDestroy()
        {
            currencyService.OnCurrencyUpdate -= CurrencyService_OnCurrencyUpdate;
        }
    }
}
