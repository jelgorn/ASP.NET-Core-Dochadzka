using EvidenciaStudentov.Application.Features.Student.DTOs;

namespace EvidenciaStudentov.Application.Features.Student.Queries;

public interface IStudentQueryService
{
    Task<StudentProfilDto?> GetProfilAsync(GetStudentProfilQuery query, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<StudentNovaZnamkaDto>> GetUpozorneniaAsync(GetStudentUpozorneniaQuery query, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<StudentPredmetDto>> GetPredmetyAsync(GetStudentPredmetyQuery query, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<StudentPredmetZnamkyDto>> GetVsetkyZnamkyAsync(GetStudentVsetkyZnamkyQuery query, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<StudentDochadzkaDto>> GetDochadzkaAsync(GetStudentDochadzkaQuery query, CancellationToken cancellationToken = default);
}
