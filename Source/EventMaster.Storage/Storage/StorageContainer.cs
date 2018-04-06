using EventMaster.Storage.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventMaster.Storage.Storage
{
    public class StorageContainer
    {
        public StorageContainer()
        {
            Employees = new List<EmployeeModel>();
            Participants = new List<ParticipantModel>();
            CourseParticipants = new List<CourseParticipantModel>();
            Courses = new List<CourseModel>();
            CourseTypes = new List<CourseTypeModel>();
            CoursePeriods = new List<CoursePeriodModel>();
            CourseTypePeriods = new List<CourseTypePeriodModel>();
            ParticipantPeriods = new List<ParticipantPeriodModel>();
        }
        public List<EmployeeModel> Employees { get; set; }
        public List<ParticipantModel> Participants { get; set; }
        public List<CourseParticipantModel> CourseParticipants { get; set; }
        public List<CourseModel> Courses { get; set; }
        public List<CourseTypeModel> CourseTypes { get; set; }
        public List<CoursePeriodModel> CoursePeriods { get; set; }
        public List<CourseTypePeriodModel> CourseTypePeriods { get; set; }
        public List<ParticipantPeriodModel> ParticipantPeriods { get; internal set; }
    }
}
