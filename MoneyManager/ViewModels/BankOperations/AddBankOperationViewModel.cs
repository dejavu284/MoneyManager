using MoneyManager.Models;
using MoneyManager.Data.Repositories.Concrete;
using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;

namespace MoneyManager.ViewModels.BankOperations
{
    public class AddBankOperationViewModel : INotifyPropertyChanged
    {
        private readonly BankOperationRepository _bankOperationRepository;
        private readonly AccountRepository _accountRepository;
        private readonly SubcategoryRepository _subcategoryRepository;

        private BankOperation _newBankOperation;
        private ObservableCollection<Account> _accounts;
        private ObservableCollection<Subcategory> _subcategories;
        private ObservableCollection<string> _operationTypes;

        public BankOperation NewBankOperation
        {
            get => _newBankOperation;
            set
            {
                _newBankOperation = value;
                OnPropertyChanged(nameof(NewBankOperation));
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

        public ObservableCollection<Subcategory> Subcategories
        {
            get => _subcategories;
            set
            {
                _subcategories = value;
                OnPropertyChanged(nameof(Subcategories));
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

        public ICommand AddBankOperationCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public AddBankOperationViewModel(BankOperationRepository bankOperationRepository, AccountRepository accountRepository, SubcategoryRepository subcategoryRepository)
        {
            _bankOperationRepository = bankOperationRepository;
            _accountRepository = accountRepository;
            _subcategoryRepository = subcategoryRepository;

            NewBankOperation = new BankOperation
            {
                OperationDate = DateTime.Now // Устанавливаем текущую дату
            };

            OperationTypes = new ObservableCollection<string> { "пополнение", "снятие" };

            AddBankOperationCommand = new RelayCommand(async _ => await AddBankOperation());

            LoadData();
        }

        private async Task AddBankOperation()
        {
            if (NewBankOperation.OperationType == "снятие")
            {
                if (NewBankOperation.Account.AccountBalance < NewBankOperation.OperationAmount)
                {
                    MessageBox.Show("Недостаточно средств на счету для выполнения операции.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                NewBankOperation.Account.AccountBalance -= NewBankOperation.OperationAmount;
            }
            else if (NewBankOperation.OperationType == "пополнение")
            {
                NewBankOperation.Account.AccountBalance += NewBankOperation.OperationAmount;
            }

            await _accountRepository.UpdateAsync(NewBankOperation.Account);
            await _bankOperationRepository.AddAsync(NewBankOperation);
            BankOperationAdded?.Invoke(this, NewBankOperation);
        }

        private async void LoadData()
        {
            var accounts = await _accountRepository.GetAllAsync();
            Accounts = new ObservableCollection<Account>(accounts);

            var subcategories = await _subcategoryRepository.GetAllAsync();
            Subcategories = new ObservableCollection<Subcategory>(subcategories);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event EventHandler<BankOperation> BankOperationAdded;
    }
}
