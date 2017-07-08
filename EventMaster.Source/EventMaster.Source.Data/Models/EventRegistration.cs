using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMaster.Source.Data.Models
{
    public class EventRegistration
    {
        public string Id { get; set; }
        public string ParticipantId { get; set; }
        public string FirstEventChoiceId { get; set; }
        public string SecondEventChoiceId { get; set; }
        public string ThirdEventChoiceId { get; set; }
        public DateTime ReceptionDate { get; set; }
    }
}
