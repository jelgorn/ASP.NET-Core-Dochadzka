namespace EvidenciaStudentov.Application.Features.Student.Queries;

public sealed record GetStudentProfilQuery(int StudentId);
public sealed record GetStudentUpozorneniaQuery(int StudentId);
public sealed record GetStudentPredmetyQuery(int StudentId);
public sealed record GetStudentVsetkyZnamkyQuery();
public sealed record GetStudentDochadzkaQuery(int StudentId);
