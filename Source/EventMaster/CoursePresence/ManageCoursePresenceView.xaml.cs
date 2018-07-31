using EventMaster.Course;
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

namespace EventMaster.CoursePresence
{
    /// <summary>
    /// Interaction logic for ManageCoursePresenceView.xaml
    /// </summary>
    public partial class ManageCoursePresenceView : Page
    {
        public ManageCoursePresenceView()
        {
            InitializeComponent();
            this.DataContext = new ManageCoursePresenceViewModel();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var newSelectedItem = e.AddedItems[0] as CourseViewModel;
                ((ManageCoursePresenceViewModel)this.DataContext).SelectedCourse = newSelectedItem;
            }
            else
            {
                ((ManageCoursePresenceViewModel)this.DataContext).SelectedCourse = null;
            }
        }
    }
}
