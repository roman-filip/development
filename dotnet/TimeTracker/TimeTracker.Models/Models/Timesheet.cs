using System;
using System.Collections.Generic;

namespace RFI.TimeTracker.Models
{
    public class Timesheet : BaseNotifyObject
    {
        private List<TimesheetEntry> _timesheetEntries = new List<TimesheetEntry>();

        public int Month { get; set; }

        public int Year { get; set; }

        public List<TimesheetEntry> TimesheetEntries
        {
            get { return _timesheetEntries; }
            set { _timesheetEntries = value; }
        }

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
    }
}
