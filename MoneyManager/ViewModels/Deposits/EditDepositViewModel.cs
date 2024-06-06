using MoneyManager.Models;
using MoneyManager.Data.Repositories.Concrete;
using System.ComponentModel;
using System.Windows.Input;
using System.Threading.Tasks;
using System;

namespace MoneyManager.ViewModels.Deposits
{
    public class EditDepositViewModel : INotifyPropertyChanged
    {
        private readonly DepositRepository _depositRepository;
        private readonly CurrencyRepository _currencyRepository;
        private Deposit _deposit;
        private string _currencyCode;

        public Deposit Deposit
        {
            get => _deposit;
            set
            {
                _deposit = value;
                OnPropertyChanged(nameof(Deposit));
            }
        }

        public string CurrencyCode
        {
            get => _currencyCode;
            set
            {
                _currencyCode = value;
                OnPropertyChanged(nameof(CurrencyCode));
            }
        }

        public ICommand UpdateDepositCommand { get; }

        public EditDepositViewModel(DepositRepository depositRepository, CurrencyRepository currencyRepository, Deposit deposit)
        {
            _depositRepository = depositRepository;
            _currencyRepository = currencyRepository;
            _deposit = deposit;
            _currencyCode = deposit.Currency.CurrencyCode;

            UpdateDepositCommand = new RelayCommand(async _ => await UpdateDeposit());
        }

        private async Task UpdateDeposit()
        {
            var currency = await _currencyRepository.GetByCodeAsync(CurrencyCode);
            if (currency != null)
            {
                Deposit.CurrencyId = currency.CurrencyId;
                await _depositRepository.UpdateAsync(Deposit);
                DepositUpdated?.Invoke(this, Deposit);
            }
            else
            {
                // Обработка случая, когда валюта с указанным кодом не найдена
                // Например, можно показать сообщение пользователю
            }
        }

        public event EventHandler<Deposit> DepositUpdated;
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
