using MoneyManager.Models;
using MoneyManager.Data.Repositories.Concrete;
using System;
using System.Windows.Input;
using System.ComponentModel;
using System.Threading.Tasks;

namespace MoneyManager.ViewModels.Categorys
{
    public class EditCategoryViewModel : INotifyPropertyChanged
    {
        private readonly CategoryRepository _categoryRepository;
        private Category _editedCategory;

        public Category EditedCategory
        {
            get => _editedCategory;
            set
            {
                _editedCategory = value;
                OnPropertyChanged(nameof(EditedCategory));
            }
        }

        public ICommand EditCategoryCommand { get; }

        public event EventHandler<Category> CategoryUpdated;

        public EditCategoryViewModel(CategoryRepository categoryRepository, Category category)
        {
            _categoryRepository = categoryRepository;
            EditedCategory = category;
            EditCategoryCommand = new RelayCommand(async _ => await EditCategory());
        }

        private async Task EditCategory()
        {
            await _categoryRepository.UpdateAsync(EditedCategory);
            CategoryUpdated?.Invoke(this, EditedCategory);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
