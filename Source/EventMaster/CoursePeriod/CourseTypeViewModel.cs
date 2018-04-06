using EventMaster._Helper;
using EventMaster.Storage.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMaster.CoursePeriod
{
    public class CourseTypeViewModel : INotifyPropertyChanged
    {
        private CoursePeriodModel periodParent;
        public CourseTypeViewModel(CoursePeriodModel period, CourseTypeModel type)
        {
            this.TypeId = type.Id;
            this.TypeName = type.TypeName;
            this.periodParent = period;
        }

        public string TypeId { get; set; }
        public string TypeName { get; set; }
        public int NumberOfRequiredTypes
        {
            get
            {
                return periodParent.CourseTypes.Count(x => x == this.TypeId);
            }
        }

        public string NumberOfRequiredTypesString
        {
            get { return string.Format("{0} Kurs(e)", NumberOfRequiredTypes); }
        }

        public BindingCommand AddCourseTypeCommand
        {
            get
            {
                return new BindingCommand(
                    x =>
                    {
                        periodParent.CourseTypes.Add(this.TypeId);
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NumberOfRequiredTypes"));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NumberOfRequiredTypesString"));
                    }
                );
            }
        }
        public BindingCommand SubtractCourseTypeCommand
        {
            get
            {
                return new BindingCommand(
                    x =>
                    {
                        if (NumberOfRequiredTypes > 0)
                        {
                            periodParent.CourseTypes.Remove(this.TypeId);
                        }
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NumberOfRequiredTypes"));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NumberOfRequiredTypesString"));
                    }
                );
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
