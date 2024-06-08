using MoneyManager.Models;
using MoneyManager.Data.Repositories.Concrete;
using System.ComponentModel;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System;

namespace MoneyManager.ViewModels.BankOperations
{
    public class EditBankOperationViewModel : INotifyPropertyChanged
    {
        private readonly BankOperationRepository _bankOperationRepository;
        private readonly AccountRepository _accountRepository;
        private readonly SubcategoryRepository _subcategoryRepository;

        private BankOperation _selectedBankOperation;
        private ObservableCollection<Account> _accounts;
        private ObservableCollection<Subcategory> _subcategories;
        private ObservableCollection<string> _operationTypes;

        public BankOperation SelectedBankOperation
        {
            get => _selectedBankOperation;
            set
            {
                _selectedBankOperation = value;
                OnPropertyChanged(nameof(SelectedBankOperation));
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

        public ICommand UpdateBankOperationCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public EditBankOperationViewModel(BankOperationRepository bankOperationRepository, AccountRepository accountRepository, SubcategoryRepository subcategoryRepository, BankOperation selectedBankOperation)
        {
            _bankOperationRepository = bankOperationRepository;
            _accountRepository = accountRepository;
            _subcategoryRepository = subcategoryRepository;

            SelectedBankOperation = selectedBankOperation;
            UpdateBankOperationCommand = new RelayCommand(async _ => await UpdateBankOperation());

            LoadData();
        }

        private async Task UpdateBankOperation()
        {
            await _bankOperationRepository.UpdateAsync(SelectedBankOperation);
            BankOperationUpdated?.Invoke(this, SelectedBankOperation);
        }

        private async void LoadData()
        {
            var accounts = await _accountRepository.GetAllAsync();
            Accounts = new ObservableCollection<Account>(accounts);

            var subcategories = await _subcategoryRepository.GetAllAsync();
            Subcategories = new ObservableCollection<Subcategory>(subcategories);

            OperationTypes = new ObservableCollection<string> { "Пополнение", "Снятие" };
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event EventHandler<BankOperation> BankOperationUpdated;
    }
}
