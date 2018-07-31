using System;
using System.Collections.Generic;
using System.Text;

namespace EventMaster.Storage.Model
{
    public class EmployeeModel
    {
        internal EmployeeModel()
        {

        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Firstname { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public static EmployeeModel CreateNewEmployee()
        {
            var model = new EmployeeModel();
            model.Id = Guid.NewGuid().ToString();
            Workspace.CurrentData.Employees.Add(model);
            return model;
        }
    }
}
