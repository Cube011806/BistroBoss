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
                    .ThenInclude(p => p.Kategoria)
            .Include(z => z.Opinia)
            .FirstOrDefault(z => z.Id == id);

            return View(zamowienie);
        }
        public IActionResult SetCancelled(int id)
        {
            var zamowienie = _dbContext.Zamowienia.FirstOrDefault(z => z.Id == id);
            if (zamowienie == null)
            {
                return NotFound();
            }
            zamowienie.Status = 0;
            _dbContext.SaveChanges();
            return RedirectToAction("ShowOrder", "Manage", new { id });
        }
        public IActionResult ShowUsers()
        {
            var users = _dbContext.Uzytkownicy.Where(u=>u.Email != null).ToList();
            return View(users);
        }
        public IActionResult MakeAdmin(string id)
        {
            var user = _dbContext.Uzytkownicy.Find(id);
            user.AccessLevel = 1;
            _dbContext.Uzytkownicy.Update(user);
            _dbContext.SaveChanges();
            return RedirectToAction("ShowUsers");
        }

        public IActionResult UnmakeAdmin(string id)
        {
            var user = _dbContext.Uzytkownicy.Find(id);
            user.AccessLevel = 0;
            _dbContext.Uzytkownicy.Update(user);
            _dbContext.SaveChanges();
            return RedirectToAction("ShowUsers");
        }
        public IActionResult RemoveUser(string id)
        {
            var user = _dbContext.Users.Find(id);
            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();
            return RedirectToAction("ShowUsers");
        }
        public IActionResult SetInPreparation(int id)
        {
            var zamowienie = _dbContext.Zamowienia.FirstOrDefault(z => z.Id == id);
            if (zamowienie == null)
            {
                return NotFound();
            }
            zamowienie.Status = 2;
            _dbContext.SaveChanges();
            return RedirectToAction("ShowOrder", "Manage", new { id });
        }
        public IActionResult SetInDelivery(int id)
        {
            var zamowienie = _dbContext.Zamowienia.FirstOrDefault(z => z.Id == id);
            if (zamowienie == null)
            {
                return NotFound();
            }
            zamowienie.Status = 3;
            _dbContext.SaveChanges();
            return RedirectToAction("ShowOrder", "Manage", new { id });
        }
        public IActionResult SetCompleted(int id)
        {
            var zamowienie = _dbContext.Zamowienia.FirstOrDefault(z => z.Id == id);
            if (zamowienie == null)
            {
                return NotFound();
            }
            zamowienie.Status = 4;
            _dbContext.SaveChanges();
            return RedirectToAction("ShowOrder", "Manage", new { id });
        }
        public IActionResult DeleteOrder(int id)
        {
            var zamowienie = _dbContext.Zamowienia.Find(id);
            if(zamowienie != null)
            {
                _dbContext.Zamowienia.Remove(zamowienie);
                _dbContext.SaveChanges();
            }
            return RedirectToAction("ShowAllOrders");
        }
    }
}
