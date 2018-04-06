using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMaster.Storage.Model
{
    public class CourseTypePeriodModel
    {
        internal CourseTypePeriodModel()
        {

        }
        public string Id { get; set; }
        public string PeriodName { get; set; }

        public static CourseTypePeriodModel CreateNewCourseTypePeriodModel()
        {
            var model = new CourseTypePeriodModel();
            model.Id = Guid.NewGuid().ToString();
            Workspace.CurrentData.CourseTypePeriods.Add(model);
            return model;
        }
    }
}
