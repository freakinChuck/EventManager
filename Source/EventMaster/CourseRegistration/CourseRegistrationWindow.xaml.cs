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
using System.Windows.Shapes;
using EventMaster.Participant;

namespace EventMaster.CourseRegistration
{
    /// <summary>
    /// Interaction logic for CourseRegistrationWindow.xaml
    /// </summary>
    public partial class CourseRegistrationWindow : Window
    {
        public CourseRegistrationWindow()
        {
            InitializeComponent();
            this.DataContext = new CourseRegistrationViewModel();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var newSelectedItem = e.AddedItems[0] as ParticipantViewModel;
                ((CourseRegistrationViewModel)this.DataContext).SelectedParticipant = newSelectedItem;
            }
            else
            {
                ((CourseRegistrationViewModel)this.DataContext).SelectedParticipant = null;
            }
        }
    }
}
