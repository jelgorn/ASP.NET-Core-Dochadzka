namespace EvidenciaStudentov.ViewModels
{
    public class DochadzkaViewModel
    {
        public int PredmetId { get; set; }
        public string Predmet { get; set; } = string.Empty;
        public DateTime Datum { get; set; }
        public bool JePritomny { get; set; }
        public List<ZiakDetail> Studenti { get; set; } = new();
        public string PredmetNazov { get; set; } = string.Empty;
    }
}
