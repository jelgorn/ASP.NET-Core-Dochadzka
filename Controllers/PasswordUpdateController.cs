using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EvidenciaStudentov.Infrastructure.Persistence;
using EvidenciaStudentov.Models;

namespace ASP_NET_Bakalarka.Controllers
{
    public class PasswordUpdateController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<Pouzivatel> _passwordHasher;

        public PasswordUpdateController(ApplicationDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<Pouzivatel>();
        }

        // GET: /PasswordUpdate/UpdateAll
        // Tento endpoint prejde všetkých používateľov a aktualizuje heslá, ktoré nie sú zahashované.
        public async Task<IActionResult> UpdateAll()
        {
            var users = _context.Pouzivatelia.ToList();

            foreach (var user in users)
            {
                // Predpokladáme, že zahashované heslo má formát, ktorý zvyčajne začína "AQAAAA"
                // Toto je heuristika – upravte podmienku podľa vašich potrieb.
                if (!user.Heslo.StartsWith("AQAAAA"))
                {
                    // V tomto prípade predpokladáme, že heslo je v plain texte a treba ho zahashovať.
                    user.Heslo = _passwordHasher.HashPassword(user, user.Heslo);
                }
            }

            await _context.SaveChangesAsync();
            return Content("Všetky heslá boli aktualizované.");
        }
    }
}


