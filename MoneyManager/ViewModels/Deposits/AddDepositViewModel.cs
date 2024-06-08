using MoneyManager.Models;
using MoneyManager.Data.Repositories.Concrete;
using System.ComponentModel;
using System.Windows.Input;
using System.Threading.Tasks;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace MoneyManager.ViewModels.Deposits
{
    public class AddDepositViewModel : INotifyPropertyChanged
    {
        private readonly DepositRepository _depositRepository;
        private readonly CurrencyRepository _currencyRepository;
        private Deposit _newDeposit;

        private ObservableCollection<Currency> _currencies;

        public ObservableCollection<Currency> Currencies
        {
            get => _currencies;
            set
            {
                _currencies = value;
                OnPropertyChanged(nameof(Currencies));
            }
        }

        public Deposit NewDeposit
        {
            get => _newDeposit;
            set
            {
                _newDeposit = value;
                OnPropertyChanged(nameof(NewDeposit));
            }
        }

        public ICommand AddDepositCommand { get; }

        public AddDepositViewModel(DepositRepository depositRepository, CurrencyRepository currencyRepository)
        {
            _depositRepository = depositRepository;
            _currencyRepository = currencyRepository;
            _newDeposit = new Deposit
            {
                StartDate = DateTime.Today,
                EndDate = DateTime.Today
            };

            AddDepositCommand = new RelayCommand(async _ => await AddDeposit());

            LoadData();
        }

        private async Task AddDeposit()
        {
            if (NewDeposit.Currency != null)
            {
                NewDeposit.CurrencyId = NewDeposit.Currency.CurrencyId;
                NewDeposit.Status = true;  // Активируем вклад при добавлении
                await _depositRepository.AddAsync(NewDeposit);
                DepositAdded?.Invoke(this, NewDeposit);
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

        public event EventHandler<Deposit> DepositAdded;
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
