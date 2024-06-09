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
using MoneyManager.ViewModels.Categorys;

namespace MoneyManager.ViewModels.Categorys
{
    public class CategoryViewModel : INotifyPropertyChanged
    {
        private readonly CategoryRepository _categoryRepository;
        private ObservableCollection<Category> _categories;
        private Category _selectedCategory;
        private object _currentViewModel;

        public ObservableCollection<Category> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }

        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
                OnPropertyChanged(nameof(IsCategorySelected));
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

        public bool IsCategorySelected => SelectedCategory != null;

        public ICommand ShowGenerateCategoriesViewCommand { get; }
        public ICommand ShowAddCategoryViewCommand { get; }
        public ICommand EditCategoryCommand { get; }
        public ICommand DeleteCategoryCommand { get; }

        public CategoryViewModel()
        {
            var context = CreateDbContext();
            _categoryRepository = new CategoryRepository(context);

            ShowGenerateCategoriesViewCommand = new RelayCommand(_ => ShowGenerateCategoriesView());
            ShowAddCategoryViewCommand = new RelayCommand(_ => ShowAddCategoryView());
            EditCategoryCommand = new RelayCommand(_ => ShowEditCategoryView(), _ => IsCategorySelected);
            DeleteCategoryCommand = new RelayCommand(async _ => await DeleteCategory(), _ => IsCategorySelected);

            LoadCategories();

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

        private async void LoadCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();
            Categories = new ObservableCollection<Category>(categories);
        }

        private void ShowGenerateCategoriesView()
        {
            var generateCategoriesViewModel = new GenerateCategoriesViewModel(_categoryRepository, this);
            CurrentViewModel = generateCategoriesViewModel;
        }

        private void ShowAddCategoryView()
        {
            var addCategoryViewModel = new AddCategoryViewModel(_categoryRepository);
            addCategoryViewModel.CategoryAdded += AddCategoryViewModel_CategoryAdded;
            CurrentViewModel = addCategoryViewModel;
        }

        private void AddCategoryViewModel_CategoryAdded(object sender, Category e)
        {
            Categories.Add(e);
            CurrentViewModel = null;
        }

        private void ShowEditCategoryView()
        {
            var editCategoryViewModel = new EditCategoryViewModel(_categoryRepository, SelectedCategory);
            editCategoryViewModel.CategoryUpdated += EditCategoryViewModel_CategoryUpdated;
            CurrentViewModel = editCategoryViewModel;
        }

        private void EditCategoryViewModel_CategoryUpdated(object sender, Category e)
        {
            var index = Categories.IndexOf(SelectedCategory);
            if (index >= 0)
            {
                Categories[index] = e;
                SelectedCategory = e;
                OnPropertyChanged(nameof(Categories));
            }
            CurrentViewModel = null;
        }

        private async Task DeleteCategory()
        {
            if (SelectedCategory != null)
            {
                // Показать всплывающее окно с подтверждением
                var result = MessageBox.Show("Вы уверены, что хотите удалить данную категорию?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    await _categoryRepository.DeleteAsync(SelectedCategory.CategoryId);
                    Categories.Remove(SelectedCategory);
                    SelectedCategory = null;
                    OnPropertyChanged(nameof(IsCategorySelected));
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
