using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using RFI.TimeTracker.Models;

namespace RFI.TimeTracker.ViewModels.Entities
{
    public class Timesheet : BaseNotifyObject
    {
        private readonly ObservableCollection<TimesheetEntry> _timesheetEntries;

        public int Month { get; set; }

        public int Year { get; set; }

        public ObservableCollection<TimesheetEntry> TimesheetEntries { get { return _timesheetEntries; } }

        public TimeSpan TotalWorkTime
        {
            get
            {
                var totalWorkTime = new TimeSpan();
                foreach (TimesheetEntry timesheetEntry in TimesheetEntries)
                {
                    totalWorkTime += timesheetEntry.WorkTime ?? new TimeSpan();
                }

                return totalWorkTime;
            }
        }

        public string Overview
        {
            get
            {
                if (Year > 0 && Month > 0)
                {
                    var tmpDate = new DateTime(Year, Month, 1);
                    return string.Format("{0} ({1} / {2})",
                        tmpDate.ToString("MMMM yyyy"),
                        Math.Round(TotalWorkTime.TotalHours, 1),
                        TimesheetEntries.Count * 8);
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public Timesheet()
        {
            _timesheetEntries = new ObservableCollection<TimesheetEntry>();
            _timesheetEntries.CollectionChanged += TimesheetEntriesOnCollectionChanged;
        }

        private void TimesheetEntriesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            Debug.WriteLine(sender + " " + args.Action);

            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (TimesheetEntry newTimesheetEntry in args.NewItems)
                    {
                        newTimesheetEntry.PropertyChanged += TimesheetEntryOnPropertyChanged;
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (TimesheetEntry removedTimesheetEntry in args.OldItems)
                    {
                        removedTimesheetEntry.PropertyChanged -= TimesheetEntryOnPropertyChanged;
                    }
                    break;
            }

            OnPropertyChanged("TimesheetEntries");
            OnPropertyChanged("Overview");
        }

        private void TimesheetEntryOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            Debug.WriteLine(sender + " " + args.PropertyName);

            if (args.PropertyName == "WorkTime")
            {
                OnPropertyChanged("Overview");
            }
        }
    }
}
