using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using RFI.TimeTracker.Models;
using RFI.TimeTracker.ViewModels.Interfaces;
using RFI.TimeTracker.WindowsViews.Properties;

namespace RFI.TimeTracker.WindowsViews
{
    public class TimesheetService : ITimesheetService
    {
        public List<Timesheet> LoadAllTimesheets()
        {
            if (!File.Exists(Settings.Default.XMLFileName))
            {
                return new List<Timesheet>();
            }

            var serializer = new XmlSerializer(typeof(List<Timesheet>));
            using (Stream reader = new FileStream(Settings.Default.XMLFileName, FileMode.Open))
            {
                var timesheets = (List<Timesheet>)serializer.Deserialize(reader);
                return timesheets;
            }



            //var timesheets = new List<Timesheet>()
            //{
            //    new Timesheet {Month = 9, Year = 2014},
            //    new Timesheet {Month = 10, Year = 2014},
            //    new Timesheet {Month = 11, Year = 2014},
            //};
            //timesheets[2].TimesheetEntries.Add(new TimesheetEntry { WorkStart = new DateTime(2014, 11, 1, 8, 0, 0), WorkEnd = new DateTime(2014, 11, 1, 18, 0, 0) });
            //timesheets[2].TimesheetEntries.Add(new TimesheetEntry { WorkStart = new DateTime(2014, 11, 2, 7, 0, 0), WorkEnd = new DateTime(2014, 11, 2, 18, 0, 0), LunchBreakStart = new DateTime(2014, 11, 2, 12, 0, 0), LunchBreakEnd = new DateTime(2014, 11, 2, 12, 30, 0) });
            //timesheets[2].TimesheetEntries.Add(new TimesheetEntry { WorkStart = new DateTime(2014, 11, 3, 8, 0, 0), WorkEnd = new DateTime(2014, 11, 3, 12, 0, 0), Note = "Bernardyn v Titaniu" });
            //timesheets[2].TimesheetEntries.Add(new TimesheetEntry { WorkStart = new DateTime(2014, 11, 4, 8, 0, 0), WorkEnd = new DateTime(2014, 11, 4, 17, 0, 0) });
            //timesheets[2].TimesheetEntries.Add(new TimesheetEntry { WorkStart = new DateTime(2014, 11, 5, 8, 0, 0), WorkEnd = new DateTime(2014, 11, 5, 18, 0, 0), LunchBreakStart = new DateTime(2014, 11, 5, 11, 30, 0), LunchBreakEnd = new DateTime(2014, 11, 5, 12, 30, 0) });
            //return timesheets;
        }

        public void SaveTimesheeets(List<Timesheet> timesheets)
        {
            var serializer = new XmlSerializer(timesheets.GetType());
            using (TextWriter streamWriter = new StreamWriter(Settings.Default.XMLFileName))
            {
                serializer.Serialize(streamWriter, timesheets);
            }
        }
    }
}
