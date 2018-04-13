using EventMaster._Helper;
using EventMaster.Storage;
using EventMaster.Storage.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EventMaster.Course
{
    public class ManageCourseViewModel : INotifyPropertyChanged
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

        public ManageCourseViewModel()
        {
            AllCourses = new BindingList<CourseViewModel>(Workspace.CurrentData.Courses.Select(x => new CourseViewModel(x)).ToList());
            AllEmployees = new BindingList<IdNameHelper>(Workspace.CurrentData.Employees.Select(x => new IdNameHelper() { Id = x.Id, Name = $"{x.Firstname} {x.Name}" }).OrderBy(x => x.Name).ToList());
            AllCourseTypes = new BindingList<IdNameHelper>(Workspace.CurrentData.CourseTypes.Select(x => new IdNameHelper() { Id = x.Id, Name = x.TypeName }).OrderBy(x => x.Name).ToList());
            AllPeriods = new BindingList<IdNameHelper>(Workspace.CurrentData.CoursePeriods.Select(x => new IdNameHelper() { Id = x.Id, Name = x.PeriodName }).OrderBy(x => x.Name).ToList());
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
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public BindingCommand NewCourseCommand
        {
            get { return new BindingCommand(x => AddNewCourse()); }
        }
        private void AddNewCourse()
        {
            this.AllCourses.Add(new CourseViewModel(CourseModel.CreateNewCourse()));
            Workspace.RegisterDataChanged();
            SelectedIndex = this.AllCourses.Count - 1;
        }

        public BindingCommand RemoveCourseCommand
        {
            get { return new BindingCommand(x => RemoveCourse()); }
        }
        private void RemoveCourse()
        {
            var selectedCourse = SelectedCourse;
            this.AllCourses.Remove(selectedCourse);
            //SelectedIndex = 0;
            selectedCourse.RemoveCourseFromModel();
        }

        public CourseParticipantListViewModel SelectedCourseParticipation { get; set; }

        public BindingCommand UnregisterCommand
        {
            get { return new BindingCommand(x => Unregister()); }
        }
        private void Unregister()
        {
            var selectedCourseParticipation = SelectedCourseParticipation;
            if (selectedCourseParticipation != null && SelectedCourse != null)
            {
                var result = MessageBox.Show($"Möchten Sie { selectedCourseParticipation.Vorname } { selectedCourseParticipation.Nachname }  wirklich vom Kurs '{ SelectedCourse.DisplayName }' abmelden?", "Abmeldung", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var registration = Workspace.CurrentData.CourseParticipants.Where(x => x.Id == selectedCourseParticipation.AnmeldungsId).FirstOrDefault();
                    if (registration != null)
                    {
                        Workspace.CurrentData.CourseParticipants.Remove(registration);
                        Workspace.RegisterDataChanged();
                        SelectedCourse = this.SelectedCourse;
                    }
                }
            }
        }

        public bool IsCourseSelected => SelectedCourse != null;

        public BindingList<IdNameHelper> AllEmployees { get; set; }
        public BindingList<IdNameHelper> AllCourseTypes { get; set; }
        public BindingList<IdNameHelper> AllPeriods { get; set; }

    }
}
