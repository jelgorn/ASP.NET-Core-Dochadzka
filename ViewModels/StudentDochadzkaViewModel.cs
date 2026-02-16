namespace EvidenciaStudentov.ViewModels
{
    public class StudentDochadzkaViewModel
    {
        public int PouzivatelId { get; set; }
        public string Meno { get; set; } = string.Empty;
        public string Priezvisko { get; set; } = string.Empty;
        public bool JePritomny { get; set; }
    }
}

