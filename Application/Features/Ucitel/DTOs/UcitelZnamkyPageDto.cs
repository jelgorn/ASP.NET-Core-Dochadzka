namespace EvidenciaStudentov.Application.Features.Ucitel.DTOs;

public sealed class UcitelZnamkyPageDto
{
    public List<UcitelPredmetDetailDto> Predmety { get; set; } = new();
}

public sealed class UcitelPredmetDetailDto
{
    public int PredmetId { get; set; }
    public string Nazov { get; set; } = string.Empty;
    public string? Popis { get; set; }
    public List<UcitelZiakDto> Ziaci { get; set; } = new();
}

public sealed class UcitelZiakDto
{
    public int ZiakId { get; set; }
    public string Meno { get; set; } = string.Empty;
    public string Priezvisko { get; set; } = string.Empty;
    public double Priemer { get; set; }
}
