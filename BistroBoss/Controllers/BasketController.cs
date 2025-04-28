
using BistroBoss.Data;
using BistroBoss.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BistroBoss.Controllers
{
    public class BasketController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<Uzytkownik> _userManager;

        public BasketController(ApplicationDbContext dbContext, UserManager<Uzytkownik> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            var uzytkownik = _dbContext.Uzytkownicy.Find(userId);
            //if (uzytkownik.Koszyk == null) {
            //uzytkownik.Koszyk = new Koszyk();
            //}
            //var koszyk = _dbContext.Koszyki.FirstOrDefault(k => k.UzytkownikId == userId);
            //return View(koszyk);
            return View();
        }
        public IActionResult AddToBasket()
        {
            return View();
        }
    }
}
