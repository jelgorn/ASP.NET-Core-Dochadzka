using EvidenciaStudentov.Application.Features.Ucitel.Commands;
using EvidenciaStudentov.Application.Features.Ucitel.DTOs;
using EvidenciaStudentov.Application.Features.Ucitel.Queries;
using EvidenciaStudentov.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ASP_NET_Bakalarka.Controllers;

public class UcitelController : Controller
{
    private readonly IUcitelQueryService _queryService;
    private readonly IUcitelCommandService _commandService;

    public UcitelController(IUcitelQueryService queryService, IUcitelCommandService commandService)
    {
        _queryService = queryService;
        _commandService = commandService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        if (!TryGetCurrentUserId(out var pouzivatelId))
        {
            return Unauthorized();
        }

        var dto = await _queryService.GetProfilAsync(new GetUcitelProfilQuery(pouzivatelId), cancellationToken);
        if (dto is null)
        {
            return NotFound("Ucitel nebol najdeny.");
        }

        return View(MapToUcitelProfilViewModel(dto));
    }

    public async Task<IActionResult> Znamky(CancellationToken cancellationToken)
    {
        if (!TryGetCurrentUserId(out var pouzivatelId))
        {
            return Unauthorized("Nie ste prihlaseny.");
        }

        var dto = await _queryService.GetZnamkyAsync(new GetUcitelZnamkyQuery(pouzivatelId), cancellationToken);
        if (dto is null)
        {
            return NotFound("Ucitel nebol najdeny.");
        }

        return View(MapToUcitelViewModel(dto));
    }

