using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineProdajaKarata.Data;
using OnlineProdajaKarata.Models;

namespace OnlineProdajaKarata.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManifestacijaAdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ManifestacijaAdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ManifestacijaAdmin
        public async Task<IActionResult> Index()
        {
            return View(await _context.Manifestacija.ToListAsync());
        }

        // GET: ManifestacijaAdmin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sql = @"
        SELECT u.*, k.*, m.*
        FROM AspNetUsers u
        JOIN Karta k ON k.Korisnik = u.Id 
        JOIN Manifestacija m ON k.Manifestacija = m.IDManifestacije
        WHERE m.IDManifestacije = {0}";

            var result = await _context.Users
                .FromSqlRaw(sql, id)
                .ToListAsync();

            
            /*var sql = @"
            SELECT *
            FROM AspNetUsers as
            JOIN Karta k ON k.Korsnik = as.Id 
            JOIN Manifestacija m ON k.Manifestacija = m.IDManifestacije
            WHERE m.IDManifestacije = {0}";

            
            var korisnici = await _context.Users
            .FromSqlRaw(sql, id)
            .Select(km => new Korisnik
            {
                Id = km.Id,
                Ime = km.Ime,
                Prezime = km.Prezime,   
                JMBG = km.JMBG,
                KorisnickoIme = km.KorisnickoIme,
                DatumRodjenja = km.DatumRodjenja,
                Email = km.Email,
                Password = km.Password,
                BrojKupljenihKarata = km.BrojKupljenihKarata
            })
            .ToListAsync();*/

            ViewBag.Korisnici = result;

            var manifestacija = await _context.Manifestacija
                .FirstOrDefaultAsync(m => m.IDManifestacije == id);
            if (manifestacija == null)
            {
                return NotFound();
            }
            var manifestacijaList = new List<Manifestacija> { manifestacija };
            return View(manifestacijaList);
        }

        
        private bool ManifestacijaExists(int id)
        {
            return _context.Manifestacija.Any(e => e.IDManifestacije == id);
        }
    }
}
