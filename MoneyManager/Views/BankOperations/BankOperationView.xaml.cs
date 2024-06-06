using MoneyManager.ViewModels.BankOperations;
using System.Windows.Controls;

namespace MoneyManager.Views.BankOperations
{
    public partial class BankOperationView : UserControl
    {
        public BankOperationView()
        {
            InitializeComponent();
            DataContext = new BankOperationViewModel();
        }
    }
}
