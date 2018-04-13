namespace EventMaster.Course
{
    public class CourseParticipantListViewModel
    {
        public string Vorname { get; set; }
        public string Nachname { get; internal set; }
        public string Email { get; internal set; }
        public string Telefon { get; internal set; }

        public string Anwesenheit { get; set; }

        public int Laufnummer { get; set; }

        public string Ersatz { get; set; }

        public string AnmeldungsId { get; set; }
        
    }
}