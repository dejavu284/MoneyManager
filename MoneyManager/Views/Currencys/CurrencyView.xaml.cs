using MoneyManager.ViewModels.Currencys;
using System.Windows.Controls;

namespace MoneyManager.Views.Currencys
{
    public partial class CurrencyView : UserControl
    {
        public CurrencyView()
        {
            InitializeComponent();
            DataContext = new CurrencyViewModel();
        }
    }

}
