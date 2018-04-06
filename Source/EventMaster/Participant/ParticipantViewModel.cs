using EventMaster.Storage.Model;
using System.ComponentModel;
using System;
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

        public string DisplayName => $"{Name} {Firstname}";

        public event PropertyChangedEventHandler PropertyChanged;

        internal void RemoveParticipantFromModel()
        {
            Workspace.CurrentData.Participants.Remove(storageParticipant);
            Workspace.RegisterDataChanged();
        }
    }
}