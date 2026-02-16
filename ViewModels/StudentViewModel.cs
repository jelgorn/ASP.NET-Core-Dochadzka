namespace EvidenciaStudentov.ViewModels;

public class StudentViewModel
{
    public int PouzivatelId { get; set; }
    public string Meno { get; set; } = string.Empty;
    public string Priezvisko { get; set; } = string.Empty;
    public ZnamkaViewModel? NovaZnamka { get; set; }
    public List<PredmetDetail> Predmety { get; set; } = new();
    public List<DochadzkaDetail> Dochadzky { get; set; } = new();
}
