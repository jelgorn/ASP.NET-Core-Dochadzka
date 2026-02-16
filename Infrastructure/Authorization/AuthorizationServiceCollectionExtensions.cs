using EvidenciaStudentov.Domain.Constants;
using Microsoft.Extensions.DependencyInjection;

namespace EvidenciaStudentov.Infrastructure.Authorization;

public static class AuthorizationServiceCollectionExtensions
{
    public static IServiceCollection AddAppAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(PolicyNames.Admin, policy => policy.RequireRole(RoleNames.Admin));
            options.AddPolicy(PolicyNames.Ucitel, policy => policy.RequireRole(RoleNames.Ucitel));
            options.AddPolicy(PolicyNames.Ziak, policy => policy.RequireRole(RoleNames.Ziak));
        });

        return services;
    }
}

