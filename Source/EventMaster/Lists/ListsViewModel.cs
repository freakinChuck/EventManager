using System;
using System.IO;
using EventMaster._Helper;
using EventMaster.Storage;
using Microsoft.Win32;
using OfficeOpenXml;
using System.Linq;
using EventMaster.CoursePeriod;
using System.ComponentModel;
using System.Windows;

namespace EventMaster.Lists
{
    internal class ListsViewModel : INotifyPropertyChanged
    {
        public BindingCommand RegistrationConfirmationCommand
        {
            get { return new BindingCommand(x => RegistrationConfirmation()); }
        }
        public string RegistrationConfirmationStartNr { get; set; }
        public string RegistrationConfirmationEndNr { get; set; }
        public void ResetNumbers()
        {
            RegistrationConfirmationStartNr = "Start Nr.";
            RegistrationConfirmationEndNr = "End Nr.";
        }

        private void RegistrationConfirmation()
        {
            if (int.TryParse(RegistrationConfirmationStartNr, out int registrationConfirmationStartNr) && int.TryParse(RegistrationConfirmationEndNr, out int registrationConfirmationEndNr))
            {
                var allCourses = Workspace.CurrentData.Courses.Where(x => x.PeriodeId == SelectedCoursePeriod.Id).ToList();
                var allCourseIds = allCourses.Select(x => x.Id).ToList();
                var allParticipants = Workspace.CurrentData.CourseParticipants.Where(x => allCourseIds.Contains(x.CourseId)).GroupBy(x => x.ParticipantId).ToList();

                var relevantParticipants = allParticipants.Where(x => x.Any(p => p.SequencialNumber >= registrationConfirmationStartNr && p.SequencialNumber <= registrationConfirmationEndNr)).ToList();

                var excelData = relevantParticipants.Select(item =>
                {
                    var participant = Workspace.CurrentData.Participants.Find(x => x.Id == item.Key);

                    return new
                    {
                        TeilnehmerVorname = participant.Firstname,
                        TeilnehmerNachname = participant.Name,
                        TeilnehmerAdresse = participant.Address,
                        TeilnehmerOrt = participant.Town,
                        TeilnehmerEmail = participant.Email,
                        TeilnehmerTelefon = participant.Telefon,
                        TeilnehmerZusatzInfos = participant.AdditionalInformation,

                        Kurse = item.OrderBy(x => x.IsReplacementCourse).Select(x =>
                        {
                            var course = allCourses.Find(c => c.Id == x.CourseId);
                            var courseLeader = Workspace.CurrentData.Employees.Find(e => e.Id == course.EmployeeCourseLeaderId);
                            var courseType = Workspace.CurrentData.CourseTypes.Find(e => e.Id == course.CourseTypeId);

                            return new
                            {
                                Ersatz = x.IsReplacementCourse ? "Ja" : "Nein",

                                KursNr1 = course.CourseNumber,
                                KursNr2 = course.CourseNumber2,
                                KursName = course.Name,

                                Datum1 = course.Date,
                                Datum2 = course.Date2,
                                Datum3 = course.Date3,

                                Zeit1 = course.Time,
                                Zeit2 = course.Time2,
                                Zeit3 = course.Time3,

                                Treffpunkt1 = course.MeetPoint,
                                Treffpunkt2 = course.MeetPoint2,
                                Treffpunkt3 = course.MeetPoint3,
                                
                                MaximaleAnzahlTeilnehmer = course.MaxNumberOfParticipants,

                                ZusatzInfosKurs = course.AdditionalInformation,

                                Kurstyp = courseType.TypeName,

                                KursLeiterVorname = courseLeader.Firstname,
                                KursLeiterNachname = courseLeader.Name,

                                KursLeiterTitel = courseLeader.Title,
                                KursLeiterTelefon = courseLeader.Phone,
                                KursLeiterEmail = courseLeader.Email,

                            };

                        }).ToList()

                    };
                })
                .ToList();

                if (!excelData.Any())
                {
                    MessageBox.Show($"Im Bereich der Anmeldenummern {registrationConfirmationStartNr} - {registrationConfirmationEndNr} wurden keine Anmeldungen gefunden für die Periode {SelectedCoursePeriod.PeriodName} gefunden.", "keine Anmeldungen", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                using (ExcelPackage package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Anmeldebestätigung");

                    var columnIndex = 1;

                    worksheet.SetValue(1, columnIndex++, "TeilnehmerVorname");
                    worksheet.SetValue(1, columnIndex++, "TeilnehmerNachname");
                    worksheet.SetValue(1, columnIndex++, "TeilnehmerAdresse");
                    worksheet.SetValue(1, columnIndex++, "TeilnehmerOrt");
                    worksheet.SetValue(1, columnIndex++, "TeilnehmerEmail");
                    worksheet.SetValue(1, columnIndex++, "TeilnehmerTelefon");
                    worksheet.SetValue(1, columnIndex++, "TeilnehmerZusatzInfos");

                    var maxNumberOfCourseRegistration = excelData.Max(x => x.Kurse.Count);


                    for (int i = 1; i <= maxNumberOfCourseRegistration; i++)
                    {
                        worksheet.SetValue(1, columnIndex++, $"Kurs{i}KursNr1");
                        worksheet.SetValue(1, columnIndex++, $"Kurs{i}KursNr2");
                        worksheet.SetValue(1, columnIndex++, $"Kurs{i}KursName");

                        worksheet.SetValue(1, columnIndex++, $"Kurs{i}Datum1");
                        worksheet.SetValue(1, columnIndex++, $"Kurs{i}Zeit1");
                        worksheet.SetValue(1, columnIndex++, $"Kurs{i}Treffpunkt1");

                        worksheet.SetValue(1, columnIndex++, $"Kurs{i}Datum2");
                        worksheet.SetValue(1, columnIndex++, $"Kurs{i}Zeit2");
                        worksheet.SetValue(1, columnIndex++, $"Kurs{i}Treffpunkt2");

                        worksheet.SetValue(1, columnIndex++, $"Kurs{i}Datum3");
                        worksheet.SetValue(1, columnIndex++, $"Kurs{i}Zeit3");
                        worksheet.SetValue(1, columnIndex++, $"Kurs{i}Treffpunkt3");

                        worksheet.SetValue(1, columnIndex++, $"Kurs{i}MaximaleAnzahlTeilnehmer");
                        worksheet.SetValue(1, columnIndex++, $"Kurs{i}ZusatzInfosKurs");

                        worksheet.SetValue(1, columnIndex++, $"Kurs{i}Kurstyp");

                        worksheet.SetValue(1, columnIndex++, $"Kurs{i}KursLeiterVorname");
                        worksheet.SetValue(1, columnIndex++, $"Kurs{i}KursLeiterNachname");
                        worksheet.SetValue(1, columnIndex++, $"Kurs{i}KursLeiterTitel");
                        worksheet.SetValue(1, columnIndex++, $"Kurs{i}KursLeiterTelefon");
                        worksheet.SetValue(1, columnIndex++, $"Kurs{i}KursLeiterEmail");
                        worksheet.SetValue(1, columnIndex++, $"Kurs{i}Ersatz");
                    }

                    int rowIndex = 2;
                    foreach (var item in excelData)
                    {
                        columnIndex = 1;

                        worksheet.SetValue(rowIndex, columnIndex++, item.TeilnehmerVorname);
                        worksheet.SetValue(rowIndex, columnIndex++, item.TeilnehmerNachname);
                        worksheet.SetValue(rowIndex, columnIndex++, item.TeilnehmerAdresse);
                        worksheet.SetValue(rowIndex, columnIndex++, item.TeilnehmerOrt);
                        worksheet.SetValue(rowIndex, columnIndex++, item.TeilnehmerEmail);
                        worksheet.SetValue(rowIndex, columnIndex++, item.TeilnehmerTelefon);
                        worksheet.SetValue(rowIndex, columnIndex++, item.TeilnehmerZusatzInfos);

                        for (int i = 1; i <= item.Kurse.Count; i++)
                        {
                            var kurs = item.Kurse[i -1];
                            worksheet.SetValue(rowIndex, columnIndex++, kurs.KursNr1);
                            worksheet.SetValue(rowIndex, columnIndex++, kurs.KursNr2);
                            worksheet.SetValue(rowIndex, columnIndex++, kurs.KursName);
                                                                 
                            worksheet.SetValue(rowIndex, columnIndex++, kurs.Zeit1);
                            worksheet.SetValue(rowIndex, columnIndex++, kurs.Treffpunkt1);
                            worksheet.SetValue(rowIndex, columnIndex++, kurs.Datum1);
                                                                 
                            worksheet.SetValue(rowIndex, columnIndex++, kurs.Zeit2);
                            worksheet.SetValue(rowIndex, columnIndex++, kurs.Treffpunkt2);
                            worksheet.SetValue(rowIndex, columnIndex++, kurs.Datum2);
                                                                 
                            worksheet.SetValue(rowIndex, columnIndex++, kurs.Datum3);
                            worksheet.SetValue(rowIndex, columnIndex++, kurs.Zeit3);
                            worksheet.SetValue(rowIndex, columnIndex++, kurs.Treffpunkt3);

                            worksheet.SetValue(rowIndex, columnIndex++, kurs.MaximaleAnzahlTeilnehmer);
                            worksheet.SetValue(rowIndex, columnIndex++, kurs.ZusatzInfosKurs);

                            worksheet.SetValue(rowIndex, columnIndex++, kurs.Kurstyp);

                            worksheet.SetValue(rowIndex, columnIndex++, kurs.KursLeiterVorname);
                            worksheet.SetValue(rowIndex, columnIndex++, kurs.KursLeiterNachname);
                            worksheet.SetValue(rowIndex, columnIndex++, kurs.KursLeiterTitel);
                            worksheet.SetValue(rowIndex, columnIndex++, kurs.KursLeiterTelefon);
                            worksheet.SetValue(rowIndex, columnIndex++, kurs.KursLeiterEmail);
                            worksheet.SetValue(rowIndex, columnIndex++, kurs.Ersatz);
                        }
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


        public BindingCommand CheckListCommand
        {
            get { return new BindingCommand(x => CheckList()); }
        }

        private void CheckList()
        {
            var allCourses = Workspace.CurrentData.Courses.Where(x => x.PeriodeId == SelectedCoursePeriod.Id).ToList();
            var allCourseIds = allCourses.Select(x => x.Id).ToList();
            var allParticipants = Workspace.CurrentData.CourseParticipants.Where(x => allCourseIds.Contains(x.CourseId)).GroupBy(x => x.ParticipantId).ToList();

            var requiredCourseTypes = SelectedCoursePeriod.storageCoursePeriod.CourseTypes;

            var excelData = allParticipants.Select(item =>
            {
                var participant = Workspace.CurrentData.Participants.Find(x => x.Id == item.Key);

                var participations = Workspace.CurrentData.CourseParticipants.Where(x => x.ParticipantId == participant.Id && (x.Present ?? false))
                        .Select(x => Workspace.CurrentData.Courses.Find(c => c.Id == x.CourseId))
                        .Where(x => x.PeriodeId == SelectedCoursePeriod.Id)
                        .ToList();


                foreach (var participation in participations)
                {
                    if (requiredCourseTypes.Contains(participation.CourseTypeId))
                    {
                        requiredCourseTypes.Remove(participation.CourseTypeId);
                    }
                }

                var remainingCourseTypes = requiredCourseTypes.Select(id => Workspace.CurrentData.CourseTypes.Find(t => t.Id == id)).ToList();

                return new
                {
                    TeilnehmerVorname = participant.Firstname,
                    TeilnehmerNachname = participant.Name,
                    TeilnehmerAdresse = participant.Address,
                    TeilnehmerOrt = participant.Town,
                    TeilnehmerEmail = participant.Email,
                    TeilnehmerTelefon = participant.Telefon,
                    TeilnehmerZusatzInfos = participant.AdditionalInformation,

                    GenuegendKurseBesucht = !remainingCourseTypes.Any() ? "Ja" : "Nein"

                };
            });

            if (!excelData.Any())
            {
                MessageBox.Show($"Für die Periode {SelectedCoursePeriod.PeriodName} wurden keine Daten gefunden.", "keine Anmeldungen", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            using (ExcelPackage package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Jahresendliste");

                var columnIndex = 1;

                worksheet.SetValue(1, columnIndex++, "TeilnehmerVorname");
                worksheet.SetValue(1, columnIndex++, "TeilnehmerNachname");
                worksheet.SetValue(1, columnIndex++, "TeilnehmerAdresse");
                worksheet.SetValue(1, columnIndex++, "TeilnehmerOrt");
                worksheet.SetValue(1, columnIndex++, "TeilnehmerEmail");
                worksheet.SetValue(1, columnIndex++, "TeilnehmerTelefon");
                worksheet.SetValue(1, columnIndex++, "TeilnehmerZusatzInfos");
                worksheet.SetValue(1, columnIndex++, "GenuegendKurseBesucht");

                int rowIndex = 2;
                foreach (var item in excelData)
                {
                    columnIndex = 1;

                    worksheet.SetValue(rowIndex, columnIndex++, item.TeilnehmerVorname);
                    worksheet.SetValue(rowIndex, columnIndex++, item.TeilnehmerNachname);
                    worksheet.SetValue(rowIndex, columnIndex++, item.TeilnehmerAdresse);
                    worksheet.SetValue(rowIndex, columnIndex++, item.TeilnehmerOrt);
                    worksheet.SetValue(rowIndex, columnIndex++, item.TeilnehmerEmail);
                    worksheet.SetValue(rowIndex, columnIndex++, item.TeilnehmerTelefon);
                    worksheet.SetValue(rowIndex, columnIndex++, item.TeilnehmerZusatzInfos);
                    worksheet.SetValue(rowIndex, columnIndex++, item.GenuegendKurseBesucht);

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

        private int selectedIndex;

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedIndex)));
            }
        }

        public ListsViewModel()
        {
            AllCoursePeriods = new BindingList<CoursePeriodViewModel>(Workspace.CurrentData.CoursePeriods.OrderBy(x => x.PeriodName).Select(x => new CoursePeriodViewModel(x)).ToList());
            SelectedCoursePeriod = null;
            ResetNumbers();
        }

        public BindingList<CoursePeriodViewModel> AllCoursePeriods { get; set; }
        private CoursePeriodViewModel selectedCoursePeriod;

        public event PropertyChangedEventHandler PropertyChanged;

        public CoursePeriodViewModel SelectedCoursePeriod
        {
            get { return selectedCoursePeriod; }
            set
            {
                selectedCoursePeriod = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCoursePeriod)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsCoursePeriodSelected)));
            }
        }
        public bool IsCoursePeriodSelected => SelectedCoursePeriod != null;

    }
}