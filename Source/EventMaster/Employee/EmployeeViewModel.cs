using EventMaster.Storage.Model;
using System.ComponentModel;
using System;
using EventMaster.Storage;

namespace EventMaster.Employee
{
    public class EmployeeViewModel : INotifyPropertyChanged
    {
        public EmployeeViewModel(EmployeeModel storageEmployee)
        {
            this.storageEmployee = storageEmployee;
            this.PropertyChanged += EmployeeViewModel_PropertyChanged;
        }

        private void EmployeeViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Workspace.RegisterDataChanged();
        }

        private EmployeeModel storageEmployee;

        public string Id => storageEmployee.Id;

        public string Name
        {
            get { return storageEmployee.Name; }
            set
            {
                storageEmployee.Name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DisplayName"));
            }
        }

        public string Firstname
        {
            get { return storageEmployee.Firstname; }
            set
            {
                storageEmployee.Firstname = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("First"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DisplayName"));
            }
        }

        public string Title
        {
            get { return storageEmployee.Title; }
            set
            {
                storageEmployee.Firstname = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Title"));
            }
        }

        public string Email
        {
            get { return storageEmployee.Email; }
            set
            {
                storageEmployee.Email = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Email"));
            }
        }
        public string Phone
        {
            get { return storageEmployee.Phone; }
            set
            {
                storageEmployee.Phone = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Phone"));
            }
        }

        public string DisplayName => $"{Name} {Firstname}";

        public event PropertyChangedEventHandler PropertyChanged;

        internal void RemoveEmployeeFromModel()
        {
            Workspace.CurrentData.Employees.Remove(storageEmployee);
            Workspace.RegisterDataChanged();
        }
    }
}