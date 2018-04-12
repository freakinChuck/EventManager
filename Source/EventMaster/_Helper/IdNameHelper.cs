using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMaster._Helper
{
    public class IdNameHelper
    {
        public static IdNameHelper Empty { get { return new IdNameHelper(); } }

        public string Id { get; set; }
        public string Name { get; set; }
    }
}
