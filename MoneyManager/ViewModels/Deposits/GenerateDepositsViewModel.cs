using MoneyManager.Models;
using MoneyManager.Data.Repositories.Concrete;
using System.ComponentModel;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System;
using MoneyManager.Models.Generators;
using System.Linq;
using System.Collections.Generic;
using System.Windows;

namespace MoneyManager.ViewModels.Deposits
{
    public class GenerateDepositsViewModel : INotifyPropertyChanged
    {
        private int _numberOfDeposits;
        private decimal _minAmount;
        private decimal _maxAmount;
        private decimal _minInterestRate;
        private decimal _maxInterestRate;
        private DateTime _startDate;
    
        public int NumberOfDeposits
        {
            get => _numberOfDeposits;
            set
            {
                _numberOfDeposits = value;
                OnPropertyChanged(nameof(NumberOfDeposits));
            }
        }

        public decimal MinAmount
        {
            get => _minAmount;
            set
            {
                _minAmount = value;
                OnPropertyChanged(nameof(MinAmount));
            }
        }

        public decimal MaxAmount
        {
            get => _maxAmount;
            set
            {
                _maxAmount = value;
                OnPropertyChanged(nameof(MaxAmount));
            }
        }

        public decimal MinInterestRate
        {
            get => _minInterestRate;
            set
            {
                _minInterestRate = value;
                OnPropertyChanged(nameof(MinInterestRate));
            }
        }

        public decimal MaxInterestRate
        {
            get => _maxInterestRate;
            set
            {
                _maxInterestRate = value;
                OnPropertyChanged(nameof(MaxInterestRate));
            }
        }

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }

        public ICommand GenerateDepositsCommand { get; }

        private readonly DepositRepository _depositRepository;
        private readonly CurrencyRepository _currencyRepository;
        private readonly DepositViewModel _parentViewModel;

        public GenerateDepositsViewModel(DepositRepository depositRepository, CurrencyRepository currencyRepository, DepositViewModel parentViewModel)
        {
            _depositRepository = depositRepository;
            _currencyRepository = currencyRepository;
            _parentViewModel = parentViewModel;

            // Initialize properties
            _startDate = DateTime.Today;

            GenerateDepositsCommand = new RelayCommand(async _ => await GenerateDeposits());
        }

            private async Task GenerateDeposits()
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
                    for (int i = 0; i < NumberOfDeposits; i++)
                    {
                        var amount = Math.Round((decimal)(random.NextDouble() * ((double)MaxAmount - (double)MinAmount) + (double)MinAmount), 2);
                        var interestRate = Math.Round((decimal)(random.NextDouble() * ((double)MaxInterestRate - (double)MinInterestRate) + (double)MinInterestRate), 2);
                        var startDate = StartDate;
                        var endDate = startDate.AddDays(random.Next(90, 360));
                        var currency = currencies[random.Next(currencies.Count)];

                        var deposit = new DepositBuilder()
                            .SetName($"Вклад {i + 1}")
                            .SetAmount(amount)
                            .SetInterestRate(interestRate)
                            .SetStartDate(startDate)
                            .SetEndDate(endDate)
                            .SetCurrency(currency)
                            .Build();

                        await _depositRepository.AddAsync(deposit);

                        Application.Current?.Dispatcher.Invoke(() =>
                        {
                            _parentViewModel.Deposits.Add(deposit);
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
