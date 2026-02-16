namespace EvidenciaStudentov.ViewModels;

public class UcitelViewModel
{
    public List<PredmetDetail> Predmety { get; set; } = new();
    public int PredmetId { get; set; }
    public string Nazov { get; set; } = string.Empty;
    public string? Popis { get; set; }
}
