using MoneyManager.ViewModels;
using System.Windows.Controls;

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
