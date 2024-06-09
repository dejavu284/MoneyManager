using MoneyManager.Data.Repositories.Concrete;
using MoneyManager.Models.Generators;
using MoneyManager.Models;
using MoneyManager.ViewModels.Subcategories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Collections.ObjectModel;

namespace MoneyManager.ViewModels.Subcategories
{
    public class GenerateSubcategoriesViewModel : INotifyPropertyChanged
    {
        private int _numberOfSubcategories;
        private Category _selectedCategory;
        private ObservableCollection<Category> _categories;

        public ObservableCollection<Category> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }

        public int NumberOfSubcategories
        {
            get => _numberOfSubcategories;
            set
            {
                _numberOfSubcategories = value;
                OnPropertyChanged(nameof(NumberOfSubcategories));
            }
        }

        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
            }
        }

        public ICommand GenerateSubcategoriesCommand { get; }

        private readonly SubcategoryRepository _subcategoryRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly SubcategoryViewModel _parentViewModel;

        public GenerateSubcategoriesViewModel(SubcategoryRepository subcategoryRepository, CategoryRepository categoryRepository, SubcategoryViewModel parentViewModel)
        {
            _subcategoryRepository = subcategoryRepository;
            _categoryRepository = categoryRepository;
            _parentViewModel = parentViewModel;

            GenerateSubcategoriesCommand = new RelayCommand(async _ => await GenerateSubcategories());

            LoadCategories();
        }

        private async void LoadCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();
            Categories = new ObservableCollection<Category>(categories);
        }

        private async Task GenerateSubcategories()
        {
            if (SelectedCategory == null)
            {
                MessageBox.Show("Пожалуйста, выберите категорию.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            await Task.Run(async () =>
            {
                for (int i = 0; i < NumberOfSubcategories; i++)
                {
                    var subcategoryName = $"{SelectedCategory.CategoryName}_подкатегория_{i + 1}";
                    var subcategory = new SubcategoryBuilder()
                        .SetName(subcategoryName)
                        .SetCategory(SelectedCategory)
                        .Build();

                    await _subcategoryRepository.AddAsync(subcategory);

                    Application.Current?.Dispatcher.Invoke(() =>
                    {
                        _parentViewModel.Subcategories.Add(subcategory);
                    });
                }
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
