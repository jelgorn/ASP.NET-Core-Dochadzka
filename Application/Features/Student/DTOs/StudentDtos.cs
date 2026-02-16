namespace EvidenciaStudentov.Application.Features.Student.DTOs;

public sealed class StudentProfilDto
{
    public string Meno { get; set; } = string.Empty;
    public string Priezvisko { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime DatumNarodenia { get; set; }
    public string NajhorsiPredmet { get; set; } = string.Empty;
    public DateTime? DatumPoslednejZnamky { get; set; }
    public int PocetPritomnosti { get; set; }
    public int PocetNepritomnosti { get; set; }
    public List<StudentZnamkaDto> Znamky { get; set; } = new();
    public List<StudentDochadzkaDto> Dochadzky { get; set; } = new();
}

public sealed class StudentZnamkaDto
{
    public string Predmet { get; set; } = string.Empty;
    public int Hodnota { get; set; }
    public DateTime Datum { get; set; }
}

public sealed class StudentDochadzkaDto
{
    public string Predmet { get; set; } = string.Empty;
    public DateTime Datum { get; set; }
    public bool JePritomny { get; set; }
}

public sealed class StudentNovaZnamkaDto
{
    public string Predmet { get; set; } = string.Empty;
    public int Hodnota { get; set; }
    public DateTime Datum { get; set; }
}

public sealed class StudentPredmetDto
{
    public int PredmetId { get; set; }
    public string Nazov { get; set; } = string.Empty;
    public double Priemer { get; set; }
}

public sealed class StudentPredmetZnamkyDto
{
    public string Nazov { get; set; } = string.Empty;
    public double Priemer { get; set; }
    public List<StudentZnamkaDetailDto> Znamky { get; set; } = new();
}

public sealed class StudentZnamkaDetailDto
{
    public int Hodnota { get; set; }
    public DateTime Datum { get; set; }
}
