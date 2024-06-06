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

namespace MoneyManager.ViewModels.Deposits
{
    public class DepositViewModel : INotifyPropertyChanged
    {
        private readonly DepositRepository _depositRepository;
        private readonly CurrencyRepository _currencyRepository;
        private ObservableCollection<Deposit> _deposits;
        private Deposit _selectedDeposit;
        private object _currentViewModel;

        public ObservableCollection<Deposit> Deposits
        {
            get => _deposits;
            set
            {
                _deposits = value;
                OnPropertyChanged(nameof(Deposits));
            }
        }

        public Deposit SelectedDeposit
        {
            get => _selectedDeposit;
            set
            {
                _selectedDeposit = value;
                OnPropertyChanged(nameof(SelectedDeposit));
                OnPropertyChanged(nameof(IsDepositSelected));
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

        public bool IsDepositSelected => SelectedDeposit != null;

        public ICommand ShowAddDepositViewCommand { get; }
        public ICommand EditDepositCommand { get; }
        public ICommand DeleteDepositCommand { get; }

        public DepositViewModel()
        {
            var context = CreateDbContext();
            _depositRepository = new DepositRepository(context);
            _currencyRepository = new CurrencyRepository(context);

            ShowAddDepositViewCommand = new RelayCommand(_ => ShowAddDepositView());
            EditDepositCommand = new RelayCommand(_ => ShowEditDepositView(), _ => IsDepositSelected);
            DeleteDepositCommand = new RelayCommand(async _ => await DeleteDeposit(), _ => IsDepositSelected);

            LoadDeposits();

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

        private async void LoadDeposits()
        {
            var deposits = await _depositRepository.GetAllAsync();
            Deposits = new ObservableCollection<Deposit>(deposits);
        }

        private void ShowAddDepositView()
        {
            var addDepositViewModel = new AddDepositViewModel(_depositRepository, _currencyRepository);
            addDepositViewModel.DepositAdded += AddDepositViewModel_DepositAdded;
            CurrentViewModel = addDepositViewModel;
        }

        private void AddDepositViewModel_DepositAdded(object sender, Deposit e)
        {
            Deposits.Add(e);
            CurrentViewModel = null;
        }

        private void ShowEditDepositView()
        {
            var editDepositViewModel = new EditDepositViewModel(_depositRepository, _currencyRepository, SelectedDeposit);
            editDepositViewModel.DepositUpdated += EditDepositViewModel_DepositUpdated;
            CurrentViewModel = editDepositViewModel;
        }

        private void EditDepositViewModel_DepositUpdated(object sender, Deposit e)
        {
            var index = Deposits.IndexOf(SelectedDeposit);
            if (index >= 0)
            {
                Deposits[index] = e;
                SelectedDeposit = e;
                OnPropertyChanged(nameof(Deposits));
            }
            CurrentViewModel = null;
        }

        private async Task DeleteDeposit()
        {
            if (SelectedDeposit != null)
            {
                // Показать всплывающее окно с подтверждением
                var result = MessageBox.Show("Вы уверены, что хотите удалить данный вклад?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    await _depositRepository.DeleteAsync(SelectedDeposit.DepositId);
                    Deposits.Remove(SelectedDeposit);
                    SelectedDeposit = null;
                    OnPropertyChanged(nameof(IsDepositSelected));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
