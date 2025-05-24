using BistroBoss.Data; 
using BistroBoss.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace BistroBoss.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Uzytkownik> _userManager;

        public CheckoutController(ApplicationDbContext context, UserManager<Uzytkownik> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SubmitOrder(Zamowienie zamowienie, Uzytkownik uzytkownik, bool deliveryMethod)
        {
            var koszyk = _context.Koszyki.Include(k=>k.KoszykProdukty).FirstOrDefault(k => k.UzytkownikId == _userManager.GetUserId(User));
            foreach(var item in koszyk.KoszykProdukty)
            {
                var produkt = _context.Produkty.FirstOrDefault(p => p.Id == item.ProduktId);
                if (produkt != null)
                {
                    var zamowienieprodukt = new ZamowienieProdukt
                    {
                        ProduktId = produkt.Id,
                        Ilosc = item.Ilosc,
                        Cena = produkt.Cena
                    };

                    _context.ZamowieniaProdukty.Add(zamowienieprodukt);
                    zamowienie.ZamowioneProdukty.Add(zamowienieprodukt);
                }
            }
            zamowienie.DataZamowienia = DateTime.Now;
            if (!deliveryMethod)
            {
                zamowienie.Miejscowosc = "";
                zamowienie.Ulica = "";
                zamowienie.NumerBudynku = "";
                zamowienie.KodPocztowy = "";
            }

            // Pobieramy identyfikator koszyka użytkownika
            //zamowienie.KoszykId = GetUserKoszykId();

            // Obsługa użytkownika - jeśli zalogowany, przypisujemy jego ID, jeśli nie, tworzymy nowego użytkownika
            if (User.Identity.IsAuthenticated)
            {
                zamowienie.UzytkownikId = _userManager.GetUserId(User);
            }
            else
            {
                var guest = new Uzytkownik
                {
                    Imie = uzytkownik.Imie,
                    Nazwisko = uzytkownik.Nazwisko,
                    Email = uzytkownik.Email,
                    PhoneNumber = uzytkownik.PhoneNumber,
                    UserName = uzytkownik.Email
                };

                _context.Users.Add(guest);
                _context.SaveChanges();
                zamowienie.UzytkownikId = guest.Id;
            }
            //Dopisz czyszczenie koszyka
            _context.Zamowienia.Add(zamowienie);
            _context.SaveChanges();

            return RedirectToAction("OrderConfirmation", new { id = zamowienie.Id });
        }



        private int GetUserKoszykId()
        {
            var userId = _userManager.GetUserId(User);

            var koszyk = _context.Koszyki
                .FirstOrDefault(k => k.UzytkownikId == userId && k.ZamowienieId == null);

            if (koszyk == null)
            {
                throw new Exception("Nie znaleziono koszyka użytkownika.");
            }

            return koszyk.Id;
        }


        public IActionResult OrderConfirmation(int id)
        {
            ViewBag.OrderId = id;
            return View();
        }
    }
}
