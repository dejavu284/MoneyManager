using MoneyManager.Models;

namespace MoneyManager.Models.Generators
{
    public class AccountBuilder
    {
        private readonly Account _account;

        public AccountBuilder()
        {
            _account = new Account();
        }

        public AccountBuilder SetName(string name)
        {
            _account.AccountName = name;
            return this;
        }

        public AccountBuilder SetBalance(decimal balance)
        {
            _account.AccountBalance = balance;
            return this;
        }

        public AccountBuilder SetCurrency(Currency currency)
        {
            _account.Currency = currency;
            _account.CurrencyId = currency.CurrencyId;
            return this;
        }

        public Account Build()
        {
            _account.Status = true;
            return _account;
        }
    }
}
