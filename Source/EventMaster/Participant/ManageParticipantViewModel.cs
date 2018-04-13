using EventMaster._Helper;
using EventMaster.Storage;
using EventMaster.Storage.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EventMaster.Participant
{
    public class ManageParticipantViewModel : INotifyPropertyChanged
    {
        private int selectedIndex;

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set {
                selectedIndex = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedIndex)));
            }
        }

        public ManageParticipantViewModel()
        {
            AllParticipants = new BindingList<ParticipantViewModel>(Workspace.CurrentData.Participants.Select(x => new ParticipantViewModel(x)).OrderBy(x => x.DisplayName).ToList());
            SelectedParticipant = null;
        }

        public BindingList<ParticipantViewModel> AllParticipants { get; set; }
        private ParticipantViewModel selectedParticipant;

        public ParticipantViewModel SelectedParticipant
        {
            get { return selectedParticipant; }
            set
            {
                selectedParticipant = value;
                LoadPeriods();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedParticipant)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsParticipantSelected)));
            }
        }
        public CourseParticipantListViewModel SelectedCourse { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public BindingCommand NewParticipantCommand
        {
            get { return new BindingCommand(x => AddNewParticipant()); }
        }
        private void AddNewParticipant()
        {
            this.AllParticipants.Add(new ParticipantViewModel(ParticipantModel.CreateNewParticipant()));
            Workspace.RegisterDataChanged();
            SelectedIndex = this.AllParticipants.Count - 1;
        }

        public BindingCommand RemoveParticipantCommand
        {
            get { return new BindingCommand(x => RemoveParticipant()); }
        }
        private void RemoveParticipant()
        {
            var selectedParticipant = SelectedParticipant;
            this.AllParticipants.Remove(selectedParticipant);
            //SelectedIndex = 0;
            selectedParticipant.RemoveParticipantFromModel();
        }
        public BindingCommand UnregisterCommand
        {
            get { return new BindingCommand(x => Unregister()); }
        }
        private void Unregister()
        {
            var selectedCourse = SelectedCourse;
            if (selectedCourse != null && SelectedParticipant != null)
            {
                var result = MessageBox.Show($"Möchten Sie { SelectedParticipant.DisplayName } wirklich vom Kurs '{ SelectedCourse.Kursnummer } - { SelectedCourse.Titel }' abmelden?", "Abmeldung", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var registration = Workspace.CurrentData.CourseParticipants.Where(x => x.Id == selectedCourse.AnmeldungsId).FirstOrDefault();
                    if (registration != null)
                    {
                        Workspace.CurrentData.CourseParticipants.Remove(registration);
                        Workspace.RegisterDataChanged();
                        SelectedParticipant = this.SelectedParticipant;
                    }
                }
            }
        }


        

        public bool IsParticipantSelected => SelectedParticipant != null;


        public BindingList<ParticipantPeriodViewModel> AllPeriods
        {
            get; set;
        }
        private void LoadPeriods()
        {
            var participant = this.SelectedParticipant;
            if (participant == null)
            {
                this.AllPeriods = new BindingList<ParticipantPeriodViewModel>();
            }
            else
            {
                this.AllPeriods = new BindingList<ParticipantPeriodViewModel>(Workspace.CurrentData.CoursePeriods.OrderBy(x => x.PeriodName).Select(x => new ParticipantPeriodViewModel(SelectedParticipant.storageParticipant, x)).ToList());
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllPeriods)));
        }
    }
}
