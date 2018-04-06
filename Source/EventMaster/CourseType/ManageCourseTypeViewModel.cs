using EventMaster._Helper;
using EventMaster.Storage;
using EventMaster.Storage.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMaster.CourseType
{
    public class ManageCourseTypeViewModel : INotifyPropertyChanged
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

        public ManageCourseTypeViewModel()
        {
            AllCourseTypes = new BindingList<CourseTypeViewModel>(Workspace.CurrentData.CourseTypes.Select(x => new CourseTypeViewModel(x)).ToList());
            SelectedCourseType = null;
        }

        public BindingList<CourseTypeViewModel> AllCourseTypes { get; set; }
        private CourseTypeViewModel selectedCourseType;

        public CourseTypeViewModel SelectedCourseType
        {
            get { return selectedCourseType; }
            set
            {
                selectedCourseType = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCourseType)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsCourseTypeSelected)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public BindingCommand NewCourseTypeCommand
        {
            get { return new BindingCommand(x => AddNewCourseType()); }
        }
        private void AddNewCourseType()
        {
            this.AllCourseTypes.Add(new CourseTypeViewModel(CourseTypeModel.CreateNewCourseType()));
            Workspace.RegisterDataChanged();
            SelectedIndex = this.AllCourseTypes.Count - 1;
        }

        public BindingCommand RemoveCourseTypeCommand
        {
            get { return new BindingCommand(x => RemoveCourseType()); }
        }
        private void RemoveCourseType()
        {
            var selectedCourseType = SelectedCourseType;
            this.AllCourseTypes.Remove(selectedCourseType);
            //SelectedIndex = 0;
            selectedCourseType.RemoveCourseTypeFromModel();
        }

        public bool IsCourseTypeSelected => SelectedCourseType != null;
    }
}
