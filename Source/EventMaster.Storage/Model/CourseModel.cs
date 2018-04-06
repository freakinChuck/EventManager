using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMaster.Storage.Model
{
    public class CourseModel
    {
        internal CourseModel()
        {

        }
        public string Id { get; set; }
        public string PeriodeId { get; set; }
        public string EmployeeCourseLeaderId { get; set; }
        public string CourseTypeId { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public string Name { get; set; }
        public string MeetPoint { get; set; }
        public string CourseNumber { get; set; }
        public int MaxNumberOfParticipants { get; set; }

        public string AdditionalInformation { get; set; }

        public static CourseModel CreateNewCourse()
        {
            var model = new CourseModel();
            model.Id = Guid.NewGuid().ToString();
            Workspace.CurrentData.Courses.Add(model);
            return model;
        }
    }
}
