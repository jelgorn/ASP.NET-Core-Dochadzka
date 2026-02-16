using EvidenciaStudentov.Application.Features.Student.Queries;
using EvidenciaStudentov.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ASP_NET_Bakalarka.Controllers;

public class StudentController : Controller
{
    private readonly IStudentQueryService _queryService;

    public StudentController(IStudentQueryService queryService)
    {
        _queryService = queryService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        if (!TryGetCurrentUserId(out var studentId))
        {
            return Unauthorized();
        }

        var dto = await _queryService.GetProfilAsync(new GetStudentProfilQuery(studentId), cancellationToken);
        if (dto is null)
        {
            return NotFound();
        }

        var model = new StudentProfilViewModel
        {
            Meno = dto.Meno,
            Priezvisko = dto.Priezvisko,
            Email = dto.Email,
            DatumNarodenia = dto.DatumNarodenia,
            NajhorsiPredmet = dto.NajhorsiPredmet,
            DatumPoslednejZnamky = dto.DatumPoslednejZnamky,
            PocetPritomnosti = dto.PocetPritomnosti,
            PocetNepritomnosti = dto.PocetNepritomnosti,
            Znamky = dto.Znamky.Select(x => new ZnamkaViewModel
            {
                Predmet = x.Predmet,
                Hodnota = x.Hodnota,
                Datum = x.Datum
            }).ToList(),
            Dochadzky = dto.Dochadzky.Select(x => new DochadzkaViewModel
            {
                Predmet = x.Predmet,
                JePritomny = x.JePritomny,
                Datum = x.Datum
            }).ToList()
        };

        return View(model);
    }

    public async Task<IActionResult> NoveUpozornenie(CancellationToken cancellationToken)
    {
        if (!TryGetCurrentUserId(out var studentId))
        {
            return Unauthorized("Nie ste prihlaseny.");
        }

        var dtos = await _queryService.GetUpozorneniaAsync(new GetStudentUpozorneniaQuery(studentId), cancellationToken);

        var model = dtos
            .Select(x => new NovaZnamkaViewModel
            {
                Predmet = x.Predmet,
                Hodnota = x.Hodnota,
                Datum = x.Datum
            })
            .ToList();

        return View(model);
    }

    public async Task<IActionResult> Predmety(CancellationToken cancellationToken)
    {
        if (!TryGetCurrentUserId(out var studentId))
        {
            return Unauthorized("Nie ste prihlaseny.");
        }

        var dtos = await _queryService.GetPredmetyAsync(new GetStudentPredmetyQuery(studentId), cancellationToken);

        var model = dtos.Select(x => new PredmetDetail
        {
            PredmetId = x.PredmetId,
            Nazov = x.Nazov,
            Priemer = x.Priemer
        }).ToList();

        return View(model);
    }

    public async Task<IActionResult> VsetkyZnamky(CancellationToken cancellationToken)
    {
        var dtos = await _queryService.GetVsetkyZnamkyAsync(new GetStudentVsetkyZnamkyQuery(), cancellationToken);

        var model = dtos.Select(x => new PredmetZnamkyViewModel
        {
            Nazov = x.Nazov,
            Priemer = x.Priemer,
            Znamky = x.Znamky.Select(z => new ZnamkaDetail
            {
                Hodnota = z.Hodnota,
                Datum = z.Datum
            }).ToList()
        }).ToList();

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Dochadzka(CancellationToken cancellationToken)
    {
        if (!TryGetCurrentUserId(out var studentId))
        {
            return Unauthorized();
        }

        var dtos = await _queryService.GetDochadzkaAsync(new GetStudentDochadzkaQuery(studentId), cancellationToken);

        var model = dtos.Select(x => new DochadzkaViewModel
        {
            Predmet = x.Predmet,
            Datum = x.Datum,
            JePritomny = x.JePritomny
        }).ToList();

        return View(model);
    }

    private bool TryGetCurrentUserId(out int pouzivatelId)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(userIdClaim, out pouzivatelId);
    }
}
