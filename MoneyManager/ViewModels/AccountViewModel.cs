using MoneyManager.Models;
using MoneyManager.Data.Repositories.Concrete;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Windows;
using MoneyManager.Data;

namespace MoneyManager.ViewModels
{
    public class AccountViewModel : INotifyPropertyChanged
    {
        private readonly AccountRepository _accountRepository;
        private readonly CurrencyRepository _currencyRepository;
        private ObservableCollection<Account> _accounts;
        private Account _selectedAccount;
        private object _currentViewModel;

        public ObservableCollection<Account> Accounts
        {
            get => _accounts;
            set
            {
                _accounts = value;
                OnPropertyChanged(nameof(Accounts));
            }
        }

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

        public object CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        public bool IsAccountSelected => SelectedAccount != null;

        public ICommand ShowAddAccountViewCommand { get; }
        public ICommand EditAccountCommand { get; }
        public ICommand DeleteAccountCommand { get; }

        public AccountViewModel()
        {
            var context = CreateDbContext();
            _accountRepository = new AccountRepository(context);
            _currencyRepository = new CurrencyRepository(context);

            ShowAddAccountViewCommand = new RelayCommand(_ => ShowAddAccountView());
            EditAccountCommand = new RelayCommand(_ => ShowEditAccountView(), _ => IsAccountSelected);
            DeleteAccountCommand = new RelayCommand(async _ => await DeleteAccount(), _ => IsAccountSelected);

            LoadAccounts();

            // Set default view
            CurrentViewModel = null;
        }

        private static MoneyManagerContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MoneyManagerContext>();
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            optionsBuilder.UseNpgsql(connectionString);
            return new MoneyManagerContext(optionsBuilder.Options);
        }

        private async void LoadAccounts()
        {
            var accounts = await _accountRepository.GetAllAsync();
            Accounts = new ObservableCollection<Account>(accounts);
        }

        private void ShowAddAccountView()
        {
            var addAccountViewModel = new AddAccountViewModel(_accountRepository, _currencyRepository);
            addAccountViewModel.AccountAdded += AddAccountViewModel_AccountAdded;
            CurrentViewModel = addAccountViewModel;
        }

        private void AddAccountViewModel_AccountAdded(object sender, Account e)
        {
            Accounts.Add(e);
            CurrentViewModel = null;
        }

        private void ShowEditAccountView()
        {
            var editAccountViewModel = new EditAccountViewModel(_accountRepository, _currencyRepository, SelectedAccount);
            editAccountViewModel.AccountUpdated += EditAccountViewModel_AccountUpdated;
            CurrentViewModel = editAccountViewModel;
        }

        private void EditAccountViewModel_AccountUpdated(object sender, Account e)
        {
            var index = Accounts.IndexOf(SelectedAccount);
            if (index >= 0)
            {
                Accounts[index] = e;
                SelectedAccount = e;
                OnPropertyChanged(nameof(Accounts));
            }
            CurrentViewModel = null;
        }

        private async Task DeleteAccount()
        {
            if (SelectedAccount != null)
            {
                // Показать всплывающее окно с подтверждением
                var result = MessageBox.Show("Вы уверены, что хотите удалить данный счёт?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    await _accountRepository.DeleteAsync(SelectedAccount.AccountId);
                    Accounts.Remove(SelectedAccount);
                    SelectedAccount = null;
                    OnPropertyChanged(nameof(IsAccountSelected));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
