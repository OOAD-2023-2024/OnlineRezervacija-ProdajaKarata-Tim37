using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineProdajaKarata.Data;
using OnlineProdajaKarata.Models;

namespace OnlineProdajaKarata.Controllers
{
    
    public class ManifestacijaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ManifestacijaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Manifestacija
        public async Task<IActionResult> Index()
        {
            return View(await _context.Manifestacija.ToListAsync());
        }

        // GET: Manifestacija/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manifestacija = await _context.Manifestacija
                .FirstOrDefaultAsync(m => m.IDManifestacije == id);
            if (manifestacija == null)
            {
                return NotFound();
            }
            var sql = @"
            SELECT km.BrojReda, km.BrojKolone
            FROM KupljenaMjesta km
            JOIN Karta k ON km.Karta = k.IdKarte
            JOIN Manifestacija m ON k.Manifestacija = m.IDManifestacije
            WHERE m.IDManifestacije = {0}";

            var kupljenaMjesta = await _context.KupljenaMjesta
            .FromSqlRaw(sql, id)
            .Select(km => new KupljenoMjestoViewModel
            {
                BrojReda = km.BrojReda,
                BrojKolone = km.BrojKolone
            })
            .ToListAsync();

            ViewBag.KupljenaMjesta = kupljenaMjesta;

            return View(manifestacija);
        }

        // GET: Manifestacija/Create
        [Authorize(Roles = "Admin, Employee")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Manifestacija/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> Create([Bind("IDManifestacije,NazivManifestacije,DatumVrijeme,Kategorija,MjestoOdrzavanja,TrajanjeManifestacije,OpisManifestacije,CijenaKarte,Slika")] Manifestacija manifestacija)
        {
            if (ModelState.IsValid)
            {
                _context.Add(manifestacija);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(manifestacija);
        }

        // GET: Manifestacija/Edit/5
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manifestacija = await _context.Manifestacija.FindAsync(id);
            if (manifestacija == null)
            {
                return NotFound();
            }
            return View(manifestacija);
        }

        // POST: Manifestacija/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> Edit(int id, [Bind("IDManifestacije,NazivManifestacije,DatumVrijeme,Kategorija,MjestoOdrzavanja,TrajanjeManifestacije,OpisManifestacije,CijenaKarte,Slika")] Manifestacija manifestacija)
        {
            if (id != manifestacija.IDManifestacije)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(manifestacija);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ManifestacijaExists(manifestacija.IDManifestacije))
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
            return View(manifestacija);
        }

        // GET: Manifestacija/Delete/5
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manifestacija = await _context.Manifestacija
                .FirstOrDefaultAsync(m => m.IDManifestacije == id);
            if (manifestacija == null)
            {
                return NotFound();
            }

            return View(manifestacija);
        }

        // POST: Manifestacija/Delete/5
        [Authorize(Roles = "Admin, Employee")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var manifestacija = await _context.Manifestacija.FindAsync(id);
            if (manifestacija != null)
            {
                _context.Manifestacija.Remove(manifestacija);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ManifestacijaExists(int id)
        {
            return _context.Manifestacija.Any(e => e.IDManifestacije == id);
        }
    }
}
