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

namespace MoneyManager.ViewModels.Currencys
{
    public class CurrencyViewModel : INotifyPropertyChanged
    {
        private readonly CurrencyRepository _currencyRepository;
        private ObservableCollection<Currency> _currencies;
        private Currency _selectedCurrency;
        private object _currentViewModel;

        public ObservableCollection<Currency> Currencies
        {
            get => _currencies;
            set
            {
                _currencies = value;
                OnPropertyChanged(nameof(Currencies));
            }
        }

        public Currency SelectedCurrency
        {
            get => _selectedCurrency;
            set
            {
                _selectedCurrency = value;
                OnPropertyChanged(nameof(SelectedCurrency));
                OnPropertyChanged(nameof(IsCurrencySelected));
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

        public bool IsCurrencySelected => SelectedCurrency != null;

        public ICommand ShowAddCurrencyViewCommand { get; }
        public ICommand EditCurrencyCommand { get; }
        public ICommand DeleteCurrencyCommand { get; }

        public CurrencyViewModel()
        {
            var context = CreateDbContext();
            _currencyRepository = new CurrencyRepository(context);

            ShowAddCurrencyViewCommand = new RelayCommand(_ => ShowAddCurrencyView());
            EditCurrencyCommand = new RelayCommand(_ => ShowEditCurrencyView(), _ => IsCurrencySelected);
            DeleteCurrencyCommand = new RelayCommand(async _ => await DeleteCurrency(), _ => IsCurrencySelected);

            LoadCurrencies();

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

        private async void LoadCurrencies()
        {
            var currencies = await _currencyRepository.GetAllAsync();
            Currencies = new ObservableCollection<Currency>(currencies);
        }

        private void ShowAddCurrencyView()
        {
            var addCurrencyViewModel = new AddCurrencyViewModel(_currencyRepository);
            addCurrencyViewModel.CurrencyAdded += AddCurrencyViewModel_CurrencyAdded;
            CurrentViewModel = addCurrencyViewModel;
        }

        private void AddCurrencyViewModel_CurrencyAdded(object sender, Currency e)
        {
            Currencies.Add(e);
            CurrentViewModel = null;
        }

        private void ShowEditCurrencyView()
        {
            var editCurrencyViewModel = new EditCurrencyViewModel(_currencyRepository, SelectedCurrency);
            editCurrencyViewModel.CurrencyUpdated += EditCurrencyViewModel_CurrencyUpdated;
            CurrentViewModel = editCurrencyViewModel;
        }

        private void EditCurrencyViewModel_CurrencyUpdated(object sender, Currency e)
        {
            var index = Currencies.IndexOf(SelectedCurrency);
            if (index >= 0)
            {
                Currencies[index] = e;
                SelectedCurrency = e;
                OnPropertyChanged(nameof(Currencies));
            }
            CurrentViewModel = null;
        }

        private async Task DeleteCurrency()
        {
            if (SelectedCurrency != null)
            {
                // Показать всплывающее окно с подтверждением
                var result = MessageBox.Show("Вы уверены, что хотите удалить данную валюту?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    SelectedCurrency.Status = false;
                    await _currencyRepository.UpdateAsync(SelectedCurrency);
                    Currencies.Remove(SelectedCurrency);
                    SelectedCurrency = null;
                    OnPropertyChanged(nameof(IsCurrencySelected));
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
