using EvidenciaStudentov.Application.Features.Student.DTOs;
using EvidenciaStudentov.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EvidenciaStudentov.Application.Features.Student.Queries;

public sealed class StudentQueryService : IStudentQueryService
{
    private readonly ApplicationDbContext _context;

    public StudentQueryService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StudentProfilDto?> GetProfilAsync(GetStudentProfilQuery query, CancellationToken cancellationToken = default)
    {
        var student = await _context.Pouzivatelia
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PouzivatelId == query.StudentId, cancellationToken);

        if (student is null)
        {
            return null;
        }

        var znamky = await _context.Znamky
            .AsNoTracking()
            .Where(x => x.PouzivatelId == query.StudentId)
            .Select(x => new StudentZnamkaDto
            {
                Predmet = x.Predmet != null ? x.Predmet.Nazov : string.Empty,
                Hodnota = x.Hodnota,
                Datum = x.Datum
            })
            .ToListAsync(cancellationToken);

        var dochadzky = await _context.Dochadzky
            .AsNoTracking()
            .Where(x => x.PouzivatelId == query.StudentId)
            .Select(x => new StudentDochadzkaDto
            {
                Predmet = x.Predmet != null ? x.Predmet.Nazov : string.Empty,
                JePritomny = x.JePritomny,
                Datum = x.Datum
            })
            .ToListAsync(cancellationToken);

        var znamkaGroups = znamky
            .GroupBy(z => z.Predmet)
            .Select(g => new
            {
                Predmet = g.Key,
                Priemer = g.Average(x => x.Hodnota)
            })
            .ToList();

        var najhorsiPredmet = znamkaGroups
            .OrderByDescending(g => g.Priemer)
            .FirstOrDefault()?.Predmet ?? "Ziadne znamky";

        var poslednaZnamka = znamky
            .OrderByDescending(z => z.Datum)
            .FirstOrDefault()?.Datum;

        return new StudentProfilDto
        {
            Meno = student.Meno,
            Priezvisko = student.Priezvisko,
            Email = student.Email,
            DatumNarodenia = student.DatumNarodenia,
            NajhorsiPredmet = najhorsiPredmet,
            DatumPoslednejZnamky = poslednaZnamka,
            PocetPritomnosti = dochadzky.Count(d => d.JePritomny),
            PocetNepritomnosti = dochadzky.Count(d => !d.JePritomny),
            Znamky = znamky,
            Dochadzky = dochadzky
        };
    }

    public async Task<IReadOnlyList<StudentNovaZnamkaDto>> GetUpozorneniaAsync(GetStudentUpozorneniaQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _context.Znamky
            .AsNoTracking()
            .Where(x => x.PouzivatelId == query.StudentId)
            .OrderByDescending(x => x.Datum)
            .Select(x => new StudentNovaZnamkaDto
            {
                Predmet = x.Predmet != null ? x.Predmet.Nazov : "Neznamy predmet",
                Hodnota = x.Hodnota,
                Datum = x.Datum
            })
            .ToListAsync(cancellationToken);

        return result;
    }

    public async Task<IReadOnlyList<StudentPredmetDto>> GetPredmetyAsync(GetStudentPredmetyQuery query, CancellationToken cancellationToken = default)
    {
        var predmety = await _context.PriradeniePredmetovZiakom
            .AsNoTracking()
            .Where(x => x.PouzivatelId == query.StudentId)
            .Select(x => new StudentPredmetDto
            {
                PredmetId = x.PredmetId,
                Nazov = x.Predmet != null ? x.Predmet.Nazov : string.Empty,
                Priemer = _context.Znamky
                    .Where(z => z.PouzivatelId == query.StudentId && z.PredmetId == x.PredmetId)
                    .Select(z => (double?)z.Hodnota)
                    .Average() ?? 0
            })
            .ToListAsync(cancellationToken);

        return predmety;
    }

    public async Task<IReadOnlyList<StudentPredmetZnamkyDto>> GetVsetkyZnamkyAsync(GetStudentVsetkyZnamkyQuery query, CancellationToken cancellationToken = default)
    {
        var predmety = await _context.Predmety
            .AsNoTracking()
            .Select(p => new StudentPredmetZnamkyDto
            {
                Nazov = p.Nazov,
                Priemer = p.Znamky.Any() ? p.Znamky.Average(z => z.Hodnota) : 0,
                Znamky = p.Znamky
                    .Select(z => new StudentZnamkaDetailDto
                    {
                        Hodnota = z.Hodnota,
                        Datum = z.Datum
                    })
                    .ToList()
            })
            .ToListAsync(cancellationToken);

        return predmety;
    }

    public async Task<IReadOnlyList<StudentDochadzkaDto>> GetDochadzkaAsync(GetStudentDochadzkaQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _context.Dochadzky
            .AsNoTracking()
            .Where(x => x.PouzivatelId == query.StudentId)
            .OrderBy(x => x.Datum)
            .Select(x => new StudentDochadzkaDto
            {
                Predmet = x.Predmet != null ? x.Predmet.Nazov : string.Empty,
                Datum = x.Datum,
                JePritomny = x.JePritomny
            })
            .ToListAsync(cancellationToken);

        return result;
    }
}
