using MoneyManager.Models;
using MoneyManager.Data.Repositories.Concrete;
using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Threading.Tasks;

namespace MoneyManager.ViewModels
{
    public class EditCurrencyViewModel : INotifyPropertyChanged
    {
        private readonly CurrencyRepository _currencyRepository;
        private Currency _editedCurrency;

        public Currency EditedCurrency
        {
            get => _editedCurrency;
            set
            {
                _editedCurrency = value;
                OnPropertyChanged(nameof(EditedCurrency));
            }
        }

        public ICommand EditCurrencyCommand { get; }

        public event EventHandler<Currency> CurrencyUpdated;

        public EditCurrencyViewModel(CurrencyRepository currencyRepository, Currency currency)
        {
            _currencyRepository = currencyRepository;
            EditedCurrency = currency;
            EditCurrencyCommand = new RelayCommand(async _ => await EditCurrency());
        }

        private async Task EditCurrency()
        {
            await _currencyRepository.UpdateAsync(EditedCurrency);
            CurrencyUpdated?.Invoke(this, EditedCurrency);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
