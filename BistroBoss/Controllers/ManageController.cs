using BistroBoss.Data;
using BistroBoss.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BistroBoss.Controllers
{
    public class ManageController : BaseController
    {
        private readonly UserManager<Uzytkownik> _userManager;
        private readonly IEmailService _emailService;

        public ManageController(ApplicationDbContext dbContext, UserManager<Uzytkownik> userManager, IEmailService emailService) : base(dbContext)
        {
            _userManager = userManager;
            _emailService = emailService;
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
            TempData["SuccessMessage"] = "Pomyślnie anulowano zamówienie!";
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
            TempData["SuccessMessage"] = "Pomyślnie zmieniono rangę użytkownika!";
            return RedirectToAction("ShowUsers");
        }

        public IActionResult UnmakeAdmin(string id)
        {
            var user = _dbContext.Uzytkownicy.Find(id);
            user.AccessLevel = 0;
            _dbContext.Uzytkownicy.Update(user);
            _dbContext.SaveChanges();
            TempData["SuccessMessage"] = "Pomyślnie zmieniono rangę użytkownika!";
            return RedirectToAction("ShowUsers");
        }
        public IActionResult RemoveUser(string id)
        {
            var user = _dbContext.Users.Find(id);
            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();
            TempData["SuccessMessage"] = "Pomyślnie usunięto użytkownika!";
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
            TempData["SuccessMessage"] = "Pomyślnie zmieniono status zamówienia na 'W przygotowaniu'!";
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
            var user = _dbContext.Uzytkownicy.FirstOrDefault(u => u.Id == zamowienie.UzytkownikId);
            string message = $@"
            <html>
                <body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
                    <h2 style='color: #2196F3;'>Twoje zamówienie jest w drodze!</h2>
                    <p><strong>Numer zamówienia:</strong> {zamowienie.Id}</p>
                    <p><strong>Data zamówienia:</strong> {zamowienie.DataZamowienia:dd.MM.yyyy HH:mm}</p>
                    <p><strong>Cena całkowita:</strong> {zamowienie.CenaCalkowita} zł</p>
                    <p>
                        <strong>Adres dostawy:</strong><br />
                        {zamowienie.Miejscowosc}, {zamowienie.Ulica} {zamowienie.NumerBudynku}<br />
                        {zamowienie.KodPocztowy}
                    </p>
                    <hr style='margin: 20px 0;' />
                    <p>Oczekuj dostawcy. Twój posiłek niedługo dotrze na miejsce.</p>
                    <p style='color: #777;'>Dziękujemy za zaufanie!<br />Zespół BistroBoss</p>
                </body>
            </html>";
            _emailService.SendEmail(user.Email, "Zamówienie w drodze", message);
            _dbContext.SaveChanges();
            if(zamowienie.SposobDostawy)
            {
                TempData["SuccessMessage"] = "Pomyślnie zmieniono status zamówienia na 'W dostawie'!";
            }
            else
            {
                TempData["SuccessMessage"] = "Pomyślnie zmieniono status zamówienia na 'Gotowe do odbioru'!";
            }
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
            TempData["SuccessMessage"] = "Pomyślnie zmieniono status zamówienia na 'Zrealizowane'!";
            return RedirectToAction("ShowOrder", "Manage", new { id });
        }
        public IActionResult DeleteOrder(int id)
        {
            var zamowienie = _dbContext.Zamowienia.Find(id);
            if(zamowienie != null)
            {
                _dbContext.Zamowienia.Remove(zamowienie);
                _dbContext.SaveChanges();
                TempData["SuccessMessage"] = "Pomyślnie usunięto zamówienie!";
            }
            return RedirectToAction("ShowAllOrders");
        }
    }
}
