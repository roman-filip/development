using System.Windows;
using RFI.TimeTracker.ViewModels;

namespace TimeTracker.WindowsViews
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Overrides of Application

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ModuleController.Init();
        }

        #endregion
    }
}
