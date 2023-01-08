using Lunaculture.Player.Currency;
using TMPro;
using UnityEngine;

namespace Lunaculture.UI.Currency
{
    public class CurrencyUIController : MonoBehaviour
    {
        [SerializeField] private GameUIInterconnect gameUIInterconnect = null!;
        [SerializeField] private TextMeshProUGUI moneyText = null!;

        private CurrencyService currencyService = null!;

        private void Start()
        {
            currencyService = gameUIInterconnect.CurrencyService;

            currencyService.OnCurrencyUpdate += CurrencyService_OnCurrencyUpdate;
        }

        private void CurrencyService_OnCurrencyUpdate(CurrencyUpdateEvent obj)
        {
            moneyText.text = $"{obj.Currency}cr";
        }

        private void OnDestroy()
        {
            currencyService.OnCurrencyUpdate -= CurrencyService_OnCurrencyUpdate;
        }
    }
}
