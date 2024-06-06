using MoneyManager.Models;
using MoneyManager.Data.Repositories.Concrete;
using System.ComponentModel;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System;

namespace MoneyManager.ViewModels.Subcategories
{
    public class EditSubcategoryViewModel : INotifyPropertyChanged
    {
        private readonly SubcategoryRepository _subcategoryRepository;
        private readonly CategoryRepository _categoryRepository;
        private Subcategory _selectedSubcategory;
        private ObservableCollection<Category> _categories;

        public Subcategory SelectedSubcategory
        {
            get => _selectedSubcategory;
            set
            {
                _selectedSubcategory = value;
                OnPropertyChanged(nameof(SelectedSubcategory));
            }
        }

        public ObservableCollection<Category> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }

        public ICommand UpdateSubcategoryCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public EditSubcategoryViewModel(SubcategoryRepository subcategoryRepository, CategoryRepository categoryRepository, Subcategory selectedSubcategory)
        {
            _subcategoryRepository = subcategoryRepository;
            _categoryRepository = categoryRepository;
            SelectedSubcategory = selectedSubcategory;
            UpdateSubcategoryCommand = new RelayCommand(async _ => await UpdateSubcategory());

            LoadCategories();
        }

        private async Task UpdateSubcategory()
        {
            await _subcategoryRepository.UpdateAsync(SelectedSubcategory);
            SubcategoryUpdated?.Invoke(this, SelectedSubcategory);
        }

        private async void LoadCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();
            Categories = new ObservableCollection<Category>(categories);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event EventHandler<Subcategory> SubcategoryUpdated;
    }
}
