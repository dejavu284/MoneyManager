using MoneyManager.Models;
using MoneyManager.Data.Repositories.Concrete;
using System;
using System.Windows.Input;
using System.ComponentModel;
using System.Threading.Tasks;

namespace MoneyManager.ViewModels.Categorys
{
    public class AddCategoryViewModel : INotifyPropertyChanged
    {
        private readonly CategoryRepository _categoryRepository;
        private Category _newCategory;

        public Category NewCategory
        {
            get => _newCategory;
            set
            {
                _newCategory = value;
                OnPropertyChanged(nameof(NewCategory));
            }
        }

        public ICommand AddCategoryCommand { get; }

        public event EventHandler<Category> CategoryAdded;

        public AddCategoryViewModel(CategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
            NewCategory = new Category();
            AddCategoryCommand = new RelayCommand(async _ => await AddCategory());
        }

        private async Task AddCategory()
        {
            NewCategory.Status = true;
            await _categoryRepository.AddAsync(NewCategory);
            CategoryAdded?.Invoke(this, NewCategory);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
