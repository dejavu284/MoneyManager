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

namespace MoneyManager.ViewModels.Subcategories
{
    public class SubcategoryViewModel : INotifyPropertyChanged
    {
        private readonly SubcategoryRepository _subcategoryRepository;
        private readonly CategoryRepository _categoryRepository;
        private ObservableCollection<Subcategory> _subcategorys;
        private Subcategory _selectedSubcategory;
        private object _currentViewModel;

        public ObservableCollection<Subcategory> Subcategorys
        {
            get => _subcategorys;
            set
            {
                _subcategorys = value;
                OnPropertyChanged(nameof(Subcategorys));
            }
        }

        public Subcategory SelectedSubcategory
        {
            get => _selectedSubcategory;
            set
            {
                _selectedSubcategory = value;
                OnPropertyChanged(nameof(SelectedSubcategory));
                OnPropertyChanged(nameof(IsSubcategorySelected));
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

        public bool IsSubcategorySelected => SelectedSubcategory != null;

        public ICommand ShowAddSubcategoryViewCommand { get; }
        public ICommand EditSubcategoryCommand { get; }
        public ICommand DeleteSubcategoryCommand { get; }

        public SubcategoryViewModel()
        {
            var context = CreateDbContext();
            _subcategoryRepository = new SubcategoryRepository(context);
            _categoryRepository = new CategoryRepository(context);

            ShowAddSubcategoryViewCommand = new RelayCommand(_ => ShowAddSubcategoryView());
            EditSubcategoryCommand = new RelayCommand(_ => ShowEditSubcategoryView(), _ => IsSubcategorySelected);
            DeleteSubcategoryCommand = new RelayCommand(async _ => await DeleteSubcategory(), _ => IsSubcategorySelected);

            LoadSubcategorys();

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

        private async void LoadSubcategorys()
        {
            var subcategorys = await _subcategoryRepository.GetAllAsync();
            Subcategorys = new ObservableCollection<Subcategory>(subcategorys);
        }

        private void ShowAddSubcategoryView()
        {
            var addSubcategoryViewModel = new AddSubcategoryViewModel(_subcategoryRepository, _categoryRepository);
            addSubcategoryViewModel.SubcategoryAdded += AddSubcategoryViewModel_SubcategoryAdded;
            CurrentViewModel = addSubcategoryViewModel;
        }

        private void AddSubcategoryViewModel_SubcategoryAdded(object sender, Subcategory e)
        {
            Subcategorys.Add(e);
            CurrentViewModel = null;
        }

        private void ShowEditSubcategoryView()
        {
            var editSubcategoryViewModel = new EditSubcategoryViewModel(_subcategoryRepository, _categoryRepository, SelectedSubcategory);
            editSubcategoryViewModel.SubcategoryUpdated += EditSubcategoryViewModel_SubcategoryUpdated;
            CurrentViewModel = editSubcategoryViewModel;
        }

        private void EditSubcategoryViewModel_SubcategoryUpdated(object sender, Subcategory e)
        {
            var index = Subcategorys.IndexOf(SelectedSubcategory);
            if (index >= 0)
            {
                Subcategorys[index] = e;
                SelectedSubcategory = e;
                OnPropertyChanged(nameof(Subcategorys));
            }
            CurrentViewModel = null;
        }

        private async Task DeleteSubcategory()
        {
            if (SelectedSubcategory != null)
            {
                // Показать всплывающее окно с подтверждением
                var result = MessageBox.Show("Вы уверены, что хотите удалить данную подкатегорию?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    SelectedSubcategory.Status = false;
                    await _subcategoryRepository.UpdateAsync(SelectedSubcategory);
                    Subcategorys.Remove(SelectedSubcategory);
                    SelectedSubcategory = null;
                    OnPropertyChanged(nameof(IsSubcategorySelected));
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
