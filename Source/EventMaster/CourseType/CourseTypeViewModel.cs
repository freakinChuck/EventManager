using EventMaster.Storage.Model;
using System.ComponentModel;
using System;
using EventMaster.Storage;

namespace EventMaster.CourseType
{
    public class CourseTypeViewModel : INotifyPropertyChanged
    {
        public CourseTypeViewModel(CourseTypeModel storageCourseType)
        {
            this.storageCourseType = storageCourseType;
            this.PropertyChanged += CourseTypeViewModel_PropertyChanged;
        }

        private void CourseTypeViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Workspace.RegisterDataChanged();
        }

        private CourseTypeModel storageCourseType;

        public string Id => storageCourseType.Id;

        public string TypeName
        {
            get { return storageCourseType.TypeName; }
            set
            {
                storageCourseType.TypeName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TypeName"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        internal void RemoveCourseTypeFromModel()
        {
            Workspace.CurrentData.CourseTypes.Remove(storageCourseType);
            Workspace.RegisterDataChanged();
        }
    }
}