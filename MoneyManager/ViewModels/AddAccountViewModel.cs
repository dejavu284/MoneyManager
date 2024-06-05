﻿using MoneyManager.Models;
using MoneyManager.Models;
using MoneyManager.Data.Repositories.Concrete;
using System.ComponentModel;
using System.Windows.Input;
using System.Threading.Tasks;
using System;

namespace MoneyManager.ViewModels
{
    public class AddAccountViewModel : INotifyPropertyChanged
    {
        private readonly AccountRepository _accountRepository;
        private Account _newAccount;

        public Account NewAccount
        {
            get => _newAccount;
            set
            {
                _newAccount = value;
                OnPropertyChanged(nameof(NewAccount));
            }
        }

        public ICommand AddAccountCommand { get; }

        public AddAccountViewModel(AccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
            _newAccount = new Account();

            AddAccountCommand = new RelayCommand(async _ => await AddAccount());
        }

        private async Task AddAccount()
        {
            await _accountRepository.AddAsync(NewAccount);
            AccountAdded?.Invoke(this, NewAccount);
        }

        public event EventHandler<Account> AccountAdded;
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}


//using System;
//using System.ComponentModel;
//using System.Windows.Input;

//namespace MoneyManager.ViewModels
//{
//    public class AddAccountViewModel : INotifyPropertyChanged
//    {
//        public Account NewAccount { get; set; }
//        public ICommand AddAccountCommand { get; }

//        private readonly Action _onAccountAdded;

//        public AddAccountViewModel(Action onAccountAdded)
//        {
//            NewAccount = new Account();
//            _onAccountAdded = onAccountAdded;
//            AddAccountCommand = new RelayCommand(_ => AddAccount());
//        }

//        private void AddAccount()
//        {
//            _onAccountAdded?.Invoke();
//        }

//        public event PropertyChangedEventHandler PropertyChanged;
//        protected virtual void OnPropertyChanged(string propertyName)
//        {
//            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//        }
//    }
//}
