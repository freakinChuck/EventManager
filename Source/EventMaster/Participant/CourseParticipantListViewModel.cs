namespace EventMaster.Participant
{
    public class CourseParticipantListViewModel
    {
        public string Periode { get; set; }
        public string Kursnummer { get; set; }
        public string Titel { get; set; }
        public string Datum { get; set; }
        public string Kursleiter { get; set; }
        public string Anwesenheit { get; set; }

        public int Laufnummer { get; set; }

        public string Ersatz { get; set; }

        public string AnmeldungsId { get; set; }
    }
}