using MoneyManager.ViewModels.Subcategories;
using System.Windows.Controls;

namespace MoneyManager.Views.Subcategories
{
    public partial class SubcategoryView : UserControl
    {
        public SubcategoryView()
        {
            InitializeComponent();
            DataContext = new SubcategoryViewModel();
        }
    }
}
