using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MoneyManager.ViewModels.DepositOperations;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MoneyManager.Views.DepositOperations
{
    /// <summary>
    /// Interaction logic for DateRangeSelectionView.xaml
    /// </summary>
    public partial class DateRangeSelectionView : UserControl
    {
        public DateRangeSelectionView()
        {
            InitializeComponent();
        }
        private void DateRangeCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is DateRangeSelectionViewModel viewModel)
            {
                if (DateRangeCalendar.SelectedDates.Count > 0)
                {
                    viewModel.StartDate = DateRangeCalendar.SelectedDates[0];
                    viewModel.EndDate = DateRangeCalendar.SelectedDates[DateRangeCalendar.SelectedDates.Count - 1];
                }
            }
        }
    }
}
