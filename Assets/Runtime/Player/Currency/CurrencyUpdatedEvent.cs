namespace Lunaculture.Player.Currency
{
    public struct CurrencyUpdateEvent
    {
        public int Currency { get; private set; }

        public int ChangeInCurrency { get; private set; }

        public CurrencyUpdateEvent(int currency, int changeInCurrency)
        {
            Currency = currency;
            ChangeInCurrency = changeInCurrency;
        }
    }
}
