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
        public ManageEmployeeViewModel()
        {
            AllEmployees = new BindingList<EmployeeViewModel>(new List<EmployeeViewModel>() {
                new EmployeeViewModel() { Name = "Silvio" },
                new EmployeeViewModel() { Name = "Silvio" },
                new EmployeeViewModel() { Name = "Silvio" },
                new EmployeeViewModel() { Name = "Silvio" },
                new EmployeeViewModel() { Name = "Silvio" },
                new EmployeeViewModel() { Name = "Silvio" },
                new EmployeeViewModel() { Name = "Silvio" },
                new EmployeeViewModel() { Name = "Silvio" },
                new EmployeeViewModel() { Name = "Silvio" }
            });
        }
        public BindingList<EmployeeViewModel> AllEmployees { get; set; }
        private EmployeeViewModel selectedEmployee;

        public EmployeeViewModel SelectedEmployee
        {
            get { return selectedEmployee; }
            set
            {
                selectedEmployee = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedEmployee"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
