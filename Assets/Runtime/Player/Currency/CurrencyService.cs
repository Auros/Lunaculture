using System;
using UnityEngine;

namespace Lunaculture.Player.Currency
{
    public class CurrencyService : MonoBehaviour
    {
        public event Action<CurrencyUpdateEvent>? OnCurrencyUpdate;

        public int Currency
        {
            get => currency;
            set
            {
                var deltaCurrency = value - currency;

                currency = value;

                OnCurrencyUpdate?.Invoke(new CurrencyUpdateEvent(currency, deltaCurrency));
            }
        }

        private int currency;
    }
}
