

namespace EvidenciaStudentov.ViewModels
{
    public class PredmetZnamkyViewModel
    {
        public string Nazov { get; set; } = string.Empty;
        public double Priemer { get; set; }
        public List<ZnamkaDetail> Znamky { get; set; } = new();
    }
}

