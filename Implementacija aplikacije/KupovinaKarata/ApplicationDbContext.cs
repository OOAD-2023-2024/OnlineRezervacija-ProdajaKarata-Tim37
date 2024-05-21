using KupovinaKarata.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Reflection.Emit;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    public DbSet<User> User { get; set; }
    public DbSet<KupljenaMjesta> KupljenaMjesta { get; set; }
    public DbSet<Manifestacija> Manifestacija { get; set; }
    public DbSet<Karta> Karta { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("User");
        modelBuilder.Entity<KupljenaMjesta>().ToTable("KupljenaMjesta");
        modelBuilder.Entity<Manifestacija>().ToTable("Manifestacija");
        modelBuilder.Entity<Karta>().ToTable("Karta");
        base.OnModelCreating(modelBuilder);
    }
}