using EventMaster.Storage;
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
            AllEmployees = new BindingList<EmployeeViewModel>(Workspace.CurrentData.Employees.Select(x => new EmployeeViewModel(x)).ToList());
            MainViewModel.Instance.PreDataSave += Instance_PreDataSave; 
        }

        private void Instance_PreDataSave(object sender, EventArgs e)
        {
            DoSaveDataToModel();
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

        private void DoSaveDataToModel()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
