namespace EvidenciaStudentov.Application.Features.Ucitel.DTOs;

public sealed class UcitelDochadzkaFormDto
{
    public int PredmetId { get; set; }
    public string PredmetNazov { get; set; } = string.Empty;
    public DateTime Datum { get; set; }
    public List<UcitelDochadzkaStudentDto> Studenti { get; set; } = new();
}

public sealed class UcitelDochadzkaStudentDto
{
    public int PouzivatelId { get; set; }
    public string Meno { get; set; } = string.Empty;
    public string Priezvisko { get; set; } = string.Empty;
    public bool JePritomny { get; set; }
}
