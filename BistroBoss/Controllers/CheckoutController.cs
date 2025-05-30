using BistroBoss.Data; 
using BistroBoss.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;

namespace BistroBoss.Controllers
{
    public class CheckoutController : BaseController
    {
        private readonly UserManager<Uzytkownik> _userManager;
        private readonly IEmailService _emailService;

        public CheckoutController(ApplicationDbContext dbContext, UserManager<Uzytkownik> userManager, IEmailService emailService) : base(dbContext)
        {
            _userManager = userManager;
            _emailService = emailService;
        }
        public IActionResult Index()
        {
            Zamowienie zamowienie = new Zamowienie();

            if (User.Identity.IsAuthenticated)
            {
                var userId = _userManager.GetUserId(User);
                var user = _dbContext.Uzytkownicy.FirstOrDefault(u => u.Id == userId);

                if (user != null)
                {
                    zamowienie.Uzytkownik = new Uzytkownik
                    {
                        Imie = user.Imie,
                        Nazwisko = user.Nazwisko,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber
                    };
                }
            }

            return View(zamowienie);
        }
        [HttpPost]
        public IActionResult SubmitOrder(Zamowienie zamowienie, Uzytkownik uzytkownik, bool deliveryMethod)
        {
            if(zamowienie.SposobDostawy)
            {
                if (zamowienie.Imie.IsNullOrEmpty() || zamowienie.Nazwisko.IsNullOrEmpty() || zamowienie.Email.IsNullOrEmpty() || zamowienie.NumerTelefonu.IsNullOrEmpty() ||
                zamowienie.Miejscowosc.IsNullOrEmpty() || zamowienie.Ulica.IsNullOrEmpty() || zamowienie.NumerBudynku.IsNullOrEmpty() || zamowienie.KodPocztowy.IsNullOrEmpty())
                {
                    TempData["ErrorMessage"] = "Wszystkie pola muszą zostać wypełnione!";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                if (zamowienie.Imie.IsNullOrEmpty() || zamowienie.Nazwisko.IsNullOrEmpty() || zamowienie.Email.IsNullOrEmpty() || zamowienie.NumerTelefonu.IsNullOrEmpty())
                {
                    TempData["ErrorMessage"] = "Wszystkie pola muszą zostać wypełnione!";
                    return RedirectToAction("Index");
                }
            }
            var koszyk = _dbContext.Koszyki.Include(k => k.KoszykProdukty).FirstOrDefault(k => k.UzytkownikId == _userManager.GetUserId(User));
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
            //deliveryMethod - true = dostawa, false = odbiór osobisty
            if (!deliveryMethod)
            {
                zamowienie.Miejscowosc = "";
                zamowienie.Ulica = "";
                zamowienie.NumerBudynku = "";
                zamowienie.KodPocztowy = "";
                zamowienie.SposobDostawy = false;
            }
            else
            {
                zamowienie.SposobDostawy = true;
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
            string message = $@"
                <html>
                    <body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
                        <h2 style='color: #4CAF50;'>Dziękujemy za Twoje zamówienie!</h2>
                        <p><strong>Numer zamówienia:</strong> {zamowienie.Id}</p>
                        <p><strong>Data zamówienia:</strong> {zamowienie.DataZamowienia:dd.MM.yyyy HH:mm}</p>
                        <p><strong>Przewidywany czas realizacji:</strong> {zamowienie.PrzewidywanyCzasRealizacji} minut</p>
                        <p><strong>Cena całkowita:</strong> {zamowienie.CenaCalkowita} zł</p>
                        <p><strong>Adres dostawy:</strong><br />
                            {zamowienie.Miejscowosc}, {zamowienie.Ulica} {zamowienie.NumerBudynku}<br />
                            {zamowienie.KodPocztowy}
                        </p>
                        <hr style='margin: 20px 0;' />
                        <p>W razie pytań prosimy o kontakt z naszym działem obsługi klienta.</p>
                        <p style='color: #777;'>Pozdrawiamy,<br />Zespół BistroBoss</p>
                    </body>
                </html>";
            _emailService.SendEmail(user.Email, "Nowe zamówienie", message);
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
