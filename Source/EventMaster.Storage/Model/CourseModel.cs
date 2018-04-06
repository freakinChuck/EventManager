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
        public string EmployeeCourseLeaderId { get; set; }
        public DateTime DateAndTime { get; set; }

        public static CourseModel CreateNewCourse()
        {
            var model = new CourseModel();
            model.Id = Guid.NewGuid().ToString();
            Workspace.CurrentData.Courses.Add(model);
            return model;
        }
    }
}
