using MoneyManager.ViewModels.Deposits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MoneyManager.Views.Deposits
{
    /// <summary>
    /// Interaction logic for DepositView.xaml
    /// </summary>
    public partial class DepositView : UserControl
    {
        public DepositView()
        {
            InitializeComponent();
            DataContext = new DepositViewModel();
        }
    }
}
