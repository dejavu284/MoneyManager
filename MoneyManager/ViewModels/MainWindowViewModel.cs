using MoneyManager.Views.Accounts;
using MoneyManager.Views.Categorys;
using MoneyManager.Views.Currencys;
using MoneyManager.Views.Deposits;
using MoneyManager.Views.Subcategories;
using MoneyManager.Views.BankOperations;
using MoneyManager.Views.DepositOperations;
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
        public ICommand ShowDepositViewCommand { get; }
        public ICommand ShowCategoryViewCommand { get; }
        public ICommand ShowCurrencyViewCommand { get; }
        public ICommand ShowSubcategoryViewCommand { get; }
        public ICommand ShowBankOperationViewCommand { get; }
        public ICommand ShowDepositOperationViewCommand { get; }


        public MainWindowViewModel()
        {
            ShowAccountViewCommand = new RelayCommand(_ => ShowAccountView());
            ShowDepositViewCommand = new RelayCommand(_ => ShowDepositView());
            ShowCategoryViewCommand = new RelayCommand(_ => ShowCategoryView());
            ShowCurrencyViewCommand = new RelayCommand(_ => ShowCurrencyView());
            ShowSubcategoryViewCommand = new RelayCommand(_ => ShowSubcategoryView());
            ShowBankOperationViewCommand = new RelayCommand(_ => ShowBankOperationView());
            ShowDepositOperationViewCommand = new RelayCommand(_ => ShowDepositOperationView());

            // Set default view
            ShowDepositView();
        }

        private void ShowAccountView()
        {
            CurrentView = new AccountView();
        }

        private void ShowDepositView()
        {
            CurrentView = new DepositView();
        }

        private void ShowCategoryView()
        {
            CurrentView = new CategoryView();
        }

        private void ShowCurrencyView()
        {
            CurrentView = new CurrencyView();
        }

        private void ShowSubcategoryView()
        {
            CurrentView = new SubcategoryView();
        }

        private void ShowBankOperationView()
        {
            CurrentView = new BankOperationView();
        }

        private void ShowDepositOperationView()
        {
            CurrentView = new DepositOperationView();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
