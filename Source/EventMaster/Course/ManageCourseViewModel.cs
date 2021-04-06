using EventMaster._Helper;
using EventMaster.Storage;
using EventMaster.Storage.Model;
using Microsoft.Win32;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Outlook = Microsoft.Office.Interop.Outlook;

namespace EventMaster.Course
{
    public class ManageCourseViewModel : INotifyPropertyChanged
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

        public ManageCourseViewModel()
        {
            AllCourses = new BindingList<CourseViewModel>(Workspace.CurrentData.Courses.Select(x => new CourseViewModel(x)).OrderBy(x => x.DisplayName).ToList());
            AllEmployees = new BindingList<IdNameHelper>(Workspace.CurrentData.Employees.Select(x => new IdNameHelper() { Id = x.Id, Name = $"{x.Firstname} {x.Name}" }).OrderBy(x => x.Name).ToList());
            AllCourseTypes = new BindingList<IdNameHelper>(Workspace.CurrentData.CourseTypes.Select(x => new IdNameHelper() { Id = x.Id, Name = x.TypeName }).OrderBy(x => x.Name).ToList());
            AllPeriods = new BindingList<IdNameHelper>(Workspace.CurrentData.CoursePeriods.Select(x => new IdNameHelper() { Id = x.Id, Name = x.PeriodName }).OrderBy(x => x.Name).ToList());
            SelectedCourse = null;
        }

        public BindingList<CourseViewModel> AllCourses { get; set; }
        private CourseViewModel selectedCourse;

        public CourseViewModel SelectedCourse
        {
            get { return selectedCourse; }
            set
            {
                selectedCourse = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCourse)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsCourseSelected)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public BindingCommand NewCourseCommand
        {
            get { return new BindingCommand(x => AddNewCourse()); }
        }
        private void AddNewCourse()
        {
            this.AllCourses.Add(new CourseViewModel(CourseModel.CreateNewCourse()));
            Workspace.RegisterDataChanged();
            SelectedIndex = this.AllCourses.Count - 1;
        }

        public BindingCommand RemoveCourseCommand
        {
            get { return new BindingCommand(x => RemoveCourse()); }
        }
        private void RemoveCourse()
        {
            var selectedCourse = SelectedCourse;
            this.AllCourses.Remove(selectedCourse);
            //SelectedIndex = 0;
            selectedCourse.RemoveCourseFromModel();
        }

        public CourseParticipantListViewModel SelectedCourseParticipation { get; set; }

        public BindingCommand UnregisterCommand
        {
            get { return new BindingCommand(x => Unregister()); }
        }
        private void Unregister()
        {
            var selectedCourseParticipation = SelectedCourseParticipation;
            if (selectedCourseParticipation != null && SelectedCourse != null)
            {
                var result = MessageBox.Show($"Möchten Sie { selectedCourseParticipation.Vorname } { selectedCourseParticipation.Nachname }  wirklich vom Kurs '{ SelectedCourse.DisplayName }' abmelden?", "Abmeldung", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var registration = Workspace.CurrentData.CourseParticipants.Where(x => x.Id == selectedCourseParticipation.AnmeldungsId).FirstOrDefault();
                    if (registration != null)
                    {
                        Workspace.CurrentData.CourseParticipants.Remove(registration);
                        Workspace.RegisterDataChanged();
                        SelectedCourse = this.SelectedCourse;
                    }
                }
            }
        }

