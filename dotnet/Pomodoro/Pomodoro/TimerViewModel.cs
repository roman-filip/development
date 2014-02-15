using System;
using System.ComponentModel;
using System.Timers;
using System.Windows.Input;
using RFI.Pomodoro.Helpers;
using RFI.Pomodoro.Properties;

namespace RFI.Pomodoro
{
    class TimerViewModel : INotifyPropertyChanged
    {
        private readonly TimeSpan _workDuration;

        private readonly TimeSpan _breakDuration;

        #region ActualTime
        private TimeSpan _actualTime;

        public TimeSpan ActualTime
        {
            get
            {
                return _actualTime;
            }
            set
            {
                _actualTime = value;
                RaisePropertyChanged("ActualTime");
            }
        }
        #endregion

        private Timer _timer;

        public TimerViewModel()
        {
            _workDuration = Settings.Default.WorkDuration;
            _breakDuration = Settings.Default.BreakDuration;
            ActualTime = _workDuration;

            ResetTimerCommand = new DelegateCommand(ResetTimerCommandExecuted, ResetTimerCommandCanExecute);
            StartPauseTimerCommand = new DelegateCommand(StartPauseTimerCommandExecuted, StartPauseTimerCommandCanExecute);

            _timer = new Timer(TimeSpan.FromSeconds(1).TotalMilliseconds);
            _timer.Elapsed += _timer_Elapsed;
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (ActualTime.TotalSeconds > 0)
            {
                ActualTime = ActualTime.Subtract(TimeSpan.FromSeconds(1));
            }
        }

        private void ResetTimer()
        {
            _timer.Stop();
            ActualTime = _workDuration;

            ((DelegateCommand)ResetTimerCommand).RasieCanExecuteChanged();
        }

        #region Commands

        #region ResetTimerCommand

        public ICommand ResetTimerCommand { get; private set; }

        public bool ResetTimerCommandCanExecute()
        {
            return true;
        }

        public void ResetTimerCommandExecuted()
        {
            ResetTimer();
        }

        #endregion

        #region StartPauseTimerCommand

        public ICommand StartPauseTimerCommand { get; private set; }

        public bool StartPauseTimerCommandCanExecute()
        {
            return true;
        }

        public void StartPauseTimerCommandExecuted()
        {
            if (_timer.Enabled)
            {
                _timer.Stop();
            }
            else
            {
                _timer.Start();
            }
        }

        #endregion

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
