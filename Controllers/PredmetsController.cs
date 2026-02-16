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
    public class PredmetsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PredmetsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Predmets
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Predmety.Include(p => p.Ucitel);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Predmets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var predmet = await _context.Predmety
                .Include(p => p.Ucitel)
                .FirstOrDefaultAsync(m => m.PredmetId == id);
            if (predmet == null)
            {
                return NotFound();
            }

            return View(predmet);
        }

        // GET: Predmets/Create
        public IActionResult Create()
        {
            ViewData["UcitelId"] = new SelectList(
                _context.Pouzivatelia.Where(p => p.Rola == "ucitel"),
                "PouzivatelId", "Email");
            return View();
        }

        // POST: Predmets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PredmetId,Nazov,Popis,UcitelId")] Predmet predmet)
        {
            if (ModelState.IsValid)
            {
                _context.Add(predmet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UcitelId"] = new SelectList(_context.Pouzivatelia, "PouzivatelId", "Email", predmet.UcitelId);
            return View(predmet);
        }

        // GET: Predmets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var predmet = await _context.Predmety.FindAsync(id);
            if (predmet == null)
            {
                return NotFound();
            }
            ViewData["UcitelId"] = new SelectList(_context.Pouzivatelia, "PouzivatelId", "Email", predmet.UcitelId);
            return View(predmet);
        }

        // POST: Predmets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PredmetId,Nazov,Popis,UcitelId")] Predmet predmet)
        {
            if (id != predmet.PredmetId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(predmet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PredmetExists(predmet.PredmetId))
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
            ViewData["UcitelId"] = new SelectList(_context.Pouzivatelia, "PouzivatelId", "Email", predmet.UcitelId);
            return View(predmet);
        }

        // GET: Predmets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var predmet = await _context.Predmety
                .Include(p => p.Ucitel)
                .FirstOrDefaultAsync(m => m.PredmetId == id);
            if (predmet == null)
            {
                return NotFound();
            }

            return View(predmet);
        }

        // POST: Predmets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var predmet = await _context.Predmety.FindAsync(id);
            if (predmet != null)
            {
                _context.Predmety.Remove(predmet);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PredmetExists(int id)
        {
            return _context.Predmety.Any(e => e.PredmetId == id);
        }
    }
}


