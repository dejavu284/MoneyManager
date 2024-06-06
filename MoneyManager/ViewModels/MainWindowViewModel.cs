using MoneyManager.Views;
using System.ComponentModel;
using System.Windows.Input;

namespace MoneyManager.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public ICommand ShowAccountViewCommand { get; }
        public ICommand ShowTransactionHistoryViewCommand { get; }
        public ICommand ShowDepositViewCommand { get; }

        public MainWindowViewModel()
        {
            ShowAccountViewCommand = new RelayCommand(_ => ShowAccountView());
            ShowTransactionHistoryViewCommand = new RelayCommand(_ => ShowTransactionHistoryView());
            ShowDepositViewCommand = new RelayCommand(_ => ShowDepositView());

            // Set default view
            ShowDepositView();
        }

        private void ShowAccountView()
        {
            CurrentView = new AccountView();
        }

        private void ShowTransactionHistoryView()
        {
            CurrentView = new TransactionHistoryView();
        }

        private void ShowDepositView()
        {
            CurrentView = new DepositView();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
