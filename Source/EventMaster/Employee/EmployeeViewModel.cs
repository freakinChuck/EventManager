using EventMaster.Storage.Model;
using System.ComponentModel;

namespace EventMaster.Employee
{
    public class EmployeeViewModel : INotifyPropertyChanged
    {
        public EmployeeViewModel(EmployeeModel storageEmployee)
        {
            id = storageEmployee.Id;
            name = storageEmployee.Name;
            firstname = storageEmployee.Firstname;
        }

        private string id;
        private string name;
        private string firstname;

        public string Id => id;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DisplayName"));
            }
        }

        public string Firstname
        {
            get { return firstname; }
            set
            {
                firstname = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("First"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DisplayName"));
            }
        }

        public string DisplayName => $"{Name} {Firstname}";

        public event PropertyChangedEventHandler PropertyChanged;
    }
}