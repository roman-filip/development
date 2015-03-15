using System;

namespace RFI.TimeTracker.Models
{
    public class TimesheetEntry
    {
        public DateTime? WorkStart { get; set; }

        public DateTime? WorkEnd { get; set; }

        public DateTime? LunchBreakStart { get; set; }

        public DateTime? LunchBreakEnd { get; set; }

        public string Note { get; set; }
    }
}
