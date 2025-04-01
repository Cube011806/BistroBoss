using BistroBoss.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BistroBoss.Data
{
    public class ApplicationDbContext : IdentityDbContext<Uzytkownik>
    {
        public virtual DbSet<Uzytkownik> Uzytkownicy { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Status>()
                .HasIndex(s => s.Nazwa)
                .IsUnique();

            modelBuilder.Entity<Kategoria>()
                .HasIndex(k => k.Nazwa)
                .IsUnique();
        }
    }
}
