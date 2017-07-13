using EventMaster.Storage.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventMaster.Storage.Storage
{
    public class StorageContainer
    {
        public StorageContainer()
        {
            Employees = new List<EmployeeModel>();
        }
        public List<EmployeeModel> Employees { get; set; }
    }
}
