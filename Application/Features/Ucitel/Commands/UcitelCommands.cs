namespace EvidenciaStudentov.Application.Features.Ucitel.Commands;

public sealed record AddUcitelZnamkaCommand(int ZiakId, int PredmetId, int Hodnota);

public sealed record HromadnaZnamkaItemCommand(int ZiakId, int Hodnota);

public sealed record HromadnePridajZnamkyCommand(
    int PredmetId,
    IReadOnlyCollection<HromadnaZnamkaItemCommand> Znamky,
    DateTime Datum,
    TimeSpan Cas);

public sealed record UlozDochadzkuStudentCommand(int PouzivatelId, bool JePritomny);

public sealed record UlozDochadzkuCommand(
    int PredmetId,
    DateTime Datum,
    IReadOnlyCollection<UlozDochadzkuStudentCommand> Studenti);

public sealed record UpravitUcitelProfilCommand(
    int PouzivatelId,
    string Email,
    string AktualneHeslo,
    string? NoveHeslo);
