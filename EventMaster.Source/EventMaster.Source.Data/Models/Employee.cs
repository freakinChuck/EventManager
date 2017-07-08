using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMaster.Source.Data.Models
{
    public class Employee
    {
        public string Id { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }

        public string DisplayName
        {
            get
            {
                return string.Format("{0} {1}", Firstname, Lastname);
            }
        }
    }
}
