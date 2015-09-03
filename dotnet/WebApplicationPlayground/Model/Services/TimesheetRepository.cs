using System;
using System.Collections.Generic;
using Model.Entities;

namespace Model.Services
{
    public class TimesheetRepository : ITimesheetRepository
    {
        public IEnumerable<Timesheet> GetAllItems()
        {
            return new List<Timesheet>
            {
                new Timesheet
                {
                    Id = 1,
                    Month = 1,
                    Year = 2015,
                    TimeEntries = new List<TimesheetEntry>
                    {
                        new TimesheetEntry { Id = 1, StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1), Note = "test" },
                        new TimesheetEntry { Id = 2, StartTime = DateTime.Now.AddDays(1), EndTime = DateTime.Now.AddDays(1).AddHours(1), Note = "test2" }
                    }
                },
                new Timesheet
                {
                    Id = 2,
                    Month = 2,
                    Year = 2015,
                    TimeEntries = new List<TimesheetEntry>
                    {
                        new TimesheetEntry { Id = 1, StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1), Note = "test" },
                    }
                }
            };
        }

        public Timesheet GetItemById(int id)
        {
            return new Timesheet
            {
                Id = 1,
                Month = 1,
                Year = 2015,
                TimeEntries = new List<TimesheetEntry>
                {
                    new TimesheetEntry
                    {
                        Id = 1,
                        StartTime = DateTime.Now,
                        EndTime = DateTime.Now.AddHours(1),
                        Note = "test"
                    },
                    new TimesheetEntry
                    {
                        Id = 2,
                        StartTime = DateTime.Now.AddDays(1),
                        EndTime = DateTime.Now.AddDays(1).AddHours(1),
                        Note = "test2"
                    }
                }
            };
        }

        public void Save(Timesheet entity)
        {
            throw new NotImplementedException();
        }
    }
}
