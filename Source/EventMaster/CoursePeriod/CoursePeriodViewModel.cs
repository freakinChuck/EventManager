using EventMaster.Storage.Model;
using System.ComponentModel;
using System;
using EventMaster.Storage;

namespace EventMaster.CoursePeriod
{
    public class CoursePeriodViewModel : INotifyPropertyChanged
    {
        public CoursePeriodViewModel(CoursePeriodModel storageCoursePeriod)
        {
            this.storageCoursePeriod = storageCoursePeriod;
            this.PropertyChanged += CoursePeriodViewModel_PropertyChanged;
        }

        private void CoursePeriodViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Workspace.RegisterDataChanged();
        }

        internal CoursePeriodModel storageCoursePeriod;

        public string Id => storageCoursePeriod.Id;

        public string PeriodName
        {
            get { return storageCoursePeriod.PeriodName; }
            set
            {
                storageCoursePeriod.PeriodName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PeriodName"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        internal void RemoveCoursePeriodFromModel()
        {
            Workspace.CurrentData.CoursePeriods.Remove(storageCoursePeriod);
            Workspace.RegisterDataChanged();
        }
    }
}