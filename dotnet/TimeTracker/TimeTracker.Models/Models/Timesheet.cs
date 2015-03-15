using System.Collections.Generic;

namespace RFI.TimeTracker.Models
{
    public class Timesheet
    {
        private List<TimesheetEntry> _timesheetEntries = new List<TimesheetEntry>();

        public int Month { get; set; }

        public int Year { get; set; }

        public List<TimesheetEntry> TimesheetEntries
        {
            get { return _timesheetEntries; }
            set { _timesheetEntries = value; }
        }
    }
}
