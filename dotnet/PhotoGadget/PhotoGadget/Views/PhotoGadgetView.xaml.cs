using System.Windows;
using System.Windows.Input;

namespace PhotoGadget.Views
{
    /// <summary>
    /// Interaction logic for PhotoGadgetView.xaml
    /// </summary>
    public partial class PhotoGadgetView : Window
    {
        public PhotoGadgetView()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }
    }
}
