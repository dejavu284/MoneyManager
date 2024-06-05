using MoneyManager.Models;
using MoneyManager.Data.Repositories.Concrete;
using System.ComponentModel;
using System.Windows.Input;
using System.Threading.Tasks;
using System;

namespace MoneyManager.ViewModels
{
    public class EditAccountViewModel : INotifyPropertyChanged
    {
        private readonly AccountRepository _accountRepository;
        private Account _account;

        public Account Account
        {
            get => _account;
            set
            {
                _account = value;
                OnPropertyChanged(nameof(Account));
            }
        }

        public ICommand UpdateAccountCommand { get; }

        public EditAccountViewModel(AccountRepository accountRepository, Account account)
        {
            _accountRepository = accountRepository;
            _account = account;

            UpdateAccountCommand = new RelayCommand(async _ => await UpdateAccount());
        }

        private async Task UpdateAccount()
        {
            await _accountRepository.UpdateAsync(Account);
            AccountUpdated?.Invoke(this, Account);
        }

        public event EventHandler<Account> AccountUpdated;
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
