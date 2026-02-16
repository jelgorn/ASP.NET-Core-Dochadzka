using EvidenciaStudentov.Application.Features.Ucitel.DTOs;
using EvidenciaStudentov.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EvidenciaStudentov.Application.Features.Ucitel.Queries;

public sealed class UcitelQueryService : IUcitelQueryService
{
    private readonly ApplicationDbContext _context;

    public UcitelQueryService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UcitelProfilDto?> GetProfilAsync(GetUcitelProfilQuery query, CancellationToken cancellationToken = default)
    {
        var ucitel = await _context.Pouzivatelia
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PouzivatelId == query.PouzivatelId, cancellationToken);

        if (ucitel is null)
        {
            return null;
        }

        var priradenePredmety = await _context.PriradeniaPredmetovUcitelom
            .AsNoTracking()
            .Where(x => x.PouzivatelId == query.PouzivatelId)
            .Select(x => new UcitelPredmetInfoDto
            {
                PredmetId = x.PredmetId,
                Nazov = x.Predmet != null ? x.Predmet.Nazov : string.Empty,
                Popis = x.Predmet != null ? x.Predmet.Popis : null,
                PocetZiakov = _context.PriradeniePredmetovZiakom.Count(z => z.PredmetId == x.PredmetId)
            })
            .ToListAsync(cancellationToken);

        var predmetIds = priradenePredmety.Select(x => x.PredmetId).ToList();

        var pocetStudentov = await _context.PriradeniePredmetovZiakom
            .AsNoTracking()
            .Where(x => predmetIds.Contains(x.PredmetId))
            .Select(x => x.PouzivatelId)
            .Distinct()
            .CountAsync(cancellationToken);

        var pocetZnamok = await _context.Znamky
            .AsNoTracking()
            .CountAsync(x => predmetIds.Contains(x.PredmetId), cancellationToken);

        var pocetDochadzok = await _context.Dochadzky
            .AsNoTracking()
            .CountAsync(x => predmetIds.Contains(x.PredmetId), cancellationToken);

        return new UcitelProfilDto
        {
            Meno = ucitel.Meno,
            Priezvisko = ucitel.Priezvisko,
            Email = ucitel.Email,
            DatumNarodenia = ucitel.DatumNarodenia,
            Predmety = priradenePredmety,
            PocetStudentov = pocetStudentov,
            PocetZnamok = pocetZnamok,
            PocetDochadzok = pocetDochadzok,
            PocetPredmetov = priradenePredmety.Count,
            PoslednaZmena = DateTime.Now
        };
    }

    public async Task<UcitelZnamkyPageDto?> GetZnamkyAsync(GetUcitelZnamkyQuery query, CancellationToken cancellationToken = default)
    {
        var ucitelExists = await _context.Pouzivatelia
            .AsNoTracking()
            .AnyAsync(x => x.PouzivatelId == query.PouzivatelId, cancellationToken);

        if (!ucitelExists)
        {
            return null;
        }

        var predmety = await _context.PriradeniaPredmetovUcitelom
            .AsNoTracking()
            .Where(x => x.PouzivatelId == query.PouzivatelId)
            .Select(x => new
            {
                x.PredmetId,
                Nazov = x.Predmet != null ? x.Predmet.Nazov : string.Empty,
                Popis = x.Predmet != null ? x.Predmet.Popis : null
            })
            .Distinct()
            .ToListAsync(cancellationToken);

        var result = new UcitelZnamkyPageDto();

        foreach (var predmet in predmety)
        {
            var ziaci = await _context.PriradeniePredmetovZiakom
                .AsNoTracking()
                .Where(x => x.PredmetId == predmet.PredmetId)
                .Select(x => new
                {
                    x.PouzivatelId,
                    Meno = x.Pouzivatel != null ? x.Pouzivatel.Meno : string.Empty,
                    Priezvisko = x.Pouzivatel != null ? x.Pouzivatel.Priezvisko : string.Empty
                })
                .ToListAsync(cancellationToken);

            var priemery = await _context.Znamky
                .AsNoTracking()
                .Where(x => x.PredmetId == predmet.PredmetId)
                .GroupBy(x => x.PouzivatelId)
                .Select(g => new { PouzivatelId = g.Key, Priemer = g.Average(x => (double)x.Hodnota) })
                .ToDictionaryAsync(x => x.PouzivatelId, x => x.Priemer, cancellationToken);

            var predmetDto = new UcitelPredmetDetailDto
            {
                PredmetId = predmet.PredmetId,
                Nazov = predmet.Nazov,
                Popis = predmet.Popis,
                Ziaci = ziaci.Select(z => new UcitelZiakDto
                {
                    ZiakId = z.PouzivatelId,
                    Meno = string.Concat(z.Meno, " ", z.Priezvisko).Trim(),
                    Priezvisko = z.Priezvisko,
                    Priemer = priemery.TryGetValue(z.PouzivatelId, out var priemer) ? priemer : 0
                }).ToList()
            };

            result.Predmety.Add(predmetDto);
        }

        return result;
    }

    public async Task<UcitelDochadzkaFormDto?> GetDochadzkaFormAsync(GetUcitelDochadzkaFormQuery query, CancellationToken cancellationToken = default)
    {
        var vybranyDatum = (query.Datum ?? DateTime.Now).Date;

        var predmet = await _context.Predmety
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PredmetId == query.PredmetId, cancellationToken);

        if (predmet is null)
        {
            return null;
        }

        var studenti = await _context.PriradeniePredmetovZiakom
            .AsNoTracking()
            .Where(x => x.PredmetId == query.PredmetId)
            .Select(x => new
            {
                x.PouzivatelId,
                Meno = x.Pouzivatel != null ? x.Pouzivatel.Meno : string.Empty,
                Priezvisko = x.Pouzivatel != null ? x.Pouzivatel.Priezvisko : string.Empty
            })
            .ToListAsync(cancellationToken);

        var existujucaDochadzka = await _context.Dochadzky
            .AsNoTracking()
            .Where(x => x.PredmetId == query.PredmetId && x.Datum.Date == vybranyDatum)
            .ToListAsync(cancellationToken);

        var dto = new UcitelDochadzkaFormDto
        {
            PredmetId = predmet.PredmetId,
            PredmetNazov = predmet.Nazov,
            Datum = vybranyDatum,
            Studenti = studenti
                .Select(x =>
                {
                    var stav = existujucaDochadzka
                        .FirstOrDefault(d => d.PouzivatelId == x.PouzivatelId)?.JePritomny;

                    return new UcitelDochadzkaStudentDto
                    {
                        PouzivatelId = x.PouzivatelId,
                        Meno = x.Meno,
                        Priezvisko = x.Priezvisko,
                        JePritomny = stav ?? false
                    };
                })
                .ToList()
        };

        return dto;
    }

    public async Task<UcitelUpravitProfilDto?> GetUpravitProfilAsync(GetUcitelUpravitProfilQuery query, CancellationToken cancellationToken = default)
    {
        var pouzivatel = await _context.Pouzivatelia
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PouzivatelId == query.PouzivatelId, cancellationToken);

        if (pouzivatel is null)
        {
            return null;
        }

        return new UcitelUpravitProfilDto
        {
            Email = pouzivatel.Email
        };
    }
}
