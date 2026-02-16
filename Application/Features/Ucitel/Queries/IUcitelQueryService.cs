using EvidenciaStudentov.Application.Features.Ucitel.DTOs;

namespace EvidenciaStudentov.Application.Features.Ucitel.Queries;

public interface IUcitelQueryService
{
    Task<UcitelProfilDto?> GetProfilAsync(GetUcitelProfilQuery query, CancellationToken cancellationToken = default);
    Task<UcitelZnamkyPageDto?> GetZnamkyAsync(GetUcitelZnamkyQuery query, CancellationToken cancellationToken = default);
    Task<UcitelDochadzkaFormDto?> GetDochadzkaFormAsync(GetUcitelDochadzkaFormQuery query, CancellationToken cancellationToken = default);
    Task<UcitelUpravitProfilDto?> GetUpravitProfilAsync(GetUcitelUpravitProfilQuery query, CancellationToken cancellationToken = default);
}
