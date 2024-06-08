using System;

namespace MoneyManager.Models.Generators
{
    public class DepositBuilder
    {
        private readonly Deposit _deposit;

        public DepositBuilder()
        {
            _deposit = new Deposit();
        }

        public DepositBuilder SetName(string name)
        {
            _deposit.DepositName = name;
            return this;
        }

        public DepositBuilder SetAmount(decimal amount)
        {
            _deposit.DepositAmount = amount;
            return this;
        }

        public DepositBuilder SetInterestRate(decimal interestRate)
        {
            _deposit.InterestRate = interestRate;
            return this;
        }

        public DepositBuilder SetStartDate(DateTime startDate)
        {
            _deposit.StartDate = startDate;
            return this;
        }

        public DepositBuilder SetEndDate(DateTime endDate)
        {
            _deposit.EndDate = endDate;
            return this;
        }

        public DepositBuilder SetCurrency(Currency currency)
        {
            _deposit.Currency = currency;
            _deposit.CurrencyId = currency.CurrencyId;
            return this;
        }

        public Deposit Build()
        {
            _deposit.Status = true;
            return _deposit;
        }
    }
}
