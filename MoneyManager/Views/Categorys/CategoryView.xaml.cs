using MoneyManager.ViewModels.Categorys;
using System.Windows.Controls;

namespace MoneyManager.Views.Categorys
{
    public partial class CategoryView : UserControl
    {
        public CategoryView()
        {
            InitializeComponent();
            DataContext = new CategoryViewModel();
        }
    }

}
