using MoneyManager.Models;
using MoneyManager.Data.Repositories.Concrete;
using System.ComponentModel;
using System.Windows.Input;
using System.Threading.Tasks;
using System;
using MoneyManager.Models.Generators;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows;

namespace MoneyManager.ViewModels.Accounts
{
    public class GenerateAccountsViewModel : INotifyPropertyChanged
    {
        private int _numberOfAccounts;
        private decimal _minBalance;
        private decimal _maxBalance;

        public int NumberOfAccounts
        {
            get => _numberOfAccounts;
            set
            {
                _numberOfAccounts = value;
                OnPropertyChanged(nameof(NumberOfAccounts));
            }
        }

        public decimal MinBalance
        {
            get => _minBalance;
            set
            {
                _minBalance = value;
                OnPropertyChanged(nameof(MinBalance));
            }
        }

        public decimal MaxBalance
        {
            get => _maxBalance;
            set
            {
                _maxBalance = value;
                OnPropertyChanged(nameof(MaxBalance));
            }
        }

        public ICommand GenerateAccountsCommand { get; }

        private readonly AccountRepository _accountRepository;
        private readonly CurrencyRepository _currencyRepository;
        private readonly AccountViewModel _parentViewModel;

        public GenerateAccountsViewModel(AccountRepository accountRepository, CurrencyRepository currencyRepository, AccountViewModel parentViewModel)
        {
            _accountRepository = accountRepository;
            _currencyRepository = currencyRepository;
            _parentViewModel = parentViewModel;

            GenerateAccountsCommand = new RelayCommand(async _ => await GenerateAccounts());
        }

        private async Task GenerateAccounts()
        {
            var random = new Random();
            var currencies = (await _currencyRepository.GetAllAsync()).ToList();

            if (currencies == null || currencies.Count == 0)
            {
                // Обработка случая, когда нет доступных валют
                return;
            }

            await Task.Run(async () =>
            {
                for (int i = 0; i < NumberOfAccounts; i++)
                {
                    var balance = Math.Round((decimal)(random.NextDouble() * ((double)MaxBalance - (double)MinBalance) + (double)MinBalance), 2);
                    var currency = currencies[random.Next(currencies.Count)];

                    var account = new AccountBuilder()
                        .SetName($"Счёт {i + 1}")
                        .SetBalance(balance)
                        .SetCurrency(currency)
                        .Build();

                    await _accountRepository.AddAsync(account);

                    // Обновляем коллекцию счетов в UI-потоке
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        _parentViewModel.Accounts.Add(account);
                    });
                }
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
