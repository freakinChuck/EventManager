using EventMaster.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EventMaster
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Workspace.HasChangesChanged += Workspace_HasChangesChanged;
            Workspace.WorkspaceChanged += Workspace_WorkspaceChanged;

            var commandLineArgs = Environment.GetCommandLineArgs();

            if (commandLineArgs.Length > 1)
            {
                string filePath = commandLineArgs[1];
                Workspace.LoadWorkspace(filePath);
            }
            else
            {
                Workspace.LoadWorkspace();
            }

            (DataContext as MainViewModel)?.NotifyIsWorkspaceActiveChanged();
        }

        private void Workspace_WorkspaceChanged(object sender, EventArgs e)
        {
            ContentFrame.Content = null;
            SetTitle();
        }

        private void SetTitle()
        {
            StringBuilder titleBuilder = new StringBuilder();
            titleBuilder.AppendFormat("{1}EventMaster ({0})", string.IsNullOrEmpty(Workspace.WorkspacePath) ? "*Unbenannt" + Workspace.FILEENDING : Workspace.WorkspacePath, Workspace.HasCurrentWorkspaceChanges ? "*" : string.Empty);
            this.Title = titleBuilder.ToString();
        }

        private void Workspace_HasChangesChanged(object sender, EventArgs e)
        {
            SetTitle();
        }

        private void OpenEmployeeRibbonButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = new Employee.ManageEmployeeView();
        }

        private void ContentFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            e.Cancel = false;
        }

        private void NewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Workspace.CreateNewWorkspace();
            (DataContext as MainViewModel)?.NotifyIsWorkspaceActiveChanged();
        }

        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Workspace.LoadWorkspace();
            (DataContext as MainViewModel)?.NotifyIsWorkspaceActiveChanged();
        }

        private void SaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel)?.PreDataSaveInvoke();
            Workspace.SaveCurrentWorkspace();
            (DataContext as MainViewModel)?.NotifyIsWorkspaceActiveChanged();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (Workspace.IsWorkspaceActive)
            {
                var result = Workspace.AskToSaveCurrentWorkspaceAndSaveIt();
                e.Cancel = result.Cancel;
            }
        }

        private void OpenParticipantsRibbonButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = new Participant.ManageParticipantView();
        }

        private void OpenCourseTypesRibbonButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = new CourseType.ManageCourseTypeView();
        }

        private void OpenPeriodsRibbonButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = new CoursePeriod.ManageCoursePeriodView();
        }

        private void OpenCoursesRibbonButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = new Course.ManageCourseView();

        }

        private void OpenRegistrationRibbonButton_Click(object sender, RoutedEventArgs e)
        {
            var registrationWindow = new CourseRegistration.CourseRegistrationWindow();
            //registrationWindow.Parent = this;
            //ContentFrame.Content = registrationWindow.Content;
            registrationWindow.Show();
        }
    }
}
