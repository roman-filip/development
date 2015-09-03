using System.Collections.Generic;

namespace Model.Entities
{
    public class Timesheet
    {
        public int Id { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public IEnumerable<TimesheetEntry> TimeEntries { get; set; }
    }
}
