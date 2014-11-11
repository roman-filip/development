using System.Windows;
using System.Windows.Data;
using RFI.TimeTracker.ViewModels;

namespace RFI.TimeTracker.WindowsViews.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel(new TimesheetService());
            Closing += ((MainViewModel)DataContext).OnWindowClosing;
        }
    }
}
