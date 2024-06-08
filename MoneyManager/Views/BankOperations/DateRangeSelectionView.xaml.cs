using MoneyManager.ViewModels.BankOperations;
using System.Windows.Controls;

namespace MoneyManager.Views.BankOperations
{
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
