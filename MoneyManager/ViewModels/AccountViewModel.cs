﻿using MoneyManager.Models;
using MoneyManager.Data.Repositories.Concrete;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Data;

namespace MoneyManager.ViewModels
{
    public class AccountViewModel : INotifyPropertyChanged
    {
        private readonly AccountRepository _accountRepository;
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

        public AccountViewModel()
        {
            _accountRepository = new AccountRepository(CreateDbContext());

            ShowAddAccountViewCommand = new RelayCommand(_ => ShowAddAccountView());
            EditAccountCommand = new RelayCommand(_ => ShowEditAccountView(), _ => IsAccountSelected);

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
            var addAccountViewModel = new AddAccountViewModel(_accountRepository);
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
            var editAccountViewModel = new EditAccountViewModel(_accountRepository, SelectedAccount);
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
