using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMaster.Source.Data.Models
{
    public class Event
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }

        public int MaxNumberOfParticipants { get; set; }

        public int MinNumberOfParticipants { get; set; }
    }
}
