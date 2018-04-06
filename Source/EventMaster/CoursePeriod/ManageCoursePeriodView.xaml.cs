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

namespace EventMaster.CoursePeriod
{
    /// <summary>
    /// Interaction logic for ManageCoursePeriodView.xaml
    /// </summary>
    public partial class ManageCoursePeriodView : Page
    {
        public ManageCoursePeriodView()
        {
            InitializeComponent();
            this.DataContext = new ManageCoursePeriodViewModel();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var newSelectedItem = e.AddedItems[0] as CoursePeriodViewModel;
                ((ManageCoursePeriodViewModel)this.DataContext).SelectedCoursePeriod = newSelectedItem;
            }
            else
            {
                ((ManageCoursePeriodViewModel)this.DataContext).SelectedCoursePeriod = null;
            }
        }
    }
}
