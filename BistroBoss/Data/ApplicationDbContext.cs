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
    }
}
