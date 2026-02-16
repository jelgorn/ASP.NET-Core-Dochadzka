using System.Security.Claims;
using System.Threading.Tasks;
using EvidenciaStudentov.Infrastructure.Persistence;
using EvidenciaStudentov.Domain.Constants;
using EvidenciaStudentov.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASP_NET_Bakalarka.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<Pouzivatel> _passwordHasher;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<Pouzivatel>();
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Získame ID aktuálne prihláseného používateľa z Claimov
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized();

            var user = await _context.Pouzivatelia.FindAsync(userId);
            if (user == null)
                return NotFound();

            // Overíme aktuálne heslo
            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.Heslo, model.CurrentPassword);
            if (verificationResult == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("CurrentPassword", "Zadané aktuálne heslo je nesprávne.");
                return View(model);
            }

            // Zahashujeme a uložíme nové heslo
            user.Heslo = _passwordHasher.HashPassword(user, model.NewPassword);
            _context.Pouzivatelia.Update(user);
            await _context.SaveChangesAsync();

            // Nastavíme hlásenie o úspechu, ktoré bude dostupné po presmerovaní
            TempData["Success"] = "Heslo bolo úspešne zmenené.";

            // Podľa role používateľa presmerujeme na príslušnú profilovú stránku
            var role = user.Rola.ToLowerInvariant();
            switch (role)
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
    }
}



