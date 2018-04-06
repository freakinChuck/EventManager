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

namespace EventMaster.Course
{
    /// <summary>
    /// Interaction logic for ManageCourseView.xaml
    /// </summary>
    public partial class ManageCourseView : Page
    {
        public ManageCourseView()
        {
            InitializeComponent();
            this.DataContext = new ManageCourseViewModel();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var newSelectedItem = e.AddedItems[0] as CourseViewModel;
                ((ManageCourseViewModel)this.DataContext).SelectedCourse = newSelectedItem;
            }
            else
            {
                ((ManageCourseViewModel)this.DataContext).SelectedCourse = null;
            }
        }
    }
}
