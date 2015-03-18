using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using AutoMapper;
using RFI.TimeTracker.ViewModels.Entities;
using RFI.TimeTracker.ViewModels.Interfaces;
using Model = RFI.TimeTracker.Models;

namespace RFI.TimeTracker.ViewModels
{
    public class MainViewModel : Model.BaseNotifyObject
    {
        private readonly ITimesheetService _timesheetService;
        private ObservableCollection<Timesheet> _timesheets;
        private Timesheet _selectedTimesheet;
        private Timer _actualDayWorkTimeTimer;

        #region Binded properties

        public ObservableCollection<Timesheet> Timesheets
        {
            get
            {
                if (_timesheets == null)
                {
                    var timesheetsModel = _timesheetService.LoadAllTimesheets();
                    var timesheets = Mapper.Map<List<Timesheet>>(timesheetsModel);

                    _timesheets = new ObservableCollection<Timesheet>(
                        timesheets
                        .OrderByDescending(timesheet => timesheet.Year)
                        .ThenByDescending(timesheet => timesheet.Month));

                    SelectedTimesheet = _timesheets.FirstOrDefault();
                }

                return _timesheets;
            }
        }

        public Timesheet SelectedTimesheet
        {
            get { return _selectedTimesheet; }
            set { SetPropertyValue(ref _selectedTimesheet, value); }
        }

        public string ActualDayWorkTime
        {
            get
            {
                const string emptyDayWorkTime = "00:00";
                if (ExistActualTimesheetEntry())
                {
                    var workTime = GetActualTimesheetEntry().CalculateWorkTime(GetActualTime());
                    return workTime.HasValue ? workTime.Value.ToString(@"hh\:mm") : emptyDayWorkTime;
                }
                else
                {
                    return emptyDayWorkTime;
                }
            }
        }

        #endregion

        #region Commands

        public ICommand StarWorkCommand { get; private set; }

        public ICommand EndWorkCommand { get; private set; }

        public ICommand StarLunchBreakCommand { get; private set; }

        public ICommand EndLunchBreakCommand { get; private set; }

        #endregion

        public MainViewModel(ITimesheetService timesheetService)
        {
            _timesheetService = timesheetService;

            InitCommands();
        }

        private void InitCommands()
        {
            StarWorkCommand = new DelegateCommand(OnStartWork);
            EndWorkCommand = new DelegateCommand(OnEndWork);
            StarLunchBreakCommand = new DelegateCommand(OnStarLunchBreak);
            EndLunchBreakCommand = new DelegateCommand(OnEndLunchBreak);
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            SaveChanges();
        }

        private void OnStartWork()
        {
            var actualTimesheetEntry = GetActualTimesheetEntry();
            actualTimesheetEntry.WorkStart = GetActualTime();

            SelectActualTimesheet();
            SaveChanges();

            _actualDayWorkTimeTimer = new Timer(state => OnPropertyChanged("ActualDayWorkTime"), null, TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(30));
        }

        private void OnEndWork()
        {
            var actualTimesheetEntry = GetActualTimesheetEntry();
            actualTimesheetEntry.WorkEnd = GetActualTime();

            SelectActualTimesheet();
            SaveChanges();

            if (_actualDayWorkTimeTimer != null)
            {
                _actualDayWorkTimeTimer.Dispose();
            }
        }

        private void OnStarLunchBreak()
        {
            var actualTimesheetEntry = GetActualTimesheetEntry();
            actualTimesheetEntry.LunchBreakStart = GetActualTime();

            SelectActualTimesheet();
            SaveChanges();
        }

        private void OnEndLunchBreak()
        {
            var actualTimesheetEntry = GetActualTimesheetEntry();
            actualTimesheetEntry.LunchBreakEnd = GetActualTime();

            SelectActualTimesheet();
            SaveChanges();
        }

        private TimesheetEntry GetActualTimesheetEntry()
        {
            var actualTime = GetActualTime();
            var actualTimesheet = GetActualTimesheet();

            var actualTimesheetEntry =
                actualTimesheet.TimesheetEntries.FirstOrDefault(te =>
                    (te.WorkStart.HasValue && te.WorkStart.Value.Day == actualTime.Day)
                    || (te.WorkEnd.HasValue && te.WorkEnd.Value.Day == actualTime.Day)
                    || (te.LunchBreakStart.HasValue && te.LunchBreakStart.Value.Day == actualTime.Day)
                    || (te.LunchBreakEnd.HasValue && te.LunchBreakEnd.Value.Day == actualTime.Day));
            if (actualTimesheetEntry == null)
            {
                actualTimesheetEntry = new TimesheetEntry();
                actualTimesheet.TimesheetEntries.Add(actualTimesheetEntry);
            }

            return actualTimesheetEntry;
        }

        private Timesheet GetActualTimesheet()
        {
            var actualTime = GetActualTime();

            var actualTimesheet =
                Timesheets.FirstOrDefault(timesheet => timesheet.Month == actualTime.Month && timesheet.Year == actualTime.Year);
            if (actualTimesheet == null)
            {
                actualTimesheet = new Timesheet
                {
                    Month = actualTime.Month,
                    Year = actualTime.Year
                };
                Timesheets.Insert(0, actualTimesheet);
            }

            return actualTimesheet;
        }

        private bool ExistActualTimesheetEntry()
        {
            var actualTime = GetActualTime();
            var actualTimesheet = GetActualTimesheet();

            return actualTimesheet.TimesheetEntries.Any(te =>
                (te.WorkStart.HasValue && te.WorkStart.Value.Day == actualTime.Day)
                || (te.WorkEnd.HasValue && te.WorkEnd.Value.Day == actualTime.Day)
                || (te.LunchBreakStart.HasValue && te.LunchBreakStart.Value.Day == actualTime.Day)
                || (te.LunchBreakEnd.HasValue && te.LunchBreakEnd.Value.Day == actualTime.Day));
        }

        private DateTime GetActualTime()
        {
            var actualTime = DateTime.Now;
            return new DateTime(actualTime.Year, actualTime.Month, actualTime.Day, actualTime.Hour, actualTime.Minute, 0);
        }

        private void SelectActualTimesheet()
        {
            SelectedTimesheet = null;
            SelectedTimesheet = _timesheets.First();
        }

        private void SaveChanges()
        {
            var timesheetsModel = Mapper.Map<List<Model.Timesheet>>(_timesheets);
            _timesheetService.SaveTimesheeets(timesheetsModel);
        }
    }
}
