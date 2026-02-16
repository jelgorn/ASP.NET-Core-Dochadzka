namespace EvidenciaStudentov.Application.Common.Results;

public sealed record CommandResult(bool Succeeded, string? Message = null, string? Error = null);
