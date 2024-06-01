using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineProdajaKarata.Models;
using System.Net.Sockets;

namespace OnlineProdajaKarata.Data
{
    public class ApplicationDbContext : IdentityDbContext<Korisnik>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Manifestacija> Manifestacija { get; set; }
        public DbSet<Karta> Karta { get; set; }
        public DbSet<KupljenaMjesta> KupljenaMjesta { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Manifestacija>().ToTable("Manifestacija");
            modelBuilder.Entity<Karta>().ToTable("Karta");
            modelBuilder.Entity<KupljenaMjesta>().ToTable("KupljenaMjesta");
            modelBuilder.Entity<Korisnik>(b =>
            {
                b.Property(u => u.Ime);
                b.Property(u => u.Prezime);
                b.Property(u => u.JMBG);
                b.Property(u => u.KorisnickoIme);
                b.Property(u => u.DatumRodjenja);
                b.Property(u => u.BrojKupljenihKarata);
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
