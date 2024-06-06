using MoneyManager.Models;
using MoneyManager.Data.Repositories.Concrete;
using System.ComponentModel;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System;

namespace MoneyManager.ViewModels.Subcategories
{
    public class AddSubcategoryViewModel : INotifyPropertyChanged
    {
        private readonly SubcategoryRepository _subcategoryRepository;
        private readonly CategoryRepository _categoryRepository;
        private Subcategory _newSubcategory;
        private ObservableCollection<Category> _categories;

        public Subcategory NewSubcategory
        {
            get => _newSubcategory;
            set
            {
                _newSubcategory = value;
                OnPropertyChanged(nameof(NewSubcategory));
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

        public ICommand AddSubcategoryCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public AddSubcategoryViewModel(SubcategoryRepository subcategoryRepository, CategoryRepository categoryRepository)
        {
            _subcategoryRepository = subcategoryRepository;
            _categoryRepository = categoryRepository;
            NewSubcategory = new Subcategory();
            AddSubcategoryCommand = new RelayCommand(async _ => await AddSubcategory());

            LoadCategories();
        }

        private async Task AddSubcategory()
        {
            await _subcategoryRepository.AddAsync(NewSubcategory);
            SubcategoryAdded?.Invoke(this, NewSubcategory);
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

        public event EventHandler<Subcategory> SubcategoryAdded;
    }
}
