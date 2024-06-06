using MoneyManager.Models;
using MoneyManager.Data.Repositories.Concrete;
using System.ComponentModel;
using System.Windows.Input;
using System.Threading.Tasks;
using System;

namespace MoneyManager.ViewModels
{
    public class AddDepositViewModel : INotifyPropertyChanged
    {
        private readonly DepositRepository _depositRepository;
        private readonly CurrencyRepository _currencyRepository;
        private Deposit _newDeposit;
        private string _currencyCode;

        public Deposit NewDeposit
        {
            get => _newDeposit;
            set
            {
                _newDeposit = value;
                OnPropertyChanged(nameof(NewDeposit));
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
        }

        private async Task AddDeposit()
        {
            var currency = await _currencyRepository.GetByCodeAsync(CurrencyCode);
            if (currency != null)
            {
                NewDeposit.CurrencyId = currency.CurrencyId;
                NewDeposit.Status = true;  // Активируем вклад при добавлении
                await _depositRepository.AddAsync(NewDeposit);
                DepositAdded?.Invoke(this, NewDeposit);
            }
            else
            {
                // Обработка случая, когда валюта с указанным кодом не найдена
                // Например, можно показать сообщение пользователю
            }
        }

        public event EventHandler<Deposit> DepositAdded;
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
