using System;
using RFI.TimeTracker.Models;

namespace RFI.TimeTracker.ViewModels.Entities
{
    public class TimesheetEntry : BaseNotifyObject
    {
        private DateTime? _workStart;
        private DateTime? _workEnd;
        private DateTime? _lunchBreakStart;
        private DateTime? _lunchBreakEnd;
        private string _note;

        public DateTime? WorkStart
        {
            get { return _workStart; }
            set
            {
                if (SetPropertyValue(ref _workStart, value))
                {
                    OnPropertyChanged("WorkTime");
                }
            }
        }

        public DateTime? WorkEnd
        {
            get { return _workEnd; }
            set
            {
                if (SetPropertyValue(ref _workEnd, value))
                {
                    OnPropertyChanged("WorkTime");
                }
            }
        }

        public DateTime? LunchBreakStart
        {
            get { return _lunchBreakStart; }
            set
            {
                if (SetPropertyValue(ref _lunchBreakStart, value))
                {
                    OnPropertyChanged("WorkTime");
                }
            }
        }

        public DateTime? LunchBreakEnd
        {
            get { return _lunchBreakEnd; }
            set
            {
                if (SetPropertyValue(ref _lunchBreakEnd, value))
                {
                    OnPropertyChanged("WorkTime");
                }
            }
        }

        public string Note
        {
            get { return _note; }
            set { SetPropertyValue(ref _note, value); }
        }

        public TimeSpan? WorkTime
        {
            get
            {
                var workTime = WorkEnd - WorkStart;
                var lunchBreakTime = LunchBreakEnd - LunchBreakStart;
                return workTime - (lunchBreakTime ?? new TimeSpan());
            }
        }
    }
}
