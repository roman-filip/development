using System.Collections.Generic;
using RFI.TimeTracker.Models;

namespace RFI.TimeTracker.ViewModels.Interfaces
{
    public interface ITimesheetService
    {
        List<Timesheet> LoadAllTimesheets();

        void SaveTimesheeets(List<Timesheet> timesheets);
    }
}
