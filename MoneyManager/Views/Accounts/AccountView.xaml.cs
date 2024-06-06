using MoneyManager.ViewModels.Accounts;
using System.Windows.Controls;

namespace MoneyManager.Views.Accounts
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
