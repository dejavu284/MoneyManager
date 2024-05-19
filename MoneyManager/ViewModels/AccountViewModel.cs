using MoneyManager.Models;
using MoneyManager.Data.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MoneyManager.ViewModels
{
    public class AccountViewModel : INotifyPropertyChanged
    {
        private readonly AccountService _accountService;

        public ObservableCollection<Account> Accounts { get; set; }
        private Account _selectedAccount;
        public Account SelectedAccount
        {
            get => _selectedAccount;
            set
            {
                _selectedAccount = value;
                OnPropertyChanged(nameof(SelectedAccount));
                OnPropertyChanged(nameof(IsAccountSelected));
            }
        }

        public bool IsAccountSelected => SelectedAccount != null;

        public ICommand LoadAccountsCommand { get; }
        public ICommand AddAccountCommand { get; }
        public ICommand EditAccountCommand { get; }
        public ICommand DeleteAccountCommand { get; }

        public AccountViewModel()
        {
            _accountService = new AccountService();
            Accounts = new ObservableCollection<Account>();
            LoadAccountsCommand = new RelayCommand(async _ => await LoadAccounts());
            AddAccountCommand = new RelayCommand(async _ => await AddAccount());
            EditAccountCommand = new RelayCommand(async _ => await EditAccount(), _ => IsAccountSelected);
            DeleteAccountCommand = new RelayCommand(async _ => await DeleteAccount(), _ => IsAccountSelected);

            // Load accounts on initialization
            LoadAccounts().ConfigureAwait(false);
        }

        private async Task LoadAccounts()
        {
            var accounts = await _accountService.GetAllAsync();
            Accounts.Clear();
            foreach (var account in accounts)
            {
                Accounts.Add(account);
            }
        }

        private async Task AddAccount()
        {
            var newAccount = new Account { AccountBalance = 1000.00m, CurrencyId = 1 };
            await _accountService.AddAsync(newAccount);
            await LoadAccounts();
        }

        private async Task EditAccount()
        {
            if (SelectedAccount != null)
            {
                // Here you can add logic to edit account
                await _accountService.UpdateAsync(SelectedAccount);
                await LoadAccounts();
            }
        }

        private async Task DeleteAccount()
        {
            if (SelectedAccount != null)
            {
                await _accountService.DeleteAsync(SelectedAccount.AccountId);
                await LoadAccounts();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
