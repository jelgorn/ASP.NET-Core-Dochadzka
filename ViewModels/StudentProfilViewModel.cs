namespace EvidenciaStudentov.ViewModels;

public class StudentProfilViewModel
{
    public string Meno { get; set; } = string.Empty;
    public string Priezvisko { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime DatumNarodenia { get; set; }

    public string NajhorsiPredmet { get; set; } = string.Empty;
    public DateTime? DatumPoslednejZnamky { get; set; }

    public int PocetPritomnosti { get; set; }
    public int PocetNepritomnosti { get; set; }

    public List<ZnamkaViewModel> Znamky { get; set; } = new();
    public List<DochadzkaViewModel> Dochadzky { get; set; } = new();
}

