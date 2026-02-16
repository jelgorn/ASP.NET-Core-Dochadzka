namespace EvidenciaStudentov.Application.Features.Ucitel.DTOs;

public sealed class UcitelProfilDto
{
    public string Meno { get; set; } = string.Empty;
    public string Priezvisko { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime DatumNarodenia { get; set; }
    public int PocetPredmetov { get; set; }
    public int PocetZnamok { get; set; }
    public int PocetDochadzok { get; set; }
    public int PocetStudentov { get; set; }
    public DateTime? PoslednaZmena { get; set; }
    public List<UcitelPredmetInfoDto> Predmety { get; set; } = new();
}

public sealed class UcitelPredmetInfoDto
{
    public int PredmetId { get; set; }
    public string Nazov { get; set; } = string.Empty;
    public string? Popis { get; set; }
    public int PocetZiakov { get; set; }
}
