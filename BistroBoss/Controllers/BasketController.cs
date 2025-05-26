
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
    using System.Security.Claims;
    using System.Text.Json;

    public class BasketController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<Uzytkownik> _userManager;
        private const string SessionKeyKoszyk = "_Koszyk";

        public BasketController(ApplicationDbContext dbContext, UserManager<Uzytkownik> userManager)
        {
            _dbContext = dbContext;
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
                return RedirectToAction("Index");
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var zamowienia = _dbContext.Zamowienia;
            var mojeZamowienia = zamowienia.Where(z => z.UzytkownikId == userId).ToList();
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
                _dbContext.KoszykProdukty.Remove(koszykProdukt);
                _dbContext.SaveChanges();
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
