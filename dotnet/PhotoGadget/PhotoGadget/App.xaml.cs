using System.IO;
using System.Windows;
using System.Windows.Threading;

namespace PhotoGadget
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            File.AppendAllText("error.log", e.Exception.ToString());
        }
    }
}
