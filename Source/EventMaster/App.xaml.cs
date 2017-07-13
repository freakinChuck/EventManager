using EventMaster.Storage;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace EventMaster
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Workspace.AskSavePathWorkspace += Workspace_AskSavePathWorkspace;
            Workspace.AskSaveWorkspace += Workspace_AskSaveWorkspace;

            base.OnStartup(e);
        }

        private void Workspace_AskSaveWorkspace(object sender, WorkspaceCancelEventArgs e)
        {
            var result = MessageBox.Show("Sie haben ungespeicherte Änderungen, möchten Sie diese Speichern?", "ungespeicherte Änderungen", MessageBoxButton.YesNoCancel, MessageBoxImage.Information);
            e.Cancel = result == MessageBoxResult.Cancel;
            e.DoSave = result == MessageBoxResult.Yes;
        }

        private void Workspace_AskSavePathWorkspace(object sender, WorkspacePathEventArgs e)
        {
            if (e.IsSave)
            {
                Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
                dialog.Filter = string.Format("EventMaster (*{0})|*{0}", Workspace.FILEENDING);
                var result = dialog.ShowDialog();
                if (result ?? false)
                {
                    e.Path = dialog.FileName;
                }
            }
            else
            {
                Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
                dialog.Filter = string.Format("EventMaster (*{0})|*{0}", Workspace.FILEENDING);
                dialog.CheckFileExists = true;
                var result = dialog.ShowDialog();
                if (result ?? false)
                {
                    e.Path = dialog.FileName;
                }
            }           
        }
    }
}
