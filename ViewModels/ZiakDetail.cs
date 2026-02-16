namespace EvidenciaStudentov.ViewModels;

public class ZiakDetail
{
    public int ZiakId { get; set; }
    public string Meno { get; set; } = string.Empty;
    public string Priezvisko { get; set; } = string.Empty;
    public double Priemer { get; set; }
    public bool JePritomny { get; set; }
    public int PouzivatelId { get; set; }
    public List<ZnamkaDetail> Znamky { get; set; } = new();
}
