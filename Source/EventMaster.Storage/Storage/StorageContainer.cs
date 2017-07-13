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
            Participants = new List<ParticipantModel>();
        }
        public List<EmployeeModel> Employees { get; set; }
        public List<ParticipantModel> Participants { get; set; }
    }
}
