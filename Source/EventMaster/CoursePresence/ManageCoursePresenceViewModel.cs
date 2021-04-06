using EventMaster._Helper;
using EventMaster.Course;
using EventMaster.Storage;
using EventMaster.Storage.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMaster.CoursePresence
{
    public class ManageCoursePresenceViewModel : INotifyPropertyChanged
    {
        private int selectedIndex;

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set {
                selectedIndex = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedIndex)));
            }
        }

        public ManageCoursePresenceViewModel()
        {
            AllCourses = new BindingList<CourseViewModel>(Workspace.CurrentData.Courses.Select(x => new CourseViewModel(x)).OrderBy(x => x.DisplayName).ToList());
            SelectedCourse = null;
        }

        public BindingList<CourseViewModel> AllCourses { get; set; }
        private CourseViewModel selectedCourse;

        public CourseViewModel SelectedCourse
        {
            get { return selectedCourse; }
            set
            {
                selectedCourse = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCourse)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsCourseSelected)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllPresences)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        public List<CoursePresenceViewModel> AllPresences
        {
            get
            {
                var allParticipants = Workspace.CurrentData.CourseParticipants.Where(x => x.CourseId == selectedCourse?.Id).ToList();
                return allParticipants.Select(x => new CoursePresenceViewModel(x)).Where(x =>
                {
                    try
                    {
                        x.Name.ToString();
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }).OrderBy(x => x.Name).ToList();
            }
        }

        public bool IsCourseSelected => SelectedCourse != null;
    }
}
