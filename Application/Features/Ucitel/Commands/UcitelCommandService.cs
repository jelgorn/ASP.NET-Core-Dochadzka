using EvidenciaStudentov.Application.Common.Results;
using EvidenciaStudentov.Infrastructure.Persistence;
using EvidenciaStudentov.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EvidenciaStudentov.Application.Features.Ucitel.Commands;

public sealed class UcitelCommandService : IUcitelCommandService
{
    private readonly ApplicationDbContext _context;
    private readonly PasswordHasher<Pouzivatel> _passwordHasher;

    public UcitelCommandService(ApplicationDbContext context)
    {
        _context = context;
        _passwordHasher = new PasswordHasher<Pouzivatel>();
    }

    public async Task<CommandResult> AddZnamkuAsync(AddUcitelZnamkaCommand command, CancellationToken cancellationToken = default)
    {
        if (command.Hodnota < 1 || command.Hodnota > 5)
        {
            return new CommandResult(false, Error: "Hodnota znamky musi byt v rozsahu 1-5.");
        }

        var ziakExists = await _context.Pouzivatelia
            .AnyAsync(x => x.PouzivatelId == command.ZiakId, cancellationToken);

        if (!ziakExists)
        {
            return new CommandResult(false, Error: $"Ziak s ID {command.ZiakId} nebol najdeny.");
        }

        var predmetExists = await _context.Predmety
            .AnyAsync(x => x.PredmetId == command.PredmetId, cancellationToken);

        if (!predmetExists)
        {
            return new CommandResult(false, Error: $"Predmet s ID {command.PredmetId} nebol najdeny.");
        }

        _context.Znamky.Add(new Znamka
        {
            PouzivatelId = command.ZiakId,
            PredmetId = command.PredmetId,
            Hodnota = command.Hodnota,
            Datum = DateTime.Now
        });

        await _context.SaveChangesAsync(cancellationToken);

        return new CommandResult(true, Message: "Znamka bola uspesne pridana.");
    }

    public async Task<CommandResult> HromadnePridajZnamkyAsync(HromadnePridajZnamkyCommand command, CancellationToken cancellationToken = default)
    {
        var predmetExists = await _context.Predmety
            .AnyAsync(x => x.PredmetId == command.PredmetId, cancellationToken);

        if (!predmetExists)
        {
            return new CommandResult(false, Error: "Predmet nebol najdeny.");
        }

        var datumCas = command.Datum.Date + command.Cas;
        var inserted = 0;

        foreach (var znamka in command.Znamky)
        {
            if (znamka.Hodnota < 1 || znamka.Hodnota > 5)
            {
                continue;
            }

            _context.Znamky.Add(new Znamka
            {
                PouzivatelId = znamka.ZiakId,
                PredmetId = command.PredmetId,
                Hodnota = znamka.Hodnota,
                Datum = datumCas
            });

            inserted++;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return inserted > 0
            ? new CommandResult(true, Message: "Znamky boli uspesne ulozene.")
            : new CommandResult(false, Error: "Nebola pridana ziadna validna znamka.");
    }

    public async Task<CommandResult> UlozDochadzkuAsync(UlozDochadzkuCommand command, CancellationToken cancellationToken = default)
    {
        foreach (var student in command.Studenti)
        {
            var existujuciZaznam = await _context.Dochadzky
                .FirstOrDefaultAsync(d => d.PredmetId == command.PredmetId
                    && d.PouzivatelId == student.PouzivatelId
                    && d.Datum.Date == command.Datum.Date, cancellationToken);

            if (existujuciZaznam is not null)
            {
                existujuciZaznam.JePritomny = student.JePritomny;
            }
            else
            {
                _context.Dochadzky.Add(new Dochadzka
                {
                    PredmetId = command.PredmetId,
                    PouzivatelId = student.PouzivatelId,
                    Datum = command.Datum.Date,
                    JePritomny = student.JePritomny
                });
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
        return new CommandResult(true, Message: "Dochadzka bola uspesne ulozena.");
    }

    public async Task<CommandResult> UpravitProfilAsync(UpravitUcitelProfilCommand command, CancellationToken cancellationToken = default)
    {
        var pouzivatel = await _context.Pouzivatelia
            .FirstOrDefaultAsync(x => x.PouzivatelId == command.PouzivatelId, cancellationToken);

        if (pouzivatel is null)
        {
            return new CommandResult(false, Error: "Pouzivatel nebol najdeny.");
        }

        var verification = _passwordHasher.VerifyHashedPassword(pouzivatel, pouzivatel.Heslo, command.AktualneHeslo);
        if (verification == PasswordVerificationResult.Failed)
        {
            return new CommandResult(false, Error: "Zadane heslo je nespravne.");
        }

        pouzivatel.Email = command.Email;

        if (!string.IsNullOrWhiteSpace(command.NoveHeslo))
        {
            pouzivatel.Heslo = _passwordHasher.HashPassword(pouzivatel, command.NoveHeslo);
        }

        _context.Pouzivatelia.Update(pouzivatel);
        await _context.SaveChangesAsync(cancellationToken);

        return new CommandResult(true, Message: "Udaje boli uspesne zmenene.");
    }
}
