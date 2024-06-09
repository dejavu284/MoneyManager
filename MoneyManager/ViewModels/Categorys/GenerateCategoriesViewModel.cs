using MoneyManager.Models;
using MoneyManager.Data.Repositories.Concrete;
using System.ComponentModel;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System;
using MoneyManager.Models.Generators;
using System.Linq;
using System.Windows;
using MoneyManager.ViewModels.Categorys;

namespace MoneyManager.ViewModels.Categorys
{
    public class GenerateCategoriesViewModel : INotifyPropertyChanged
    {
        private int _numberOfCategories;

        public int NumberOfCategories
        {
            get => _numberOfCategories;
            set
            {
                _numberOfCategories = value;
                OnPropertyChanged(nameof(NumberOfCategories));
            }
        }

        public ICommand GenerateCategoriesCommand { get; }

        private readonly CategoryRepository _categoryRepository;
        private readonly CategoryViewModel _parentViewModel;

        public GenerateCategoriesViewModel(CategoryRepository categoryRepository, CategoryViewModel parentViewModel)
        {
            _categoryRepository = categoryRepository;
            _parentViewModel = parentViewModel;

            GenerateCategoriesCommand = new RelayCommand(async _ => await GenerateCategories());
        }

        private async Task GenerateCategories()
        {
            await Task.Run(async () =>
            {
                for (int i = 0; i < NumberOfCategories; i++)
                {
                    var category = new CategoryBuilder()
                        .SetName($"Категория {i + 1}")
                        .Build();

                    await _categoryRepository.AddAsync(category);

                    // Обновляем коллекцию категорий в UI-потоке
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        _parentViewModel.Categories.Add(category);
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
