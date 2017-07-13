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

        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Firstname { get; set; }
        public string Address { get; set; }
        public string Town { get; set; }
        public string AdditionalInformation { get; set; }

        public bool Active { get; set; }

        public static ParticipantModel CreateNewParticipant()
        {
            var model = new ParticipantModel();
            model.Id = Guid.NewGuid().ToString();
            Workspace.CurrentData.Participants.Add(model);
            return model;
        }
    }
}
