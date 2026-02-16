using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EvidenciaStudentov.Infrastructure.Persistence;
using EvidenciaStudentov.Models;
using System.Diagnostics;

namespace ASP_NET_Bakalarka.Controllers
{
    public class ZnamkasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ZnamkasController(ApplicationDbContext context)
        {
            _context = context;
        }


 // POST Znamkas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ZnamkaId,PredmetId,PouzivatelId,Hodnota,Datum")] Znamka znamka)
        {
            // Debugging vstupných údajov
            Console.WriteLine($"Známka na vstupe: PredmetId={znamka.PredmetId}, PouzivatelId={znamka.PouzivatelId}, Hodnota={znamka.Hodnota}, Datum={znamka.Datum}");

            // Odstránenie validácie navigačných vlastností
            ModelState.Remove(nameof(Znamka.Predmet));
            ModelState.Remove(nameof(Znamka.Pouzivatel));

            if (ModelState.IsValid)
            {
                try
                {
                    // Pridanie záznamu do databázy
                    _context.Add(znamka);
                    await _context.SaveChangesAsync();

                    // Debugging potvrdenia o úspechu
                    var pocetZnamok = _context.Znamky.Count();
                    Console.WriteLine($"Známka úspešne uložená. Počet záznamov v Znamky: {pocetZnamok}");

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Chyba pri ukladaní: {ex.Message}");
                }
            }
            else
            {
                // Debugging chýb ModelState
                Console.WriteLine("ModelState nie je validný. Chyby:");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"ModelState Error: {error.ErrorMessage}");
                }
            }

            // Obnovenie ViewData pre dropdowny
            ViewData["PouzivatelId"] = new SelectList(_context.Pouzivatelia, "PouzivatelId", "Email", znamka.PouzivatelId);
            ViewData["PredmetId"] = new SelectList(_context.Predmety, "PredmetId", "Nazov", znamka.PredmetId);

            return View(znamka);
        }


        // GET: Znamkas
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Znamky.Include(z => z.Pouzivatel).Include(z => z.Predmet);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Znamkas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var znamka = await _context.Znamky
                .Include(z => z.Pouzivatel)
                .Include(z => z.Predmet)
                .FirstOrDefaultAsync(m => m.ZnamkaId == id);
            if (znamka == null)
            {
                return NotFound();
            }

            return View(znamka);
        }

        // GET: Znamkas/Create
        public IActionResult Create()
        {
            // Načítanie predmetov a používateľov z databázy
            var predmety = _context.Predmety.ToList();
            var pouzivatelia = _context.Pouzivatelia.ToList();

            // Debugging: Výpis počtu záznamov pre kontrolu
            Console.WriteLine($"Počet predmetov: {predmety.Count}");
            Console.WriteLine($"Počet používateľov: {pouzivatelia.Count}");

            // Naplnenie ViewData pre dropdowny vo formulári
            ViewData["PouzivatelId"] = new SelectList(pouzivatelia, "PouzivatelId", "Email");
            ViewData["PredmetId"] = new SelectList(predmety, "PredmetId", "Nazov");

            return View();
        }



        // GET: Znamkas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var znamka = await _context.Znamky.FindAsync(id);
            if (znamka == null)
            {
                return NotFound();
            }
            ViewData["PouzivatelId"] = new SelectList(_context.Pouzivatelia, "PouzivatelId", "Email", znamka.PouzivatelId);
            ViewData["PredmetId"] = new SelectList(_context.Predmety, "PredmetId", "Nazov", znamka.PredmetId);
            return View(znamka);
        }

        // POST: Znamkas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ZnamkaId,PredmetId,PouzivatelId,Hodnota,Datum")] Znamka znamka)
        {
            if (id != znamka.ZnamkaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(znamka);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZnamkaExists(znamka.ZnamkaId))
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
            ViewData["PouzivatelId"] = new SelectList(_context.Pouzivatelia, "PouzivatelId", "Email", znamka.PouzivatelId);
            ViewData["PredmetId"] = new SelectList(_context.Predmety, "PredmetId", "Nazov", znamka.PredmetId);
            return View(znamka);
        }

        // GET: Znamkas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var znamka = await _context.Znamky
                .Include(z => z.Pouzivatel)
                .Include(z => z.Predmet)
                .FirstOrDefaultAsync(m => m.ZnamkaId == id);
            if (znamka == null)
            {
                return NotFound();
            }

            return View(znamka);
        }

        // POST: Znamkas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var znamka = await _context.Znamky.FindAsync(id);
            if (znamka != null)
            {
                _context.Znamky.Remove(znamka);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ZnamkaExists(int id)
        {
            return _context.Znamky.Any(e => e.ZnamkaId == id);
        }
    }
}


