using BistroBoss.Data;
using BistroBoss.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BistroBoss.Controllers
{
    public class MenuController : BaseController
    {
        private readonly UserManager<Uzytkownik> _userManager;

        public MenuController(ApplicationDbContext dbContext, UserManager<Uzytkownik> userManager) : base(dbContext)
        {
            _userManager = userManager;
        }
        public IActionResult Index(int? kategoriaId)
        {
            var users = _dbContext.Uzytkownicy;

            if (users.Where(u => u.AccessLevel == 1 && u.Email != null).Count() < 1)
            {
                var admin = users.FirstOrDefault();
                admin.AccessLevel = 1;
                _dbContext.Uzytkownicy.Update(admin);
                _dbContext.SaveChanges();
                Console.WriteLine("Podniesiono poziom uprawnień użytkownika: " + admin.Email);
            }
            if (kategoriaId != null)
            {
                var kategorie = _dbContext.Kategorie.Where(k => k.Id == kategoriaId).Include(k => k.Produkty).ToList();
                return View(kategorie);
            }
            else
            {
                var kategorie = _dbContext.Kategorie.Include(k => k.Produkty).ToList();
                return View(kategorie);
            }
        }
        public IActionResult Details(int id)
        {
            var produkty = _dbContext.Produkty.Include(p => p.Kategoria);
            var produkt = produkty.FirstOrDefault(p => p.Id == id);
            return View(produkt); 
        }
    }
}
