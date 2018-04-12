using EventMaster._Helper;
using EventMaster.Storage.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMaster.Participant
{
    public class ParticipantPeriodViewModel : INotifyPropertyChanged
    {
        private ParticipantModel participantParent;
        public ParticipantPeriodViewModel(ParticipantModel participant, CoursePeriodModel period)
        {
            this.PeriodId = period.Id;
            this.PeriodName = period.PeriodName;
            this.participantParent = participant;
        }

        public string PeriodId { get; set; }
        public string PeriodName { get; set; }
        public int NumberOfPeriods
        {
            get
            {
                return participantParent.PeriodIds.Count(x => x == this.PeriodId);
            }
        }

        public string NumberOfPeriodsString
        {
            get { return NumberOfPeriods > 0 ? "Ja" : "Nein"; }
        }

        public bool AddEnabled
        {
            get { return NumberOfPeriods < 1; }
        }
        public bool MinusEnabled
        {
            get { return NumberOfPeriods > 0; }
        }

        public BindingCommand AddParticipantPeriodCommand
        {
            get
            {
                return new BindingCommand(
                    x =>
                    {
                        if (AddEnabled)
                        {
                            participantParent.PeriodIds.Add(this.PeriodId);
                        }
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NumberOfPeriods"));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NumberOfPeriodsString"));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AddEnabled"));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MinusEnabled"));
                    }
                );
            }
        }
        public BindingCommand SubtractParticipantPeriodCommand
        {
            get
            {
                return new BindingCommand(
                    x =>
                    {
                        if (NumberOfPeriods > 0)
                        {
                            participantParent.PeriodIds.Remove(this.PeriodId);
                        }
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NumberOfPeriods"));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NumberOfPeriodsString"));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AddEnabled"));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MinusEnabled"));
                    }
                );
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
