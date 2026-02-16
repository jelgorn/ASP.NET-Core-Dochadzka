using EvidenciaStudentov.Application.Common.Results;

namespace EvidenciaStudentov.Application.Features.Ucitel.Commands;

public interface IUcitelCommandService
{
    Task<CommandResult> AddZnamkuAsync(AddUcitelZnamkaCommand command, CancellationToken cancellationToken = default);
    Task<CommandResult> HromadnePridajZnamkyAsync(HromadnePridajZnamkyCommand command, CancellationToken cancellationToken = default);
    Task<CommandResult> UlozDochadzkuAsync(UlozDochadzkuCommand command, CancellationToken cancellationToken = default);
    Task<CommandResult> UpravitProfilAsync(UpravitUcitelProfilCommand command, CancellationToken cancellationToken = default);
}
