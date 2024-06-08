using MoneyManager.Models;
using MoneyManager.Data.Repositories.Concrete;
using System.ComponentModel;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System;

namespace MoneyManager.ViewModels.DepositOperations
{
    public class EditDepositOperationViewModel : INotifyPropertyChanged
    {
        private readonly DepositOperationRepository _depositOperationRepository;
        private readonly AccountRepository _accountRepository;
        private readonly DepositRepository _depositRepository;

        private DepositOperation _selectedDepositOperation;
        private ObservableCollection<Account> _accounts;
        private ObservableCollection<Deposit> _deposits;
        private ObservableCollection<string> _operationTypes;

        public DepositOperation SelectedDepositOperation
        {
            get => _selectedDepositOperation;
            set
            {
                _selectedDepositOperation = value;
                OnPropertyChanged(nameof(SelectedDepositOperation));
            }
        }

        public ObservableCollection<Account> Accounts
        {
            get => _accounts;
            set
            {
                _accounts = value;
                OnPropertyChanged(nameof(Accounts));
            }
        }

        public ObservableCollection<Deposit> Deposits
        {
            get => _deposits;
            set
            {
                _deposits = value;
                OnPropertyChanged(nameof(Deposits));
            }
        }

        public ObservableCollection<string> OperationTypes
        {
            get => _operationTypes;
            set
            {
                _operationTypes = value;
                OnPropertyChanged(nameof(OperationTypes));
            }
        }

        public ICommand UpdateDepositOperationCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public EditDepositOperationViewModel(DepositOperationRepository depositOperationRepository, AccountRepository accountRepository, DepositRepository depositRepository, DepositOperation selectedDepositOperation)
        {
            _depositOperationRepository = depositOperationRepository;
            _accountRepository = accountRepository;
            _depositRepository = depositRepository;

            SelectedDepositOperation = selectedDepositOperation;
            UpdateDepositOperationCommand = new RelayCommand(async _ => await UpdateDepositOperation());

            LoadData();
        }

        private async Task UpdateDepositOperation()
        {
            await _depositOperationRepository.UpdateAsync(SelectedDepositOperation);
            DepositOperationUpdated?.Invoke(this, SelectedDepositOperation);
        }

        private async void LoadData()
        {
            var accounts = await _accountRepository.GetAllAsync();
            Accounts = new ObservableCollection<Account>(accounts);

            var deposits = await _depositRepository.GetAllAsync();
            Deposits = new ObservableCollection<Deposit>(deposits);

            OperationTypes = new ObservableCollection<string> { "Пополнение", "Снятие" };
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event EventHandler<DepositOperation> DepositOperationUpdated;
    }
}
