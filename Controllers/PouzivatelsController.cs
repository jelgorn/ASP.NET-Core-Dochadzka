using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EvidenciaStudentov.Infrastructure.Persistence;
using EvidenciaStudentov.Models;
using Microsoft.AspNetCore.Identity;

namespace ASP_NET_Bakalarka.Controllers
{
    public class PouzivatelsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<Pouzivatel> _passwordHasher;

        public PouzivatelsController(ApplicationDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<Pouzivatel>();
        }

        // GET: Pouzivatels
        public async Task<IActionResult> Index()
        {
            return View(await _context.Pouzivatelia.ToListAsync());
        }

        // GET: Pouzivatels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pouzivatel = await _context.Pouzivatelia
                .FirstOrDefaultAsync(m => m.PouzivatelId == id);
            if (pouzivatel == null)
            {
                return NotFound();
            }

            return View(pouzivatel);
        }

        // GET: Pouzivatels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pouzivatels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PouzivatelId,Meno,Priezvisko,DatumNarodenia,Email,Heslo,Rola")] Pouzivatel pouzivatel)
        {
            if (ModelState.IsValid)
            {
                // Hashovanie hesla pomocou PasswordHasher – salt a hashovanie sa riešia interné
                pouzivatel.Heslo = _passwordHasher.HashPassword(pouzivatel, pouzivatel.Heslo);
                _context.Add(pouzivatel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pouzivatel);
        }

        // GET: Pouzivatels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pouzivatel = await _context.Pouzivatelia.FindAsync(id);
            if (pouzivatel == null)
            {
                return NotFound();
            }
            return View(pouzivatel);
        }

        // POST: Pouzivatels/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PouzivatelId,Meno,Priezvisko,DatumNarodenia,Email,Heslo,Rola")] Pouzivatel pouzivatel)
        {
            if (id != pouzivatel.PouzivatelId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Ak sa heslo zmenilo, je vhodné ho opätovne zahashovať
                    pouzivatel.Heslo = _passwordHasher.HashPassword(pouzivatel, pouzivatel.Heslo);

                    _context.Update(pouzivatel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PouzivatelExists(pouzivatel.PouzivatelId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(pouzivatel);
        }

        // GET: Pouzivatels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pouzivatel = await _context.Pouzivatelia
                .FirstOrDefaultAsync(m => m.PouzivatelId == id);
            if (pouzivatel == null)
            {
                return NotFound();
            }

            return View(pouzivatel);
        }

        // POST: Pouzivatels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pouzivatel = await _context.Pouzivatelia.FindAsync(id);
            if (pouzivatel != null)
            {
                _context.Pouzivatelia.Remove(pouzivatel);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PouzivatelExists(int id)
        {
            return _context.Pouzivatelia.Any(e => e.PouzivatelId == id);
        }
    }
}


