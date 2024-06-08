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
using System;

namespace MoneyManager.ViewModels.BankOperations
{
    public class BankOperationViewModel : INotifyPropertyChanged
    {
        private readonly BankOperationRepository _bankOperationRepository;
        private readonly AccountRepository _accountRepository;
        private readonly SubcategoryRepository _subcategoryRepository;
        private ObservableCollection<BankOperation> _bankOperations;
        private BankOperation _selectedBankOperation;
        private object _currentViewModel;

        public ObservableCollection<BankOperation> BankOperations
        {
            get => _bankOperations;
            set
            {
                _bankOperations = value;
                OnPropertyChanged(nameof(BankOperations));
            }
        }

        public BankOperation SelectedBankOperation
        {
            get => _selectedBankOperation;
            set
            {
                _selectedBankOperation = value;
                OnPropertyChanged(nameof(SelectedBankOperation));
                OnPropertyChanged(nameof(IsBankOperationSelected));
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

        public bool IsBankOperationSelected => SelectedBankOperation != null;

        public ICommand ShowAddBankOperationViewCommand { get; }
        public ICommand EditBankOperationCommand { get; }
        public ICommand DeleteBankOperationCommand { get; }
        public ICommand ShowDateRangeSelectionViewCommand { get; }
        public ICommand LoadOperationsForPeriodCommand { get; }

        public BankOperationViewModel()
        {
            var context = CreateDbContext();
            _bankOperationRepository = new BankOperationRepository(context);
            _accountRepository = new AccountRepository(context);
            _subcategoryRepository = new SubcategoryRepository(context);

            ShowAddBankOperationViewCommand = new RelayCommand(_ => ShowAddBankOperationView());
            EditBankOperationCommand = new RelayCommand(_ => ShowEditBankOperationView(), _ => IsBankOperationSelected);
            DeleteBankOperationCommand = new RelayCommand(async _ => await DeleteBankOperation(), _ => IsBankOperationSelected);
            ShowDateRangeSelectionViewCommand = new RelayCommand(_ => ShowDateRangeSelectionView());
            LoadOperationsForPeriodCommand = new RelayCommand(async _ => await LoadOperationsForPeriod());

            LoadBankOperations();

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

        private async void LoadBankOperations()
        {
            var bankOperations = await _bankOperationRepository.GetAllAsync();
            BankOperations = new ObservableCollection<BankOperation>(bankOperations);
        }

        private void ShowAddBankOperationView()
        {
            var addBankOperationViewModel = new AddBankOperationViewModel(_bankOperationRepository, _accountRepository, _subcategoryRepository);
            addBankOperationViewModel.BankOperationAdded += AddBankOperationViewModel_BankOperationAdded;
            CurrentViewModel = addBankOperationViewModel;
        }

        private void AddBankOperationViewModel_BankOperationAdded(object sender, BankOperation e)
        {
            BankOperations.Add(e);
            CurrentViewModel = null;
        }

        private void ShowEditBankOperationView()
        {
            var editBankOperationViewModel = new EditBankOperationViewModel(_bankOperationRepository, _accountRepository, _subcategoryRepository, SelectedBankOperation);
            editBankOperationViewModel.BankOperationUpdated += EditBankOperationViewModel_BankOperationUpdated;
            CurrentViewModel = editBankOperationViewModel;
        }

        private void EditBankOperationViewModel_BankOperationUpdated(object sender, BankOperation e)
        {
            var index = BankOperations.IndexOf(SelectedBankOperation);
            if (index >= 0)
            {
                BankOperations[index] = e;
                SelectedBankOperation = e;
                OnPropertyChanged(nameof(BankOperations));
            }
            CurrentViewModel = null;
        }

        private async Task DeleteBankOperation()
        {
            if (SelectedBankOperation != null)
            {
                var result = MessageBox.Show("Вы уверены, что хотите удалить данную операцию?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    await _bankOperationRepository.DeleteAsync(SelectedBankOperation.OperationId);
                    BankOperations.Remove(SelectedBankOperation);
                    SelectedBankOperation = null;
                    OnPropertyChanged(nameof(IsBankOperationSelected));
                }
            }
        }

        private void ShowDateRangeSelectionView()
        {
            var dateRangeSelectionViewModel = new DateRangeSelectionViewModel();
            dateRangeSelectionViewModel.DateRangeSelected += async (s, e) =>
            {
                StartDate = e.StartDate;
                EndDate = e.EndDate;
                await LoadOperationsForPeriod();
                CurrentViewModel = null;
            };
            CurrentViewModel = dateRangeSelectionViewModel;
        }

        private DateTime? _startDate;
        public DateTime? StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }

        private DateTime? _endDate;
        public DateTime? EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }

        private async Task LoadOperationsForPeriod()
        {
            if (StartDate.HasValue && EndDate.HasValue)
            {
                var operations = await _bankOperationRepository.GetOperationsForPeriodAsync(StartDate.Value, EndDate.Value);
                BankOperations = new ObservableCollection<BankOperation>(operations);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
