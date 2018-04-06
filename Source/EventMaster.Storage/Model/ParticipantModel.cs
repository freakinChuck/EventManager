using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMaster.Storage.Model
{
    public class ParticipantModel
    {
        internal ParticipantModel()
        {
            PeriodIds = new List<string>();
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Firstname { get; set; }
        public string Address { get; set; }
        public string Town { get; set; }
        public string AdditionalInformation { get; set; }
        public List<string> PeriodIds { get; set; }

        public static ParticipantModel CreateNewParticipant()
        {
            var model = new ParticipantModel();
            model.Id = Guid.NewGuid().ToString();
            Workspace.CurrentData.Participants.Add(model);
            return model;
        }
    }
}
