using BistroBoss.Data;
using BistroBoss.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BistroBoss.Controllers
{
    public class MenuController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<Uzytkownik> _userManager;

        public MenuController(ApplicationDbContext dbContext, UserManager<Uzytkownik> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var kategorie = _dbContext.Kategorie.Include(k=>k.Produkty).ToList();
            return View(kategorie);
        }
        public IActionResult Details(int id)
        {
            var produkty = _dbContext.Produkty.Include(p => p.Kategoria);
            var produkt = produkty.FirstOrDefault(p => p.Id == id);
            return View(produkt); 
        }
    }
}
