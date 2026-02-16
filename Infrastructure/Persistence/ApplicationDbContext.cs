using EvidenciaStudentov.Models;
using Microsoft.EntityFrameworkCore;

namespace EvidenciaStudentov.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Predmet> Predmety { get; set; }
    public DbSet<Znamka> Znamky { get; set; }
    public DbSet<Dochadzka> Dochadzky { get; set; }
    public DbSet<PriradeniePredmetovUcitelom> PriradeniaPredmetovUcitelom { get; set; }
    public DbSet<PriradeniePredmetovZiakom> PriradeniePredmetovZiakom { get; set; }
    public DbSet<Pouzivatel> Pouzivatelia { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}

