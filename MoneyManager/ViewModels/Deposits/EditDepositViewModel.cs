using MoneyManager.Models;
using MoneyManager.Data.Repositories.Concrete;
using System.ComponentModel;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;
using System;

namespace MoneyManager.ViewModels.Deposits
{
    public class EditDepositViewModel : INotifyPropertyChanged
    {
        private readonly DepositRepository _depositRepository;
        private readonly CurrencyRepository _currencyRepository;
        private Deposit _deposit;

        private ObservableCollection<Currency> _currencies;

        public Deposit Deposit
        {
            get => _deposit;
            set
            {
                _deposit = value;
                OnPropertyChanged(nameof(Deposit));
            }
        }

        public ObservableCollection<Currency> Currencies
        {
            get => _currencies;
            set
            {
                _currencies = value;
                OnPropertyChanged(nameof(Currencies));
            }
        }

        public ICommand UpdateDepositCommand { get; }

        public EditDepositViewModel(DepositRepository depositRepository, CurrencyRepository currencyRepository, Deposit deposit)
        {
            _depositRepository = depositRepository;
            _currencyRepository = currencyRepository;
            _deposit = deposit;

            UpdateDepositCommand = new RelayCommand(async _ => await UpdateDeposit());

            LoadData();
        }

        private async Task UpdateDeposit()
        {
            if (Deposit.Currency != null)
            {
                Deposit.CurrencyId = Deposit.Currency.CurrencyId;
                await _depositRepository.UpdateAsync(Deposit);
                DepositUpdated?.Invoke(this, Deposit);
            }
            else
            {
                // Обработка случая, когда валюта не выбрана
                MessageBox.Show("Пожалуйста, выберите валюту.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void LoadData()
        {
            var currencies = await _currencyRepository.GetAllAsync();
            Currencies = new ObservableCollection<Currency>(currencies);
        }

        public event EventHandler<Deposit> DepositUpdated;
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
