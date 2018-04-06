using EventMaster._Helper;
using EventMaster.Storage;
using EventMaster.Storage.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMaster.CoursePeriod
{
    public class ManageCoursePeriodViewModel : INotifyPropertyChanged
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

        public ManageCoursePeriodViewModel()
        {
            AllCoursePeriods = new BindingList<CoursePeriodViewModel>(Workspace.CurrentData.CoursePeriods.Select(x => new CoursePeriodViewModel(x)).ToList());
            SelectedCoursePeriod = null;
        }

        public BindingList<CoursePeriodViewModel> AllCoursePeriods { get; set; }
        private CoursePeriodViewModel selectedCoursePeriod;

        public CoursePeriodViewModel SelectedCoursePeriod
        {
            get { return selectedCoursePeriod; }
            set
            {
                selectedCoursePeriod = value;
                LoadPeriods();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCoursePeriod)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsCoursePeriodSelected)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public BindingCommand NewCoursePeriodCommand
        {
            get { return new BindingCommand(x => AddNewCoursePeriod()); }
        }
        private void AddNewCoursePeriod()
        {
            this.AllCoursePeriods.Add(new CoursePeriodViewModel(CoursePeriodModel.CreateNewCoursePeriod()));
            Workspace.RegisterDataChanged();
            SelectedIndex = this.AllCoursePeriods.Count - 1;
        }

        public BindingCommand RemoveCoursePeriodCommand
        {
            get { return new BindingCommand(x => RemoveCoursePeriod()); }
        }
        private void RemoveCoursePeriod()
        {
            var selectedCoursePeriod = SelectedCoursePeriod;
            this.AllCoursePeriods.Remove(selectedCoursePeriod);
            //SelectedIndex = 0;
            selectedCoursePeriod.RemoveCoursePeriodFromModel();
        }

        public bool IsCoursePeriodSelected => SelectedCoursePeriod != null;

        public BindingList<CourseTypeViewModel> AllCourseTypes
        {
            get; set;
        }
        private void LoadPeriods()
        {
            var period = this.SelectedCoursePeriod;
            if (period == null)
            {
                this.AllCourseTypes = new BindingList<CourseTypeViewModel>();
            }
            else
            {
                this.AllCourseTypes = new BindingList<CourseTypeViewModel>(Workspace.CurrentData.CourseTypes.OrderBy(x => x.TypeName).Select(x => new CourseTypeViewModel(SelectedCoursePeriod.storageCoursePeriod, x)).ToList());
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllCourseTypes)));
        }
    }
}