        public BindingCommand SendEmailCommand
        {
            get { return new BindingCommand(x => SendEmail()); }
        }
        private void SendEmail()
        {
            try
            {
                var course = this.SelectedCourse;
                var courseLeader = Workspace.CurrentData.Employees.Find(e => e.Id == course.EmployeeCourseLeaderId);

                var courseLeaderMail = courseLeader.Email;
                var participantIds = Workspace.CurrentData.CourseParticipants.Where(x => x.CourseId == course.Id).Select(x => new { Id = x.ParticipantId, Replace = x.IsReplacementCourse }).ToList();
                var participants = participantIds.Select(x => new { Participant = Workspace.CurrentData.Participants.Where(p => p.Id == x.Id).FirstOrDefault(), Replacement = x.Replace }).Where(x => x.Participant != null).ToList();

                Outlook.Application oApp = new Outlook.Application();
                Outlook._MailItem oMailItem = (Outlook._MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
                oMailItem.To = courseLeaderMail;
                oMailItem.BCC = string.Join(";", participants.OrderBy(x => x.Replacement).Select(x => x.Participant.Email));

                StringBuilder messageBuilder = new StringBuilder();
                messageBuilder.AppendLine("Liebe Schülerin, lieber Schüler");
                messageBuilder.AppendLine();
                messageBuilder.AppendLine("Es freut uns, dass Du Dich für das Angebot");
                messageBuilder.AppendLine();
                if (string.IsNullOrEmpty(course.CourseNumber2))
                {
                    messageBuilder.Append($"{course.CourseNumber}");
                }
                else
                {
                    messageBuilder.Append($"{course.CourseNumber}/{course.CourseNumber2}");
                }
                messageBuilder.AppendLine($" {course.Name}");
                messageBuilder.AppendLine();
                messageBuilder.AppendLine("angemeldet hast. Zur Erinnerung teilen wir Dir nochmals mit, wann und wo wir uns treffen.");

                messageBuilder.AppendLine();

                messageBuilder.AppendLine($"Wann:\t{ course.Date }");
                messageBuilder.AppendLine($"\t{ course.Time }");
                messageBuilder.AppendLine();
                messageBuilder.AppendLine($"Wo:\t{ course.MeetPoint }");
                messageBuilder.AppendLine();

                messageBuilder.AppendLine($"Wann:\t{ course.Date2 }");
                messageBuilder.AppendLine($"\t{ course.Time2 }");
                messageBuilder.AppendLine();
                messageBuilder.AppendLine($"Wo:\t{ course.MeetPoint2 }");
                messageBuilder.AppendLine();

                messageBuilder.AppendLine($"Wann:\t{ course.Date3 }");
                messageBuilder.AppendLine($"\t{ course.Time3 }");
                messageBuilder.AppendLine();
                messageBuilder.AppendLine($"Wo:\t{ course.MeetPoint3 }");
                messageBuilder.AppendLine();

                messageBuilder.AppendLine($"Leitung:\t{ courseLeader.Firstname } { courseLeader.Name }, { courseLeader.Title }, Tel. { courseLeader.Phone }");
                messageBuilder.AppendLine();

                oMailItem.Body = messageBuilder.ToString();

                oMailItem.Display(false);
            }
            catch (Exception exc)
            {
                exc.ToString();
                MessageBox.Show("Die Integration von Outlook ist fehlgeschlagen.", "Outlook", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        public BindingCommand CourseListCommand
        {
            get { return new BindingCommand(x => CourseList()); }
        }
        private void CourseList()
        {
            if (SelectedCourse.Participants.Any())
            {
                using (ExcelPackage package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add($"Kursliste {SelectedCourse.DisplayName}");

                    var columnIndex = 1;
                    worksheet.SetValue(1, columnIndex++, "Nachname");
                    worksheet.SetValue(1, columnIndex++, "Vorname");
                    worksheet.SetValue(1, columnIndex++, "Adresse");
                    worksheet.SetValue(1, columnIndex++, "Ort");
                    worksheet.SetValue(1, columnIndex++, "Telefon");
                    worksheet.SetValue(1, columnIndex++, "Email");

                    var rowIndex = 2;
                    foreach (var item in SelectedCourse.Participants)
                    {
                        columnIndex = 1;
                        var registration = Workspace.CurrentData.CourseParticipants.Find(x => x.Id == item.AnmeldungsId);
                        var participant = Workspace.CurrentData.Participants.Find(x => x.Id == registration.ParticipantId);
                        worksheet.SetValue(rowIndex, columnIndex++, participant.Name);
                        worksheet.SetValue(rowIndex, columnIndex++, participant.Firstname);
                        worksheet.SetValue(rowIndex, columnIndex++, participant.Address);
                        worksheet.SetValue(rowIndex, columnIndex++, participant.Town);
                        worksheet.SetValue(rowIndex, columnIndex++, participant.Telefon);
                        worksheet.SetValue(rowIndex, columnIndex++, participant.Email);
                        rowIndex++;
                    }

                    SaveFileDialog dialog = new SaveFileDialog();
                    dialog.AddExtension = true;
                    dialog.DefaultExt = "*.xlsx";
                    dialog.Filter = string.Format("Excel (*{0})|*{0}", ".xlsx");
                    var result = dialog.ShowDialog();
                    if (result ?? false)
                    {
                        package.SaveAs(new FileInfo(dialog.FileName));
                    }

                }
            }
            
        }

        public bool IsCourseSelected => SelectedCourse != null;

        public BindingList<IdNameHelper> AllEmployees { get; set; }
        public BindingList<IdNameHelper> AllCourseTypes { get; set; }
        public BindingList<IdNameHelper> AllPeriods { get; set; }

    }
}
