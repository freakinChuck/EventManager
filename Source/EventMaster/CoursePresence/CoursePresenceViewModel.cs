using EventMaster.Storage.Model;
using System.ComponentModel;
using System;
using EventMaster.Storage;
using EventMaster._Helper;

namespace EventMaster.CoursePresence
{
    public class CoursePresenceViewModel : INotifyPropertyChanged
    {
        public CoursePresenceViewModel(CourseParticipantModel storageParicipantModel)
        {
            this.storageParicipantModel = storageParicipantModel;
            this.PropertyChanged += CoursePresenceViewModel_PropertyChanged;
        }

        private void CoursePresenceViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Workspace.RegisterDataChanged();
        }

        private CourseParticipantModel storageParicipantModel;

        public string Id => storageParicipantModel.Id;

        public string Name
        {
            get
            {
                var participant = Workspace.CurrentData.Participants.Find(x => x.Id == storageParicipantModel.ParticipantId);
                return $"{ participant.Firstname } { participant.Name }";
            }
        }

        public string PresenceState
        {
            get
            {
                return storageParicipantModel.Present.HasValue ? (storageParicipantModel.Present.Value ? "Anwesend" : "Abwesend") : "Offen";
            }
        }

        public bool IsPresent
        {
            get { return storageParicipantModel.Present ?? false; }
        }
        public bool IsNotPresent
        {
            get { return !IsPresent; }
        }
        public BindingCommand SetPresentCommand { get { return new BindingCommand(x => SetPresent()); } }
        public void SetPresent()
        {
            storageParicipantModel.Present = true;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PresenceState"));
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsOpen"));
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsMissing"));
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsPresent"));
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsNotOpen"));
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsNotMissing"));
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsNotPresent"));
        }

        public bool IsMissing
        {
            get { return !(storageParicipantModel.Present ?? true); }
        }
        public bool IsNotMissing
        {
            get { return !IsMissing; }
        }
        public BindingCommand SetMissingCommand { get { return new BindingCommand(x => SetMissing()); } }
        public void SetMissing()
        {
            storageParicipantModel.Present = false;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PresenceState"));
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsOpen"));
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsMissing"));
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsPresent"));
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsNotOpen"));
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsNotMissing"));
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsNotPresent"));
        }

        public bool IsOpen
        {
            get { return !storageParicipantModel.Present.HasValue; }
        }
        public bool IsNotOpen
        {
            get { return !IsOpen; }
        }
        public BindingCommand SetOpenCommand { get { return new BindingCommand(x => SetOpen()); } }
        public void SetOpen()
        {
            storageParicipantModel.Present = null;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PresenceState"));
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsOpen"));
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsMissing"));
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsPresent"));
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsNotOpen"));
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsNotMissing"));
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsNotPresent"));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}