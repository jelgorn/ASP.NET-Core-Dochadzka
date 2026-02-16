namespace EvidenciaStudentov.ViewModels;

public class PredmetViewModel
{
    public int PredmetViewId { get; set; }
    public string Nazov { get; set; } = string.Empty;
    public string? Popis { get; set; }
    public string? Ucitel { get; set; }
    public int PocetZiakov { get; set; }
}
