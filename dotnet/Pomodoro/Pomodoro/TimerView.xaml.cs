using RFI.Pomodoro.Properties;
using System.Windows;
using System.Windows.Input;

namespace RFI.Pomodoro
{
    /// <summary>
    /// Interaction logic for TimerView.xaml
    /// </summary>
    public partial class TimerView : Window
    {
        public TimerView()
        {
            InitializeComponent();

            this.Left = Settings.Default.StartupLocation.X;
            this.Top = Settings.Default.StartupLocation.Y;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
