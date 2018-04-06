using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMaster.Storage.Model
{
    public class ParticipantPeriodModel
    {
        internal ParticipantPeriodModel()
        {

        }
        public string Id { get; set; }
        public string PeriodId { get; set; }
        public string ParticipantId { get; set; }

        public static ParticipantPeriodModel CreateNeweParticipantPeriod()
        {
            var model = new ParticipantPeriodModel();
            model.Id = Guid.NewGuid().ToString();
            Workspace.CurrentData.ParticipantPeriods.Add(model);
            return model;
        }
    }
}
