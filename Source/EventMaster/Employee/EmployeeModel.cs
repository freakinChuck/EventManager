using System.ComponentModel;

namespace EventMaster.Employee
{
    public class EmployeeViewModel : INotifyPropertyChanged
    {
        private string name;

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

        private string firstname;


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