using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfValidations
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ((ViewModel)this.DataContext).ValidationCompleted += (sender, args) => MessageBox.Show(args.Message);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.tbTest.GetBindingExpression(TextBox.ToolTipProperty).UpdateSource();
        }
    }
}
