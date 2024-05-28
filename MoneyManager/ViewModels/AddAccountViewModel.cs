using MoneyManager.Models;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace MoneyManager.ViewModels
{
    public class AddAccountViewModel : INotifyPropertyChanged
    {
        public Account NewAccount { get; set; }
        public ICommand AddAccountCommand { get; }

        private readonly Action _onAccountAdded;

        public AddAccountViewModel(Action onAccountAdded)
        {
            NewAccount = new Account();
            _onAccountAdded = onAccountAdded;
            AddAccountCommand = new RelayCommand(_ => AddAccount());
        }

        private void AddAccount()
        {
            _onAccountAdded?.Invoke();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
