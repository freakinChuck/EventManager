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

namespace EventMaster.Employee
{
    /// <summary>
    /// Interaction logic for ManageEmployeeView.xaml
    /// </summary>
    public partial class ManageEmployeeView : Page
    {
        public ManageEmployeeView()
        {
            InitializeComponent();
            this.DataContext = new ManageEmployeeViewModel();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var newSelectedItem = e.AddedItems[0] as EmployeeViewModel;
            ((ManageEmployeeViewModel)this.DataContext).SelectedEmployee = newSelectedItem;
        }
    }
}
