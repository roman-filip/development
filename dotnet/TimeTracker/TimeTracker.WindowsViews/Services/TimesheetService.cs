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
