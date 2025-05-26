using BistroBoss.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BistroBoss.Data
{
    public class ApplicationDbContext : IdentityDbContext<Uzytkownik>
    {
        public virtual DbSet<Uzytkownik> Uzytkownicy { get; set; }
        public virtual DbSet<Zamowienie> Zamowienia { get; set; }
        public virtual DbSet<ZamowienieProdukt> ZamowieniaProdukty { get; set; }
        //public virtual DbSet<Status> Statusy { get; set; }
        public virtual DbSet<Kategoria> Kategorie { get; set; }
        public virtual DbSet<Dostawa> Dostawy { get; set; }
        public virtual DbSet<Opinia> Opinie { get; set; }
        public virtual DbSet<Koszyk> Koszyki { get; set; }
        public virtual DbSet<KoszykProdukt> KoszykProdukty { get; set; }
        public virtual DbSet<Produkt> Produkty { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Opinia>()
                .HasOne(o => o.Zamowienie)
                .WithOne()
                .HasForeignKey<Opinia>(o => o.ZamowienieId);
            
            modelBuilder.Entity<Zamowienie>()
                .HasOne(z => z.Opinia)
                .WithOne()
                .HasForeignKey<Zamowienie>(z => z.OpiniaId);

            modelBuilder.Entity<Koszyk>()
                .HasOne(k => k.Uzytkownik)
                .WithOne(u => u.Koszyk)
                .HasForeignKey<Koszyk>(k => k.UzytkownikId);


            //modelBuilder.Entity<Zamowienie>()
            //    .HasOne(z => z.Koszyk)
            //    .WithOne()
            //    .HasForeignKey<Zamowienie>(z => z.KoszykId);
            
            //modelBuilder.Entity<Status>()
            //    .HasIndex(s => s.Nazwa)
            //    .IsUnique();

            modelBuilder.Entity<Kategoria>()
                .HasIndex(k => k.Nazwa)
                .IsUnique();

            //modelBuilder.Entity<Zamowienie>()
            //    .HasOne(z => z.Koszyk)
            //    .WithOne()
            //    .HasForeignKey<Zamowienie>(z => z.KoszykId)
            //    .OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<Koszyk>()
            //    .HasOne(k => k.Zamowienie)
            //    .WithOne(z => z.Koszyk)
            //    .HasForeignKey<Koszyk>(k => k.ZamowienieId);
        }
    }
}
