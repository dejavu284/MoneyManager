using MoneyManager.Models;
using MoneyManager.Data.Repositories.Concrete;
using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Threading.Tasks;

namespace MoneyManager.ViewModels
{
    public class AddCurrencyViewModel : INotifyPropertyChanged
    {
        private readonly CurrencyRepository _currencyRepository;
        private Currency _newCurrency;

        public Currency NewCurrency
        {
            get => _newCurrency;
            set
            {
                _newCurrency = value;
                OnPropertyChanged(nameof(NewCurrency));
            }
        }

        public ICommand AddCurrencyCommand { get; }

        public event EventHandler<Currency> CurrencyAdded;

        public AddCurrencyViewModel(CurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
            NewCurrency = new Currency();
            AddCurrencyCommand = new RelayCommand(async _ => await AddCurrency());
        }

        private async Task AddCurrency()
        {
            NewCurrency.Status = true;
            await _currencyRepository.AddAsync(NewCurrency);
            CurrencyAdded?.Invoke(this, NewCurrency);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
