using BistroBoss.Data;
using BistroBoss.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BistroBoss.Controllers
{
    public class ManageController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<Uzytkownik> _userManager;

        public ManageController(ApplicationDbContext dbContext, UserManager<Uzytkownik> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ShowAllOrders()
        {
            var zamowienia = _dbContext.Zamowienia.ToList();
            return View(zamowienia);
        }
        public IActionResult ShowOrder(int id)
        {
            var zamowienie = _dbContext.Zamowienia
            .Include(z => z.ZamowioneProdukty)
            .ThenInclude(zp => zp.Produkt)
            .FirstOrDefault(z => z.Id == id);

            return View(zamowienie);
        }
    }
}
