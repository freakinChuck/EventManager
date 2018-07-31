using EventMaster.Storage.Model;
using System.ComponentModel;
using System;
using System.Linq;
using EventMaster.Storage;
using System.Collections.Generic;

namespace EventMaster.Participant
{
    public class ParticipantViewModel : INotifyPropertyChanged
    {
        public ParticipantViewModel(ParticipantModel storageParticipant)
        {
            this.storageParticipant = storageParticipant;
            this.PropertyChanged += ParticipantViewModel_PropertyChanged;
        }

        private void ParticipantViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Workspace.RegisterDataChanged();
        }

        internal ParticipantModel storageParticipant;

        public string Id => storageParticipant.Id;

        public string Name
        {
            get { return storageParticipant.Name; }
            set
            {
                storageParticipant.Name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DisplayName"));
            }
        }

        public string Firstname
        {
            get { return storageParticipant.Firstname; }
            set
            {
                storageParticipant.Firstname = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("First"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DisplayName"));
            }
        }

        public string Address
        {
            get { return storageParticipant.Address; }
            set
            {
                storageParticipant.Address = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Address"));
            }
        }
        public string Town
        {
            get { return storageParticipant.Town; }
            set
            {
                storageParticipant.Town = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Town"));
            }
        }

        public string Telefon
        {
            get { return storageParticipant.Telefon; }
            set
            {
                storageParticipant.Telefon = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Telefon"));
            }
        }

        public string Email
        {
            get { return storageParticipant.Email; }
            set
            {
                storageParticipant.Email = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Email"));
            }
        }
        public string AdditionalInformation
        {
            get { return storageParticipant.AdditionalInformation; }
            set
            {
                storageParticipant.AdditionalInformation = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AdditionalInformation"));
            }
        }

        public List<string> PeriodIds
        {
            get { return storageParticipant.PeriodIds; }
            set
            {
                storageParticipant.PeriodIds = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PeriodIds"));
            }
        }

        public List<CourseParticipantListViewModel> Courses
        {
            get
            {
                var participants = Workspace.CurrentData.CourseParticipants.Where(x => x.ParticipantId == this.Id).ToList();
                var participantCourseMapping = participants.Select(x => new { Participant = x, Course = Workspace.CurrentData.Courses.Where(c => c.Id == x.CourseId).FirstOrDefault() }).ToList();
                var displayPreperation = participantCourseMapping.Where(x => x.Course != null).Select(x => new { Participant = x.Participant, Course = x.Course, CourseLeader = Workspace.CurrentData.Employees.FirstOrDefault(e => e.Id == x.Course.EmployeeCourseLeaderId) }).ToList();
                return displayPreperation.Select(x => new CourseParticipantListViewModel
                {
                    Periode = Workspace.CurrentData.CoursePeriods.Where(p => p.Id == x.Course.PeriodeId).FirstOrDefault()?.PeriodName ?? "unbekannte Periode",
                    Titel = x.Course.Name,
                    Kursnummer = x.Course.CourseNumber + (!string.IsNullOrEmpty(x.Course.CourseNumber2) ? "/"+x.Course.CourseNumber2 : string.Empty),
                    Datum = $"{x.Course.Date} - {x.Course.Time}",
                    Kursleiter = x.CourseLeader != null ? ($"{ x.CourseLeader.Name } { x.CourseLeader.Firstname }") : "unbekannt",
                    Anwesenheit = x.Participant.Present.HasValue ? (x.Participant.Present.Value ? "Anwesend" : "Abwesend") : "Offen",
                    Ersatz = x.Participant.IsReplacementCourse ? "Ersatzkurs" : string.Empty,
                    Laufnummer = x.Participant.SequencialNumber,
                    AnmeldungsId = x.Participant.Id,
                }).ToList();
            }
        }
        public List<CourseParticipantListViewModel> CoursesWithoutAnsences
        {
            get { return Courses.Where(x => (x.Anwesenheit != "Abwesend")).ToList(); }
        }

        public string DisplayName => $"{Name} {Firstname}";

        public event PropertyChangedEventHandler PropertyChanged;

        internal void RemoveParticipantFromModel()
        {
            Workspace.CurrentData.Participants.Remove(storageParticipant);
            Workspace.RegisterDataChanged();
        }
    }
}