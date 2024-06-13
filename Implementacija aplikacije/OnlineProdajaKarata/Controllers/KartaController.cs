using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineProdajaKarata.Data;
using OnlineProdajaKarata.Models;

namespace OnlineProdajaKarata.Controllers
{
    
    public class KartaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KartaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Karta
         
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var sql = @"
            SELECT *
            FROM Manifestacija m
            JOIN Karta k ON k.Manifestacija = m.IDManifestacije 
            WHERE k.Korisnik = {0}";
            var manifestacije = await _context.Manifestacija
            .FromSqlRaw(sql, userId)
            .Select(km => new Manifestacija
            {
                IDManifestacije = km.IDManifestacije,
                NazivManifestacije = km.NazivManifestacije,
                DatumVrijeme = km.DatumVrijeme,
                Kategorija = km.Kategorija,
                MjestoOdrzavanja = km.MjestoOdrzavanja,
                TrajanjeManifestacije = km.TrajanjeManifestacije,   
                OpisManifestacije = km.OpisManifestacije,
                CijenaKarte = km.CijenaKarte,
                Slika = km.Slika
            })
            .ToListAsync();

            var sql1 = @"
            SELECT *
            FROM Karta
            WHERE Korisnik = {0}";
            var karte = await _context.Karta
            .FromSqlRaw(sql1, userId)
            .Select(km => new Karta
            {
               IdKarte = km.IdKarte,
               IDUser = km.IDUser,
               IDManifestacije = km.IDManifestacije,
               KodKarte = km.KodKarte,  
               DatumKupovine = km.DatumKupovine,    
               Kolicina = km.Kolicina
            })
            .ToListAsync();

            List<string> listaSlika = new List<string>();
            foreach (var x in karte)
            {
                var sql2 = @"SELECT m.Slika
                                    FROM Manifestacija m
                                    JOIN Karta k ON k.Manifestacija = m.IDManifestacije
                                    WHERE k.IdKarte = {0}";
                var slika = await _context.Manifestacija
                .FromSqlRaw(sql2, x.IdKarte)
                .Select(k => k.Slika)
                .FirstOrDefaultAsync();
                listaSlika.Add(slika);
            }
            List<int> listaRedova = new List<int>();
            foreach (var x in karte)
            {
                var sql3 = @"SELECT km.BrojReda
                                    FROM KupljenaMjesta km
                                    JOIN Karta k ON k.IdKarte = km.Karta
                                    WHERE k.IdKarte = {0}";
                var red = await _context.KupljenaMjesta
                .FromSqlRaw(sql3, x.IdKarte)
                .Select(k => k.BrojReda)
                .FirstOrDefaultAsync();
                listaRedova.Add(red);
            }

            List<int> listaKolona = new List<int>();
            foreach (var x in karte)
            {
                var sql4 = @"SELECT km.BrojKolone
                                    FROM KupljenaMjesta km
                                    JOIN Karta k ON k.IdKarte = km.Karta
                                    WHERE k.IdKarte = {0}";
                var kolona = await _context.KupljenaMjesta
                .FromSqlRaw(sql4, x.IdKarte)
                .Select(k => k.BrojKolone)
                .FirstOrDefaultAsync();
                listaKolona.Add(kolona);
            }

            List<DateTime> listaDatuma = new List<DateTime>();
            foreach (var x in karte)
            {
                var sql5 = @"SELECT m.DatumVrijeme
                                    FROM Manifestacija m
                                    JOIN Karta k ON k.IdKarte = m.IDManifestacije
                                    WHERE k.IdKarte = {0}";
                var vrijeme = await _context.Manifestacija
                .FromSqlRaw(sql5, x.IdKarte)
                .Select(k => k.DatumVrijeme)
                .FirstOrDefaultAsync();
                listaDatuma.Add(vrijeme);
            }

            ViewBag.Datumi = listaDatuma;
            ViewBag.Kolone = listaKolona;
            ViewBag.Redovi = listaRedova;
            ViewBag.Slike = listaSlika;
            ViewBag.Manifestacije = manifestacije;
            ViewBag.Karte = karte;

            return View(karte);
        }


        // GET: Karta/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var karta = await _context.Karta
                .FirstOrDefaultAsync(m => m.IdKarte == id);
            if (karta == null)
            {
                return NotFound();
            }

            return View(karta);
        }

        // POST: Karta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var karta = await _context.Karta.FindAsync(id);
            if (karta != null)
            {
                _context.Karta.Remove(karta);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KartaExists(int id)
        {
            return _context.Karta.Any(e => e.IdKarte == id);
        }
    }
}
