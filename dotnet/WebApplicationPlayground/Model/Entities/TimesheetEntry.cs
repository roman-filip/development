using System;

namespace Model.Entities
{
    public class TimesheetEntry
    {
        public int Id { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public string Note { get; set; }
    }
}
