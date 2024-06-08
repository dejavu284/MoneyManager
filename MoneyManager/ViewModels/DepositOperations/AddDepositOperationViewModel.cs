using MoneyManager.Models;
using MoneyManager.Data.Repositories.Concrete;
using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;

namespace MoneyManager.ViewModels.DepositOperations
{
    public class AddDepositOperationViewModel : INotifyPropertyChanged
    {
        private readonly DepositOperationRepository _depositOperationRepository;
        private readonly AccountRepository _accountRepository;
        private readonly DepositRepository _depositRepository;

        private DepositOperation _newDepositOperation;
        private ObservableCollection<Account> _accounts;
        private ObservableCollection<Deposit> _deposits;
        private ObservableCollection<string> _operationTypes;

        public DepositOperation NewDepositOperation
        {
            get => _newDepositOperation;
            set
            {
                _newDepositOperation = value;
                OnPropertyChanged(nameof(NewDepositOperation));
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

        public ICommand AddDepositOperationCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public AddDepositOperationViewModel(DepositOperationRepository depositOperationRepository, AccountRepository accountRepository, DepositRepository depositRepository)
        {
            _depositOperationRepository = depositOperationRepository;
            _accountRepository = accountRepository;
            _depositRepository = depositRepository;

            NewDepositOperation = new DepositOperation
            {
                OperationDate = DateTime.Now // Устанавливаем текущую дату
            };

            OperationTypes = new ObservableCollection<string> { "Пополнение", "Снятие" };

            AddDepositOperationCommand = new RelayCommand(async _ => await AddDepositOperation());

            LoadData();
        }

        private async Task AddDepositOperation()
        {
            if (NewDepositOperation.OperationType == "Снятие")
            {
                if (NewDepositOperation.Deposit.DepositAmount < NewDepositOperation.OperationAmount)
                {
                    MessageBox.Show("Недостаточно средств на вкладе для выполнения операции.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                NewDepositOperation.Deposit.DepositAmount -= NewDepositOperation.OperationAmount;
                NewDepositOperation.Account.AccountBalance += NewDepositOperation.OperationAmount;
            }
            else if (NewDepositOperation.OperationType == "Пополнение")
            {
                if (NewDepositOperation.Account.AccountBalance < NewDepositOperation.OperationAmount)
                {
                    MessageBox.Show("Недостаточно средств на счету для выполнения операции.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                NewDepositOperation.Account.AccountBalance -= NewDepositOperation.OperationAmount;
                NewDepositOperation.Deposit.DepositAmount += NewDepositOperation.OperationAmount;
            }

            await _accountRepository.UpdateAsync(NewDepositOperation.Account);
            await _depositRepository.UpdateAsync(NewDepositOperation.Deposit);
            await _depositOperationRepository.AddAsync(NewDepositOperation);
            DepositOperationAdded?.Invoke(this, NewDepositOperation);
        }

        private async void LoadData()
        {
            var accounts = await _accountRepository.GetAllAsync();
            Accounts = new ObservableCollection<Account>(accounts);

            var deposits = await _depositRepository.GetAllAsync();
            Deposits = new ObservableCollection<Deposit>(deposits);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event EventHandler<DepositOperation> DepositOperationAdded;
    }
}
