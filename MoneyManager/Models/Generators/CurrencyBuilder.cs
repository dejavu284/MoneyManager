using System;

namespace MoneyManager.Models.Generators
{
    public class CurrencyBuilder
    {
        private readonly Currency _currency;

        public CurrencyBuilder()
        {
            _currency = new Currency();
        }

        public CurrencyBuilder SetCode(string code)
        {
            _currency.CurrencyCode = code;
            return this;
        }

        public Currency Build()
        {
            _currency.Status = true;
            return _currency;
        }
    }
}
