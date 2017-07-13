using EventMaster.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EventMaster
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private static MainViewModel instance;
        public static MainViewModel Instance => instance;

        public event EventHandler PreDataSave;
        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel()
        {
            MainViewModel.instance = this;
        }

        public bool IsWorkspaceActive
        {
            get { return Workspace.IsWorkspaceActive; }
        }

        internal void NotifyIsWorkspaceActiveChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsWorkspaceActive)));
        }

        internal void PreDataSaveInvoke()
        {
            PreDataSave?.Invoke(this, new EventArgs());
        }
    }
}
