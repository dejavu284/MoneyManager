using MoneyManager.Models;
using MoneyManager.Data.Repositories.Concrete;
using System.ComponentModel;
using System.Windows.Input;
using System.Threading.Tasks;
using System;

namespace MoneyManager.ViewModels
{
    public class AddAccountViewModel : INotifyPropertyChanged
    {
        private readonly AccountRepository _accountRepository;
        private readonly CurrencyRepository _currencyRepository;
        private Account _newAccount;
        private string _currencyCode;

        public Account NewAccount
        {
            get => _newAccount;
            set
            {
                _newAccount = value;
                OnPropertyChanged(nameof(NewAccount));
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

        public ICommand AddAccountCommand { get; }

        public AddAccountViewModel(AccountRepository accountRepository, CurrencyRepository currencyRepository)
        {
            _accountRepository = accountRepository;
            _currencyRepository = currencyRepository;
            _newAccount = new Account();

            AddAccountCommand = new RelayCommand(async _ => await AddAccount());
        }

        private async Task AddAccount()
        {
            var currency = await _currencyRepository.GetByCodeAsync(CurrencyCode);
            if (currency != null)
            {
                NewAccount.CurrencyId = currency.CurrencyId;
                await _accountRepository.AddAsync(NewAccount);
                AccountAdded?.Invoke(this, NewAccount);
            }
            else
            {
                // Обработка случая, когда валюта с указанным кодом не найдена
                // Например, можно показать сообщение пользователю
            }
        }

        public event EventHandler<Account> AccountAdded;
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
