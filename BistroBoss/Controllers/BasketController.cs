
using BistroBoss.Data;
using BistroBoss.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using System.Text.Json;

namespace BistroBoss.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using NuGet.Protocol;
    using System.Security.Claims;
    using System.Text.Json;

    public class BasketController : BaseController
    {
        private readonly UserManager<Uzytkownik> _userManager;
        private const string SessionKeyKoszyk = "_Koszyk";

        public BasketController(ApplicationDbContext dbContext, UserManager<Uzytkownik> userManager) : base(dbContext)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = _userManager.GetUserId(User);
                var koszyk = _dbContext.Koszyki
                    .Include(k => k.KoszykProdukty)
                    .ThenInclude(kp => kp.Produkt)
                    .FirstOrDefault(k => k.UzytkownikId == userId);

                if (koszyk == null)
                {
                    koszyk = new Koszyk { UzytkownikId = userId };
                    _dbContext.Koszyki.Add(koszyk);
                    _dbContext.SaveChanges();
                }

                return View("IndexLoggedIn", koszyk);
            }
            else
            {
                var koszyk = GetSessionKoszyk();
                return View("IndexSession", koszyk);
            }
        }

        public IActionResult AddToBasket(int produktId)
        {
            var produkt = _dbContext.Produkty.Find(produktId);
            if (produkt == null)
                return NotFound();

            if (User.Identity.IsAuthenticated)
            {
                var userId = _userManager.GetUserId(User);
                var koszyk = _dbContext.Koszyki
                    .Include(k => k.KoszykProdukty)
                    .FirstOrDefault(k => k.UzytkownikId == userId);

                if (koszyk == null)
                {
                    koszyk = new Koszyk { UzytkownikId = userId };
                    _dbContext.Koszyki.Add(koszyk);
                }

                var koszykProdukt = koszyk.KoszykProdukty.FirstOrDefault(kp => kp.ProduktId == produktId);
                if (koszykProdukt != null)
                {
                    koszykProdukt.Ilosc++;
                }
                else
                {
                    var kp = new KoszykProdukt { ProduktId = produktId, Ilosc = 1 };
                    _dbContext.KoszykProdukty.Add(kp);
                    koszyk.KoszykProdukty.Add(kp);
                }

                _dbContext.SaveChanges();
                TempData["SuccessMessage"] = "Produkt został pomyślnie dodany do koszyka!";
                return RedirectToAction("Index","Menu");
            }
            else
            {
                var koszyk = GetSessionKoszyk();

                var sessionProdukt = koszyk.Produkty.FirstOrDefault(p => p.ProduktId == produktId);
                if (sessionProdukt != null)
                {
                    sessionProdukt.Ilosc++;
                }
                else
                {
                    koszyk.Produkty.Add(new KoszykSessionProduktDto
                    {
                        ProduktId = produkt.Id,
                        Nazwa = produkt.Nazwa,
                        Ilosc = 1
                    });
                }

                SaveSessionKoszyk(koszyk);
                return RedirectToAction("Index");
            }
        }
        public IActionResult ShowMyOrders()
        {
            var userId = _userManager.GetUserId(User);
            var user = _dbContext.Uzytkownicy.Find(userId);
            var zamowienia = _dbContext.Zamowienia;

                var mojeZamowienia = zamowienia.Where(z => z.UzytkownikId == user.Id).ToList();
                return View(mojeZamowienia);

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
        public IActionResult AddReview(int ZamowienieId)
        {
            var model = new Opinia
            {
                ZamowienieId = ZamowienieId,
                Ocena = 1
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult AddReview(Opinia opinia)
        {
            opinia.UzytkownikId = _userManager.GetUserId(User);
            _dbContext.Opinie.Add(opinia);

            var zamowienie = _dbContext.Zamowienia.Include(z => z.Opinia).FirstOrDefault(z => z.Id == opinia.ZamowienieId);
            zamowienie.Opinia = opinia;

            _dbContext.SaveChanges();

            return RedirectToAction("ShowOrder", new { id = opinia.ZamowienieId });
        }


        public IActionResult CancelOrder(int id)
        {
            var zamowienie = _dbContext.Zamowienia.FirstOrDefault(z => z.Id == id);
            if (zamowienie == null)
            {
                return NotFound();
            }
            zamowienie.Status = 0;
            _dbContext.SaveChanges();
            return RedirectToAction("ShowOrder", "Basket", new { id });
        }
        private KoszykSessionDto GetSessionKoszyk()
        {
            var json = HttpContext.Session.GetString(SessionKeyKoszyk);
            if (string.IsNullOrEmpty(json))
            {
                return new KoszykSessionDto();
            }
            return JsonSerializer.Deserialize<KoszykSessionDto>(json) ?? new KoszykSessionDto();
        }
        public IActionResult ReOrder(int id)
        {
            var userId = _userManager.GetUserId(User);
            var czyMaAktualneZamowienie = _dbContext.Zamowienia.Where(z => z.UzytkownikId == userId).Any(z => z.Status != 4 && z.Status != 0);
            if(czyMaAktualneZamowienie)
            {
                TempData["ErrorMessage"] = "Żeby ponownie coś zamówić, nie możesz mieć zamówienia aktualnie w realizacji!";
                return RedirectToAction("ShowOrder", new { id });
            }
            var oldOrder = _dbContext.Zamowienia
             .Include(z => z.ZamowioneProdukty)
             .FirstOrDefault(z => z.Id == id);
            var newOrder = new Zamowienie
            {
                UzytkownikId = oldOrder.UzytkownikId,
                Status = 1,
                PrzewidywanyCzasRealizacji = oldOrder.PrzewidywanyCzasRealizacji,
                CenaCalkowita = oldOrder.CenaCalkowita,
                Ulica = oldOrder.Ulica,
                NumerBudynku = oldOrder.NumerBudynku,
                Miejscowosc = oldOrder.Miejscowosc,
                KodPocztowy = oldOrder.KodPocztowy,
                DataZamowienia = oldOrder.DataZamowienia,
                ZamowioneProdukty = oldOrder.ZamowioneProdukty.Select(zp => new ZamowienieProdukt
                {
                    ProduktId = zp.ProduktId,
                    Ilosc = zp.Ilosc,
                    Cena = zp.Cena
                }).ToList()
            };
            _dbContext.Zamowienia.Add(newOrder);
            _dbContext.SaveChanges();
            TempData["SuccessMessage"] = "Zamówienie zostało złożone, dziękujemy! Numer zamówienia: " + newOrder.Id;
            return RedirectToAction("ShowMyOrders", "Basket");

        }
        private void SaveSessionKoszyk(KoszykSessionDto koszyk)
        {
            var json = JsonSerializer.Serialize(koszyk);
            HttpContext.Session.SetString(SessionKeyKoszyk, json);
        }

        [HttpPost]
        public IActionResult RemoveFromBasket(int id)
        {
            var userId = _userManager.GetUserId(User);

            var koszykProdukt = _dbContext.KoszykProdukty
                .Include(kp => kp.Koszyk)
                .FirstOrDefault(kp => kp.Id == id && kp.Koszyk.UzytkownikId == userId);

            if (koszykProdukt != null)
            {
                if(koszykProdukt.Ilosc > 1)
                {
                    koszykProdukt.Ilosc = koszykProdukt.Ilosc - 1;
                    _dbContext.SaveChanges();
                    TempData["SuccessMessage"] = "Usunięto sztukę produktu z koszyka!";
                }
                else
                {
                    _dbContext.KoszykProdukty.Remove(koszykProdukt);
                    _dbContext.SaveChanges();
                    TempData["SuccessMessage"] = "Usunięto produkt z koszyka!";
                }
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult RemoveFromSessionBasket(int id)
        {
            var koszyk = GetSessionKoszyk();

            var produkt = koszyk.Produkty.FirstOrDefault(p => p.ProduktId == id);
            if (produkt != null)
            {
                koszyk.Produkty.Remove(produkt);
                SaveSessionKoszyk(koszyk); 
            }

            return RedirectToAction("Index");
        }


    }

}
