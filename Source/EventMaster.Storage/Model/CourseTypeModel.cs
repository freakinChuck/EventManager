using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMaster.Storage.Model
{
    public class CourseTypeModel
    {
        public CourseTypeModel()
        {

        }
        public string Id { get; set; }
        public string TypeName { get; set; }

        public static CourseTypeModel CreateNewCourseType()
        {
            var model = new CourseTypeModel();
            model.Id = Guid.NewGuid().ToString();
            Workspace.CurrentData.CourseTypes.Add(model);
            return model;
        }
    }
}
