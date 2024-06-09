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
        private ObservableCollection<Subcategory> _subcategories;
        private Subcategory _selectedSubcategory;
        private object _currentViewModel;

        public ObservableCollection<Subcategory> Subcategories
        {
            get => _subcategories;
            set
            {
                _subcategories = value;
                OnPropertyChanged(nameof(Subcategories));
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
        public ICommand ShowGenerateSubcategoriesViewCommand { get; }

        public SubcategoryViewModel()
        {
            var context = CreateDbContext();
            _subcategoryRepository = new SubcategoryRepository(context);
            _categoryRepository = new CategoryRepository(context);

            ShowAddSubcategoryViewCommand = new RelayCommand(_ => ShowAddSubcategoryView());
            EditSubcategoryCommand = new RelayCommand(_ => ShowEditSubcategoryView(), _ => IsSubcategorySelected);
            DeleteSubcategoryCommand = new RelayCommand(async _ => await DeleteSubcategory(), _ => IsSubcategorySelected);
            ShowGenerateSubcategoriesViewCommand = new RelayCommand(_ => ShowGenerateSubcategoriesView());

            LoadSubcategories();

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

        private async void LoadSubcategories()
        {
            var subcategories = await _subcategoryRepository.GetAllAsync();
            Subcategories = new ObservableCollection<Subcategory>(subcategories);
        }

        private void ShowAddSubcategoryView()
        {
            var addSubcategoryViewModel = new AddSubcategoryViewModel(_subcategoryRepository, _categoryRepository);
            addSubcategoryViewModel.SubcategoryAdded += AddSubcategoryViewModel_SubcategoryAdded;
            CurrentViewModel = addSubcategoryViewModel;
        }

        private void ShowGenerateSubcategoriesView()
        {
            var generateSubcategoriesViewModel = new GenerateSubcategoriesViewModel(_subcategoryRepository, _categoryRepository, this);
            CurrentViewModel = generateSubcategoriesViewModel;
        }

        private void AddSubcategoryViewModel_SubcategoryAdded(object sender, Subcategory e)
        {
            Subcategories.Add(e);
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
            var index = Subcategories.IndexOf(SelectedSubcategory);
            if (index >= 0)
            {
                Subcategories[index] = e;
                SelectedSubcategory = e;
                OnPropertyChanged(nameof(Subcategories));
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
                    await _subcategoryRepository.DeleteAsync(SelectedSubcategory.SubcategoryId);
                    Subcategories.Remove(SelectedSubcategory);
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
