namespace EvidenciaStudentov.ViewModels
{
    public class VytvorDochadzkuViewModel
    {
        public int PredmetId { get; set; }
        public string PredmetNazov { get; set; } = string.Empty;
        public DateTime Datum { get; set; }
        public List<DenDochadzkyViewModel> Studenti { get; set; } = new();
    }
}

