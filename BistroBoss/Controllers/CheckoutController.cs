using BistroBoss.Data; 
using BistroBoss.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace BistroBoss.Controllers
{
    public class CheckoutController : BaseController
    {
        private readonly UserManager<Uzytkownik> _userManager;

        public CheckoutController(ApplicationDbContext dbContext, UserManager<Uzytkownik> userManager) : base(dbContext)
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SubmitOrder(Zamowienie zamowienie, Uzytkownik uzytkownik, bool deliveryMethod)
        {
            var koszyk = _dbContext.Koszyki.Include(k=>k.KoszykProdukty).FirstOrDefault(k => k.UzytkownikId == _userManager.GetUserId(User));
            float cenaCalkowita = 0;
            foreach(var item in koszyk.KoszykProdukty)
            {
                var produkt = _dbContext.Produkty.FirstOrDefault(p => p.Id == item.ProduktId);
                if (produkt != null)
                {
                    
                    var zamowienieprodukt = new ZamowienieProdukt
                    {
                        ProduktId = produkt.Id,
                        Ilosc = item.Ilosc,
                        Cena = produkt.Cena
                        
                    };
                    _dbContext.ZamowieniaProdukty.Add(zamowienieprodukt);
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
            var userId = "";
            if (User.Identity.IsAuthenticated)
            {
                zamowienie.UzytkownikId = _userManager.GetUserId(User);
                userId = _userManager.GetUserId(User);
            }
            else
            {
                //var guest = new Uzytkownik
                //{
                //    Imie = uzytkownik.Imie,
                //    Nazwisko = uzytkownik.Nazwisko,
                //    Email = uzytkownik.Email,
                //    PhoneNumber = uzytkownik.PhoneNumber,
                //    UserName = uzytkownik.Email
                //};

                //_dbContext.Users.Add(guest);
                //_dbContext.SaveChanges();
                //zamowienie.UzytkownikId = guest.Id;
            }
            zamowienie.Status = 1;
            foreach(var produkt in koszyk.KoszykProdukty)
            {
                _dbContext.KoszykProdukty.Remove(produkt);
            }
            var czasMax = 0;
            foreach (var produkt in zamowienie.ZamowioneProdukty)
            {
                cenaCalkowita += produkt.Cena * produkt.Ilosc;
                if(czasMax < produkt.Produkt.CzasPrzygotowania)
                {
                    czasMax = produkt.Produkt.CzasPrzygotowania;
                }
            }
            zamowienie.PrzewidywanyCzasRealizacji = czasMax;
            zamowienie.CenaCalkowita = cenaCalkowita;
            var user = _dbContext.Uzytkownicy.Find(userId);
            user.Zamowienia.Add(zamowienie);
            _dbContext.Uzytkownicy.Update(user);
            _dbContext.SaveChanges();

            TempData["SuccessMessage"] = "Zamówienie zostało złożone, dziękujemy! Numer zamówienia: " + zamowienie.Id;
            return RedirectToAction("ShowMyOrders", "Basket");

            //return RedirectToAction("OrderConfirmation", new { id = zamowienie.Id });
        }

        private int GetUserKoszykId()
        {
            var userId = _userManager.GetUserId(User);

            var koszyk = _dbContext.Koszyki
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
