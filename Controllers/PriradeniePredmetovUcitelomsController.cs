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
    public class PriradeniePredmetovUcitelomsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PriradeniePredmetovUcitelomsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PriradeniePredmetovUciteloms
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PriradeniaPredmetovUcitelom
                .Include(p => p.Pouzivatel)
                .Include(p => p.Predmet);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PriradeniePredmetovUciteloms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var priradeniePredmetovUcitelom = await _context.PriradeniaPredmetovUcitelom
                .Include(p => p.Pouzivatel)
                .Include(p => p.Predmet)
                .FirstOrDefaultAsync(m => m.PriradeniePredUcitelId == id);
            if (priradeniePredmetovUcitelom == null)
            {
                return NotFound();
            }

            return View(priradeniePredmetovUcitelom);
        }

        // GET: PriradeniePredmetovUciteloms/Create
        public IActionResult Create()
        {
            // Filtrovanie používateľov s rolou "učiteľ" a kombinácia mena a priezviska
            ViewData["PouzivatelId"] = new SelectList(
                _context.Pouzivatelia
                    .Where(p => p.Rola == "ucitel")
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

        // POST: PriradeniePredmetovUciteloms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PriradeniePredUcitelId,PouzivatelId,PredmetId")] PriradeniePredmetovUcitelom priradeniePredmetovUcitelom)
        {
            if (ModelState.IsValid)
            {
                _context.Add(priradeniePredmetovUcitelom);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // V prípade chyby opätovne naplniť select list s kombináciou mena a priezviska
            ViewData["PouzivatelId"] = new SelectList(
                _context.Pouzivatelia
                    .Where(p => p.Rola == "ucitel")
                    .Select(p => new { p.PouzivatelId, FullName = p.Meno + " " + p.Priezvisko }),
                "PouzivatelId",
                "FullName",
                priradeniePredmetovUcitelom.PouzivatelId);

            // Zoznam predmetov
            ViewData["PredmetId"] = new SelectList(
                _context.Predmety
                    .Select(p => new { p.PredmetId, p.Nazov }),
                "PredmetId",
                "Nazov",
                priradeniePredmetovUcitelom.PredmetId);

            return View(priradeniePredmetovUcitelom);
        }

        // GET: PriradeniePredmetovUciteloms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var priradenie = await _context.PriradeniaPredmetovUcitelom.FindAsync(id);
            if (priradenie == null) return NotFound();

            ViewData["PouzivatelId"] = new SelectList(
                _context.Pouzivatelia
                    .Where(p => p.Rola == "ucitel")
                    .Select(p => new { p.PouzivatelId, FullName = p.Meno + " " + p.Priezvisko }),
                "PouzivatelId",
                "FullName",
                priradenie.PouzivatelId);

            ViewData["PredmetId"] = new SelectList(
                _context.Predmety, "PredmetId", "Nazov", priradenie.PredmetId);

            return View(priradenie);
        }

        // POST: PriradeniePredmetovUciteloms/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PriradeniePredUcitelId,PouzivatelId,PredmetId")] PriradeniePredmetovUcitelom priradenie)
        {
            if (id != priradenie.PriradeniePredUcitelId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(priradenie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.PriradeniaPredmetovUcitelom.Any(e => e.PriradeniePredUcitelId == id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["PouzivatelId"] = new SelectList(
                _context.Pouzivatelia
                    .Where(p => p.Rola == "ucitel")
                    .Select(p => new { p.PouzivatelId, FullName = p.Meno + " " + p.Priezvisko }),
                "PouzivatelId",
                "FullName",
                priradenie.PouzivatelId);

            ViewData["PredmetId"] = new SelectList(
                _context.Predmety, "PredmetId", "Nazov", priradenie.PredmetId);

            return View(priradenie);
        }

        // GET: PriradeniePredmetovUciteloms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var priradenie = await _context.PriradeniaPredmetovUcitelom
                .Include(p => p.Pouzivatel)
                .Include(p => p.Predmet)
                .FirstOrDefaultAsync(m => m.PriradeniePredUcitelId == id);

            if (priradenie == null) return NotFound();

            return View(priradenie);
        }

        // POST: PriradeniePredmetovUciteloms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var priradenie = await _context.PriradeniaPredmetovUcitelom.FindAsync(id);
            if (priradenie != null)
            {
                _context.PriradeniaPredmetovUcitelom.Remove(priradenie);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}


