using System;
using System.ComponentModel;
using System.Windows.Input;

namespace MoneyManager.ViewModels.DepositOperations
{
    public class DateRangeSelectedEventArgs : EventArgs
    {
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }

        public DateRangeSelectedEventArgs(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }
    }

    public class DateRangeSelectionViewModel : INotifyPropertyChanged
    {
        private DateTime? _startDate;
        private DateTime? _endDate;

        public DateTime? StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }

        public DateTime? EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }

        public ICommand ApplyDateRangeCommand { get; }

        public event EventHandler<DateRangeSelectedEventArgs> DateRangeSelected;

        public DateRangeSelectionViewModel()
        {
            ApplyDateRangeCommand = new RelayCommand(_ => ApplyDateRange());
        }

        private void ApplyDateRange()
        {
            if (StartDate.HasValue && EndDate.HasValue && DateRangeSelected != null)
            {
                DateRangeSelected(this, new DateRangeSelectedEventArgs(StartDate.Value, EndDate.Value));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
