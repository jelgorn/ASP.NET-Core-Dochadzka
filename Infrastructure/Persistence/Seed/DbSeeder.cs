using EvidenciaStudentov.Domain.Constants;
using EvidenciaStudentov.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EvidenciaStudentov.Infrastructure.Persistence;

public static class DbSeeder
{
    private const string DemoAdminEmail = "admin@demo.sk";
    private const string DemoAdminPassword = "Admin123!";
    private const string DemoUserEmail = "user@demo.sk";
    private const string DemoUserPassword = "User123!";

    public static async Task SeedDefaultUsersAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await context.Database.MigrateAsync();

        await EnsureUserExistsAsync(
            context,
            DemoAdminEmail,
            DemoAdminPassword,
            "Demo",
            "Admin",
            RoleNames.Admin,
            new DateTime(1990, 1, 1));

        await EnsureUserExistsAsync(
            context,
            DemoUserEmail,
            DemoUserPassword,
            "Demo",
            "User",
            RoleNames.Ziak,
            new DateTime(2006, 1, 1));

        await context.SaveChangesAsync();
    }

    private static async Task EnsureUserExistsAsync(
        ApplicationDbContext context,
        string email,
        string plainPassword,
        string meno,
        string priezvisko,
        string rola,
        DateTime datumNarodenia)
    {
        var exists = await context.Pouzivatelia.AnyAsync(u => u.Email == email);
        if (exists)
        {
            return;
        }

        var user = new Pouzivatel
        {
            Meno = meno,
            Priezvisko = priezvisko,
            DatumNarodenia = datumNarodenia,
            Email = email,
            Rola = rola,
            Heslo = string.Empty
        };

        var hasher = new PasswordHasher<Pouzivatel>();
        user.Heslo = hasher.HashPassword(user, plainPassword);

        context.Pouzivatelia.Add(user);
    }
}
