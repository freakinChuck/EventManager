using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventMaster._Helper;
using EventMaster.Participant;
using EventMaster.Storage;
using EventMaster.Storage.Model;

namespace EventMaster.CourseRegistration
{
    public class CourseRegistrationViewModel : INotifyPropertyChanged
    {
        public CourseRegistrationViewModel()
        {
            AllPeriods = new BindingList<IdNameHelper>(Workspace.CurrentData.CoursePeriods.Select(x => new IdNameHelper() { Id = x.Id, Name = x.PeriodName }).OrderBy(x => x.Name).ToList());
            ParticipantsByPeriod = new BindingList<ParticipantViewModel>();

        }

        public List<IdNameHelper> AllCourses
        {
            get
            {
                var list = Workspace.CurrentData.Courses.Where(x => x.PeriodeId == this.PeriodeId).Select(x => new { x.Id, x.CourseNumber, x.Name, x.MaxNumberOfParticipants })
                    .Union(Workspace.CurrentData.Courses.Where(x => x.PeriodeId == this.PeriodeId && !string.IsNullOrWhiteSpace(x.CourseNumber2)).Select(x => new { x.Id, CourseNumber = x.CourseNumber2, x.Name, x.MaxNumberOfParticipants }))
                    .OrderBy(x => x.CourseNumber).ThenBy(x => x.Name).Select(x =>
                {
                    var courseAlreadyFull = Workspace.CurrentData.CourseParticipants.Count(c => c.CourseId == x.Id) >= x.MaxNumberOfParticipants;
                    return new IdNameHelper { Id = x.Id, Name = $"{ (courseAlreadyFull ? "Ausgebucht: " : string.Empty) }{x.CourseNumber} - {x.Name}" };
                }).ToList();
                list.Insert(0, IdNameHelper.Empty);
                return list;
            }
        }

        public string Course1Id { get; set; }
        public string Course2Id { get; set; }
        public string Course3Id { get; set; }
        public string CourseReplaceId { get; set; }
        public string CourseInputId { get; set; }

        private string periodeId;
        private int selectedIndex;
        private ParticipantViewModel selectedParticipant;

        public event PropertyChangedEventHandler PropertyChanged;

        public BindingList<IdNameHelper> AllPeriods { get; }
        public BindingList<ParticipantViewModel> ParticipantsByPeriod { get; private set; }
        public string PeriodeId
        {
            get { return periodeId; }
            set
            {
                periodeId = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PeriodeId)));
                ReloadParticipants();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllCourses)));
            }
        }

        private void ReloadParticipants()
        {
            ParticipantsByPeriod = new BindingList<ParticipantViewModel>(Workspace.CurrentData.Participants.Where(x => x.PeriodIds.Contains(PeriodeId)).Select(x => new ParticipantViewModel(x)).OrderBy(x => x.DisplayName).ToList());
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ParticipantsByPeriod)));
        }

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedIndex)));
            }
        }
        public ParticipantViewModel SelectedParticipant
        {
            get { return selectedParticipant; }
            set
            {
                selectedParticipant = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedParticipant)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsParticipantSelected)));
            }
        }

        private void ResetCourseSelection()
        {
            Course1Id = string.Empty;
            Course2Id = string.Empty;
            Course3Id = string.Empty;
            CourseReplaceId = string.Empty;
            CourseInputId = string.Empty;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Course1Id)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Course2Id)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Course3Id)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CourseReplaceId)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CourseInputId)));
        }

        public bool IsParticipantSelected => SelectedParticipant != null;

        public BindingCommand RegiterSelectionCommand
        {
            get { return new BindingCommand(x => RegiterSelection()); }
        }
        private void RegiterSelection()
        {
            RegisterCourse(Course1Id);
            RegisterCourse(Course2Id);
            RegisterCourse(Course3Id);
            RegisterCourse(CourseInputId);
            RegisterCourse(CourseReplaceId, replacementCourse: true);
            ResetCourseSelection();

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedParticipant)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllCourses)));
        }

        private void RegisterCourse(string courseId, bool replacementCourse = false)
        {
            if (!string.IsNullOrWhiteSpace(courseId) && SelectedParticipant != null)
            {
                if (!Workspace.CurrentData.CourseParticipants.Any(x => x.CourseId == courseId && x.ParticipantId == SelectedParticipant.Id))
                {
                    var newParticipant = CourseParticipantModel.CreateNewCourseParticipant();
                    newParticipant.CourseId = courseId;
                    newParticipant.ParticipantId = SelectedParticipant.Id;
                    newParticipant.IsReplacementCourse = replacementCourse;
                }
            }
        }
    }
}
