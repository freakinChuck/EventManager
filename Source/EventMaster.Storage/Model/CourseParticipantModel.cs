using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMaster.Storage.Model
{
    public class CourseParticipantModel
    {
        internal CourseParticipantModel()
        {

        }
        public string Id { get; set; }
        public string CourseId { get; set; }
        public string ParticipantId { get; set; }
        public bool? Present { get; set; }

        public static CourseParticipantModel CreateNewCourseParticipant()
        {
            var model = new CourseParticipantModel();
            model.Id = Guid.NewGuid().ToString();
            Workspace.CurrentData.CourseParticipants.Add(model);
            return model;
        }
    }
}
