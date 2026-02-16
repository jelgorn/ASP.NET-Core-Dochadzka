namespace EvidenciaStudentov.Application.Features.Ucitel.Queries;

public sealed record GetUcitelProfilQuery(int PouzivatelId);
public sealed record GetUcitelZnamkyQuery(int PouzivatelId);
public sealed record GetUcitelDochadzkaFormQuery(int PredmetId, DateTime? Datum);
public sealed record GetUcitelUpravitProfilQuery(int PouzivatelId);