    [HttpPost]
    public async Task<IActionResult> PridajZnamku(int ziakId, int predmetId, int hodnota, CancellationToken cancellationToken)
    {
        var result = await _commandService.AddZnamkuAsync(new AddUcitelZnamkaCommand(ziakId, predmetId, hodnota), cancellationToken);
        if (!result.Succeeded)
        {
            return BadRequest(result.Error);
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> VytvorDochadzku(int predmetId, DateTime? datum, CancellationToken cancellationToken)
    {
        var dto = await _queryService.GetDochadzkaFormAsync(new GetUcitelDochadzkaFormQuery(predmetId, datum), cancellationToken);
        if (dto is null)
        {
            return NotFound($"Predmet s ID {predmetId} nebol najdeny.");
        }

        var model = new VytvorDochadzkuViewModel
        {
            PredmetId = dto.PredmetId,
            PredmetNazov = dto.PredmetNazov,
            Datum = dto.Datum,
            Studenti = dto.Studenti.Select(x => new DenDochadzkyViewModel
            {
                PouzivatelId = x.PouzivatelId,
                Meno = x.Meno,
                Priezvisko = x.Priezvisko,
                JePritomny = x.JePritomny
            }).ToList()
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> UlozDochadzku(VytvorDochadzkuViewModel model, CancellationToken cancellationToken)
    {
        try
        {
            var command = new UlozDochadzkuCommand(
                model.PredmetId,
                model.Datum.Date,
                (model.Studenti ?? new List<DenDochadzkyViewModel>())
                    .Select(x => new UlozDochadzkuStudentCommand(x.PouzivatelId, x.JePritomny))
                    .ToList());

            var result = await _commandService.UlozDochadzkuAsync(command, cancellationToken);
            if (result.Succeeded)
            {
                TempData["Message"] = result.Message ?? "Dochadzka bola uspesne ulozena.";
            }
            else
            {
                TempData["ErrorMessage"] = result.Error ?? "Chyba pri ukladani dochadzky.";
            }
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Chyba pri ukladani udajov: {ex.Message}";
        }

        TempData["OtvorKartu"] = model.PredmetId;
        return RedirectToAction(nameof(Znamky));
    }

    [HttpPost]
    public async Task<IActionResult> HromadnePridajZnamky(
        int predmetId,
        List<HromadnePridajZnamkyItemViewModel> znamky,
        DateTime? datum,
        TimeSpan? cas,
        CancellationToken cancellationToken)
    {
        var datumValue = (datum ?? DateTime.Now).Date;
        var casValue = cas ?? DateTime.Now.TimeOfDay;

        var command = new HromadnePridajZnamkyCommand(
            predmetId,
            (znamky ?? new List<HromadnePridajZnamkyItemViewModel>())
                .Select(x => new HromadnaZnamkaItemCommand(x.ZiakId, x.Hodnota))
                .ToList(),
            datumValue,
            casValue);

        var result = await _commandService.HromadnePridajZnamkyAsync(command, cancellationToken);
        if (result.Succeeded)
        {
            TempData["Message"] = result.Message ?? "Znamky boli uspesne ulozene.";
        }
        else
        {
            TempData["ErrorMessage"] = result.Error ?? "Nepodarilo sa ulozit znamky.";
        }

        return RedirectToAction(nameof(Znamky));
    }

    [HttpGet]
    public async Task<IActionResult> UpravitProfil(CancellationToken cancellationToken)
    {
        if (!TryGetCurrentUserId(out var pouzivatelId))
        {
            return Unauthorized();
        }

        var dto = await _queryService.GetUpravitProfilAsync(new GetUcitelUpravitProfilQuery(pouzivatelId), cancellationToken);
        if (dto is null)
        {
            return NotFound();
        }

        var model = new UcitelProfilViewModel.UpravitProfilViewModel
        {
            Email = dto.Email
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> UpravitProfil(UcitelProfilViewModel.UpravitProfilViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (!TryGetCurrentUserId(out var pouzivatelId))
        {
            return Unauthorized();
        }

        var command = new UpravitUcitelProfilCommand(
            pouzivatelId,
            model.Email,
            model.AktualneHeslo,
            string.IsNullOrWhiteSpace(model.NoveHeslo) ? null : model.NoveHeslo);

        var result = await _commandService.UpravitProfilAsync(command, cancellationToken);
        if (!result.Succeeded)
        {
            ModelState.AddModelError(nameof(model.AktualneHeslo), result.Error ?? "Nepodarilo sa upravit profil.");
            return View(model);
        }

        TempData["Success"] = result.Message ?? "Udaje boli uspesne zmenene.";
        return RedirectToAction(nameof(Index));
    }

    private static UcitelProfilViewModel MapToUcitelProfilViewModel(UcitelProfilDto dto)
    {
        return new UcitelProfilViewModel
        {
            Meno = dto.Meno,
            Priezvisko = dto.Priezvisko,
            Email = dto.Email,
            DatumNarodenia = dto.DatumNarodenia,
            PocetPredmetov = dto.PocetPredmetov,
            PocetZnamok = dto.PocetZnamok,
            PocetDochadzok = dto.PocetDochadzok,
            PocetStudentov = dto.PocetStudentov,
            PoslednaZmena = dto.PoslednaZmena,
            Predmety = dto.Predmety.Select(x => new UcitelProfilViewModel.PredmetInfo
            {
                PredmetId = x.PredmetId,
                Nazov = x.Nazov,
                Popis = x.Popis,
                PocetZakov = x.PocetZiakov
            }).ToList()
        };
    }

    private static UcitelViewModel MapToUcitelViewModel(UcitelZnamkyPageDto dto)
    {
        return new UcitelViewModel
        {
            Predmety = dto.Predmety.Select(x => new PredmetDetail
            {
                PredmetId = x.PredmetId,
                Nazov = x.Nazov,
                Popis = x.Popis,
                Ziaci = x.Ziaci.Select(z => new ZiakDetail
                {
                    ZiakId = z.ZiakId,
                    Meno = z.Meno,
                    Priezvisko = z.Priezvisko,
                    Priemer = z.Priemer
                }).ToList()
            }).ToList()
        };
    }

    private bool TryGetCurrentUserId(out int pouzivatelId)
    {
        var pouzivatelIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(pouzivatelIdClaim, out pouzivatelId);
    }
}
