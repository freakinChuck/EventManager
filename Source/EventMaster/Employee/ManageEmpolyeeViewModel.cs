using EventMaster._Helper;
using EventMaster.Storage;
using EventMaster.Storage.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMaster.Employee
{
    public class ManageEmployeeViewModel : INotifyPropertyChanged
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

        public ManageEmployeeViewModel()
        {
            AllEmployees = new BindingList<EmployeeViewModel>(Workspace.CurrentData.Employees.Select(x => new EmployeeViewModel(x)).OrderBy(x => x.DisplayName).ToList());
            SelectedEmployee = null;
        }

        public BindingList<EmployeeViewModel> AllEmployees { get; set; }
        private EmployeeViewModel selectedEmployee;

        public EmployeeViewModel SelectedEmployee
        {
            get { return selectedEmployee; }
            set
            {
                selectedEmployee = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedEmployee)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsEmployeeSelected)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public BindingCommand NewEmployeeCommand
        {
            get { return new BindingCommand(x => AddNewEmployee()); }
        }
        private void AddNewEmployee()
        {
            this.AllEmployees.Add(new EmployeeViewModel(EmployeeModel.CreateNewEmployee()));
            Workspace.RegisterDataChanged();
            SelectedIndex = this.AllEmployees.Count - 1;
        }

        public BindingCommand RemoveEmployeeCommand
        {
            get { return new BindingCommand(x => RemoveEmployee()); }
        }
        private void RemoveEmployee()
        {
            var selectedEmployee = SelectedEmployee;
            this.AllEmployees.Remove(selectedEmployee);
            //SelectedIndex = 0;
            selectedEmployee.RemoveEmployeeFromModel();
        }

        public bool IsEmployeeSelected => SelectedEmployee != null;
    }
}
