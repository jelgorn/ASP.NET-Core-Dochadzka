

namespace EvidenciaStudentov.ViewModels
{
    public class ZnamkaViewModel
    {
        public string Predmet { get; set; } = string.Empty;
        public int Hodnota { get; set; }
        public DateTime Datum { get; set; }
    }

    public class StudentZnamkaViewModel
    {
        public int PouzivatelId { get; set; }
        public string Meno { get; set; } = string.Empty;
        public string Priezvisko { get; set; } = string.Empty;
        public int? Znamka { get; set; }
    }
}
