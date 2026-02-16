using EvidenciaStudentov.Domain.Constants;
using EvidenciaStudentov.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EvidenciaStudentov.Models;

public class AuthController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly PasswordHasher<Pouzivatel> _passwordHasher;

    public AuthController(ApplicationDbContext context)
    {
        _context = context;
        _passwordHasher = new PasswordHasher<Pouzivatel>();
    }

    private IActionResult RedirectToHomeByRole(string rola)
    {
        switch (rola.ToLowerInvariant())
        {
            case RoleNames.Admin:
                return RedirectToAction("Index", "Admin");
            case RoleNames.Ucitel:
                return RedirectToAction("Index", "Ucitel");
            case RoleNames.Ziak:
                return RedirectToAction("Index", "Student");
            default:
                return RedirectToAction("Index", "Home");
        }
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View(new LoginViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData["LoginError"] = "Neplatné údaje. Skontroluj formulár.";
            return RedirectToAction("Index", "Home");
        }

        // Vyhľadanie používateľa v databáze
        var pouzivatel = _context.Pouzivatelia.FirstOrDefault(u => u.Email == model.Email);

        if (pouzivatel == null)
        {
            TempData["LoginError"] = "Neplatný email alebo heslo.";
            return RedirectToAction("Index", "Home");
        }

        // Overenie hesla pomocou PasswordHasher
        var verificationResult = _passwordHasher.VerifyHashedPassword(pouzivatel, pouzivatel.Heslo, model.Heslo);
        if (verificationResult == PasswordVerificationResult.Failed)
        {
            TempData["LoginError"] = "Neplatný email alebo heslo.";
            return RedirectToAction("Index", "Home");
        }

        // Nastavenie claimov
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, pouzivatel.PouzivatelId.ToString()),
            new Claim(ClaimTypes.Email, pouzivatel.Email),
            new Claim(ClaimTypes.Role, pouzivatel.Rola)
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        // Prihlásenie používateľa
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

        return RedirectToHomeByRole(pouzivatel.Rola);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
}




