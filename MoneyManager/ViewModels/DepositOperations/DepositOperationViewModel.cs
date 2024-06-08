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

namespace MoneyManager.ViewModels.DepositOperations
{
    public class DepositOperationViewModel : INotifyPropertyChanged
    {
        private readonly DepositOperationRepository _depositOperationRepository;
        private readonly AccountRepository _accountRepository;
        private readonly DepositRepository _depositRepository;
        private ObservableCollection<DepositOperation> _depositOperations;
        private DepositOperation _selectedDepositOperation;
        private object _currentViewModel;

        public ObservableCollection<DepositOperation> DepositOperations
        {
            get => _depositOperations;
            set
            {
                _depositOperations = value;
                OnPropertyChanged(nameof(DepositOperations));
            }
        }

        public DepositOperation SelectedDepositOperation
        {
            get => _selectedDepositOperation;
            set
            {
                _selectedDepositOperation = value;
                OnPropertyChanged(nameof(SelectedDepositOperation));
                OnPropertyChanged(nameof(IsDepositOperationSelected));
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

        public bool IsDepositOperationSelected => SelectedDepositOperation != null;

        public ICommand ShowAddDepositOperationViewCommand { get; }
        public ICommand EditDepositOperationCommand { get; }
        public ICommand DeleteDepositOperationCommand { get; }
        public ICommand ShowDateRangeSelectionViewCommand { get; }
        public ICommand LoadOperationsForPeriodCommand { get; }

        public DepositOperationViewModel()
        {
            var context = CreateDbContext();
            _depositOperationRepository = new DepositOperationRepository(context);
            _accountRepository = new AccountRepository(context);
            _depositRepository = new DepositRepository(context);

            ShowAddDepositOperationViewCommand = new RelayCommand(_ => ShowAddDepositOperationView());
            EditDepositOperationCommand = new RelayCommand(_ => ShowEditDepositOperationView(), _ => IsDepositOperationSelected);
            DeleteDepositOperationCommand = new RelayCommand(async _ => await DeleteDepositOperation(), _ => IsDepositOperationSelected);
            ShowDateRangeSelectionViewCommand = new RelayCommand(_ => ShowDateRangeSelectionView());
            LoadOperationsForPeriodCommand = new RelayCommand(async _ => await LoadOperationsForPeriod());

            LoadDepositOperations();

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

        private async void LoadDepositOperations()
        {
            var depositOperations = await _depositOperationRepository.GetAllAsync();
            DepositOperations = new ObservableCollection<DepositOperation>(depositOperations);
        }

        private void ShowAddDepositOperationView()
        {
            var addDepositOperationViewModel = new AddDepositOperationViewModel(_depositOperationRepository, _accountRepository, _depositRepository);
            addDepositOperationViewModel.DepositOperationAdded += AddDepositOperationViewModel_DepositOperationAdded;
            CurrentViewModel = addDepositOperationViewModel;
        }

        private void AddDepositOperationViewModel_DepositOperationAdded(object sender, DepositOperation e)
        {
            DepositOperations.Add(e);
            CurrentViewModel = null;
        }

        private void ShowEditDepositOperationView()
        {
            var editDepositOperationViewModel = new EditDepositOperationViewModel(_depositOperationRepository, _accountRepository, _depositRepository, SelectedDepositOperation);
            editDepositOperationViewModel.DepositOperationUpdated += EditDepositOperationViewModel_DepositOperationUpdated;
            CurrentViewModel = editDepositOperationViewModel;
        }

        private void EditDepositOperationViewModel_DepositOperationUpdated(object sender, DepositOperation e)
        {
            var index = DepositOperations.IndexOf(SelectedDepositOperation);
            if (index >= 0)
            {
                DepositOperations[index] = e;
                SelectedDepositOperation = e;
                OnPropertyChanged(nameof(DepositOperations));
            }
            CurrentViewModel = null;
        }

        private async Task DeleteDepositOperation()
        {
            if (SelectedDepositOperation != null)
            {
                var result = MessageBox.Show("Вы уверены, что хотите удалить данную операцию?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    await _depositOperationRepository.DeleteAsync(SelectedDepositOperation.DepositOperationId);
                    DepositOperations.Remove(SelectedDepositOperation);
                    SelectedDepositOperation = null;
                    OnPropertyChanged(nameof(IsDepositOperationSelected));
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
                var operations = await _depositOperationRepository.GetOperationsForPeriodAsync(StartDate.Value, EndDate.Value);
                DepositOperations = new ObservableCollection<DepositOperation>(operations);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
