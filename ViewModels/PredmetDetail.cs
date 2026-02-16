namespace EvidenciaStudentov.ViewModels;

public class PredmetDetail
{
    public int PredmetId { get; set; }
    public string Nazov { get; set; } = string.Empty;
    public double Priemer { get; set; }
    public string? Popis { get; set; }
    public List<ZiakDetail> Ziaci { get; set; } = new();
    public List<ZnamkaDetail> Znamky { get; set; } = new();
}
