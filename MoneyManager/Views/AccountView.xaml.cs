using System.Windows.Controls;
using MoneyManager.ViewModels;

namespace MoneyManager.Views
{
    public partial class AccountView : UserControl
    {
        public AccountView()
        {
            InitializeComponent();
            DataContext = new AccountViewModel();
        }
    }
}
