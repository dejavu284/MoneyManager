using MoneyManager.Models;
using MoneyManager.Data.Repositories.Concrete;
using System.ComponentModel;
using System.Windows.Input;
using System.Threading.Tasks;
using System;

namespace MoneyManager.ViewModels
{
    public class EditAccountViewModel : INotifyPropertyChanged
    {
        private readonly AccountRepository _accountRepository;
        private readonly CurrencyRepository _currencyRepository;
        private Account _account;
        private string _currencyCode;

        public Account Account
        {
            get => _account;
            set
            {
                _account = value;
                OnPropertyChanged(nameof(Account));
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

        public ICommand UpdateAccountCommand { get; }

        public EditAccountViewModel(AccountRepository accountRepository, CurrencyRepository currencyRepository, Account account)
        {
            _accountRepository = accountRepository;
            _currencyRepository = currencyRepository;
            _account = account;
            _currencyCode = account.Currency.CurrencyCode; // Инициализация текущим кодом валюты

            UpdateAccountCommand = new RelayCommand(async _ => await UpdateAccount());
        }

        private async Task UpdateAccount()
        {
            var currency = await _currencyRepository.GetByCodeAsync(CurrencyCode);
            if (currency != null)
            {
                Account.CurrencyId = currency.CurrencyId;
                await _accountRepository.UpdateAsync(Account);

                // Перезагружаем объект Account, чтобы обновить привязанные данные о валюте
                Account.Currency = currency;
                OnPropertyChanged(nameof(Account.Currency));

                AccountUpdated?.Invoke(this, Account);
            }
            else
            {
                // Обработка случая, когда валюта с указанным кодом не найдена
                // Например, можно показать сообщение пользователю
            }
        }

        public event EventHandler<Account> AccountUpdated;
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
