using EventMaster._Helper;
using EventMaster.Storage;
using EventMaster.Storage.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            AllParticipants = new BindingList<ParticipantViewModel>(Workspace.CurrentData.Participants.Select(x => new ParticipantViewModel(x)).ToList());
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
