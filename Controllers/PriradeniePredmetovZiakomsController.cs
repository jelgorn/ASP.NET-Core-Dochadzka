using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EvidenciaStudentov.Infrastructure.Persistence;
using EvidenciaStudentov.Models;

namespace ASP_NET_Bakalarka.Controllers
{
    public class PriradeniePredmetovZiakomsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PriradeniePredmetovZiakomsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PriradeniePredmetovZiakoms
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PriradeniePredmetovZiakom
                .Include(p => p.Pouzivatel)
                .Include(p => p.Predmet);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PriradeniePredmetovZiakoms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var priradeniePredmetovZiakom = await _context.PriradeniePredmetovZiakom
                .Include(p => p.Pouzivatel)
                .Include(p => p.Predmet)
                .FirstOrDefaultAsync(m => m.PriradeniePredZiakId == id);
            if (priradeniePredmetovZiakom == null)
            {
                return NotFound();
            }

            return View(priradeniePredmetovZiakom);
        }

        // GET: PriradeniePredmetovZiakoms/Create
        public IActionResult Create()
        {
            // Filtrovanie používateľov s rolou "žiak" a kombinácia mena a priezviska
            ViewData["PouzivatelId"] = new SelectList(
                _context.Pouzivatelia
                    .Where(p => p.Rola == "ziak")
                    .Select(p => new { p.PouzivatelId, FullName = p.Meno + " " + p.Priezvisko }),
                "PouzivatelId",
                "FullName");

            // Zoznam predmetov
            ViewData["PredmetId"] = new SelectList(
                _context.Predmety
                    .Select(p => new { p.PredmetId, p.Nazov }),
                "PredmetId",
                "Nazov");

            return View();
        }

        // POST: PriradeniePredmetovZiakoms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PriradeniePredZiakId,PouzivatelId,PredmetId")] PriradeniePredmetovZiakom priradeniePredmetovZiakom)
        {
            if (ModelState.IsValid)
            {
                _context.Add(priradeniePredmetovZiakom);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // V prípade chyby opätovne naplniť select list s kombináciou mena a priezviska
            ViewData["PouzivatelId"] = new SelectList(
                _context.Pouzivatelia
                    .Where(p => p.Rola == "ziak")
                    .Select(p => new { p.PouzivatelId, FullName = p.Meno + " " + p.Priezvisko }),
                "PouzivatelId",
                "FullName",
                priradeniePredmetovZiakom.PouzivatelId);

            // Zoznam predmetov
            ViewData["PredmetId"] = new SelectList(
                _context.Predmety
                    .Select(p => new { p.PredmetId, p.Nazov }),
                "PredmetId",
                "Nazov",
                priradeniePredmetovZiakom.PredmetId);

            return View(priradeniePredmetovZiakom);
        }

        // GET: PriradeniePredmetovZiakoms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var priradenie = await _context.PriradeniePredmetovZiakom.FindAsync(id);
            if (priradenie == null) return NotFound();

            ViewData["PouzivatelId"] = new SelectList(
                _context.Pouzivatelia
                    .Where(p => p.Rola == "ziak")
                    .Select(p => new { p.PouzivatelId, FullName = p.Meno + " " + p.Priezvisko }),
                "PouzivatelId",
                "FullName",
                priradenie.PouzivatelId);

            ViewData["PredmetId"] = new SelectList(
                _context.Predmety,
                "PredmetId", "Nazov", priradenie.PredmetId);

            return View(priradenie);
        }

        // POST: PriradeniePredmetovZiakoms/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PriradeniePredZiakId,PouzivatelId,PredmetId")] PriradeniePredmetovZiakom priradenie)
        {
            if (id != priradenie.PriradeniePredZiakId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(priradenie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.PriradeniePredmetovZiakom.Any(e => e.PriradeniePredZiakId == id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["PouzivatelId"] = new SelectList(
                _context.Pouzivatelia
                    .Where(p => p.Rola == "ziak")
                    .Select(p => new { p.PouzivatelId, FullName = p.Meno + " " + p.Priezvisko }),
                "PouzivatelId",
                "FullName",
                priradenie.PouzivatelId);

            ViewData["PredmetId"] = new SelectList(
                _context.Predmety,
                "PredmetId", "Nazov", priradenie.PredmetId);

            return View(priradenie);
        }

        // GET: PriradeniePredmetovZiakoms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var priradenie = await _context.PriradeniePredmetovZiakom
                .Include(p => p.Pouzivatel)
                .Include(p => p.Predmet)
                .FirstOrDefaultAsync(m => m.PriradeniePredZiakId == id);

            if (priradenie == null) return NotFound();

            return View(priradenie);
        }

        // POST: PriradeniePredmetovZiakoms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var priradenie = await _context.PriradeniePredmetovZiakom.FindAsync(id);
            if (priradenie is null)
            {
                return NotFound();
            }

            _context.PriradeniePredmetovZiakom.Remove(priradenie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}


