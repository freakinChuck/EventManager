using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EventMaster
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenEmployeeRibbonButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = new Employee.ManageEmployeeView();
        }

        private void ContentFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            e.Cancel = false;
        }
    }
}
