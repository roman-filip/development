using AutoMapper;
using RFI.TimeTracker.ViewModels.Entities;
using Model = RFI.TimeTracker.Models;

namespace RFI.TimeTracker.ViewModels
{
    public static class ModuleController
    {
        public static void Init()
        {
            Mapper.CreateMap<Model.TimesheetEntry, TimesheetEntry>();
            Mapper.CreateMap<Model.Timesheet, Timesheet>();

            Mapper.CreateMap<TimesheetEntry, Model.TimesheetEntry>();
            Mapper.CreateMap<Timesheet, Model.Timesheet>();
        }
    }
}
