using MoneyManager.Models;
using MoneyManager.Data.Repositories.Concrete;
using System.ComponentModel;
using System.Windows.Input;
using System.Threading.Tasks;
using System;

namespace MoneyManager.ViewModels.BankOperations
{
    public class EditBankOperationViewModel : INotifyPropertyChanged
    {
        private readonly BankOperationRepository _bankOperationRepository;
        private BankOperation _selectedBankOperation;

        public BankOperation SelectedBankOperation
        {
            get => _selectedBankOperation;
            set
            {
                _selectedBankOperation = value;
                OnPropertyChanged(nameof(SelectedBankOperation));
            }
        }

        public ICommand UpdateBankOperationCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public EditBankOperationViewModel(BankOperationRepository bankOperationRepository, BankOperation selectedBankOperation)
        {
            _bankOperationRepository = bankOperationRepository;
            SelectedBankOperation = selectedBankOperation;
            UpdateBankOperationCommand = new RelayCommand(async _ => await UpdateBankOperation());
        }

        private async Task UpdateBankOperation()
        {
            await _bankOperationRepository.UpdateAsync(SelectedBankOperation);
            BankOperationUpdated?.Invoke(this, SelectedBankOperation);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event EventHandler<BankOperation> BankOperationUpdated;
    }
}
