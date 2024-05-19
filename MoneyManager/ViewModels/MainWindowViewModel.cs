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

        public MainWindowViewModel()
        {
            ShowAccountViewCommand = new RelayCommand(_ => ShowAccountView());
            ShowTransactionHistoryViewCommand = new RelayCommand(_ => ShowTransactionHistoryView());

            // Set default view
            ShowAccountView();
        }

        private void ShowAccountView()
        {
            CurrentView = new AccountView();
        }

        private void ShowTransactionHistoryView()
        {
            CurrentView = new TransactionHistoryView();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
