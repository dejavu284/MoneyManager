using MoneyManager.Models;
using MoneyManager.Data.Repositories.Concrete;
using System.ComponentModel;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System;

namespace MoneyManager.ViewModels.BankOperations
{
    public class AddBankOperationViewModel : INotifyPropertyChanged
    {
        private readonly BankOperationRepository _bankOperationRepository;
        private BankOperation _newBankOperation;

        public BankOperation NewBankOperation
        {
            get => _newBankOperation;
            set
            {
                _newBankOperation = value;
                OnPropertyChanged(nameof(NewBankOperation));
            }
        }

        public ICommand AddBankOperationCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public AddBankOperationViewModel(BankOperationRepository bankOperationRepository)
        {
            _bankOperationRepository = bankOperationRepository;
            NewBankOperation = new BankOperation();
            AddBankOperationCommand = new RelayCommand(async _ => await AddBankOperation());
        }

        private async Task AddBankOperation()
        {
            await _bankOperationRepository.AddAsync(NewBankOperation);
            BankOperationAdded?.Invoke(this, NewBankOperation);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event EventHandler<BankOperation> BankOperationAdded;
    }
}
