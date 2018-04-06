using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMaster.Storage.Model
{
    public class CoursePeriodModel
    {
        internal CoursePeriodModel()
        {

        }
        public string Id { get; set; }
        public string PeriodName { get; set; }

        public static CoursePeriodModel CreateNewCourseType()
        {
            var model = new CoursePeriodModel();
            model.Id = Guid.NewGuid().ToString();
            Workspace.CurrentData.CoursePeriods.Add(model);
            return model;
        }
    }
}
