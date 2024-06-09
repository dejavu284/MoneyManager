using MoneyManager.Models;
using MoneyManager.Data.Repositories.Concrete;
using System.ComponentModel;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System;
using MoneyManager.Models.Generators;
using System.Linq;
using System.Windows;

namespace MoneyManager.ViewModels.Currencys
{
    public class GenerateCurrenciesViewModel : INotifyPropertyChanged
    {
        private int _numberOfCurrencies;

        public int NumberOfCurrencies
        {
            get => _numberOfCurrencies;
            set
            {
                _numberOfCurrencies = value;
                OnPropertyChanged(nameof(NumberOfCurrencies));
            }
        }

        public ICommand GenerateCurrenciesCommand { get; }

        private readonly CurrencyRepository _currencyRepository;
        private readonly CurrencyViewModel _parentViewModel;

        public GenerateCurrenciesViewModel(CurrencyRepository currencyRepository, CurrencyViewModel parentViewModel)
        {
            _currencyRepository = currencyRepository;
            _parentViewModel = parentViewModel;

            GenerateCurrenciesCommand = new RelayCommand(async _ => await GenerateCurrencies());
        }

        private async Task GenerateCurrencies()
        {
            var random = new Random();
            var currencyCodes = new[] { "USD", "EUR", "GBP", "JPY", "RUB", "CNY", "INR", "BRL", "ZAR", "KRW" };

            await Task.Run(async () =>
            {
                for (int i = 0; i < NumberOfCurrencies; i++)
                {
                    var code = currencyCodes[random.Next(currencyCodes.Length)] + " " + i;

                    var currency = new CurrencyBuilder()
                        .SetCode(code)
                        .Build();

                    await _currencyRepository.AddAsync(currency);

                    // Обновляем коллекцию валют в UI-потоке
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        _parentViewModel.Currencies.Add(currency);
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
