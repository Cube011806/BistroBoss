
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
    using Microsoft.IdentityModel.Tokens;
    using NuGet.Protocol;
    using System.Security.Claims;
    using System.Text.Json;

    public class BasketController : BaseController
    {
        private readonly UserManager<Uzytkownik> _userManager;
        private readonly IEmailService _emailService;
        private const string SessionKeyKoszyk = "_Koszyk";

        public BasketController(ApplicationDbContext dbContext, UserManager<Uzytkownik> userManager, IEmailService emailService) : base(dbContext)
        {
            _userManager = userManager;
            _emailService = emailService;
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
                var guestUser = _userManager.FindByIdAsync("40000000").GetAwaiter().GetResult();

                if (guestUser == null)
                {
                    guestUser = new Uzytkownik
                    {
                        Id = "40000000",
                        UserName = "guest-user",
                        Email = "guest@example.com",
                        EmailConfirmed = true,
                        Imie = "Gość",
                        Nazwisko = "Systemowy",
                        AccessLevel = 0,
                        Koszyk = new Koszyk()
                    };

                    var result = _userManager.CreateAsync(guestUser).GetAwaiter().GetResult();
                    if (!result.Succeeded)
                    {
                        var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                        throw new Exception($"Nie udało się utworzyć użytkownika gościa: {errors}");
                    }
                }

                var koszyk = _dbContext.Koszyki
                    .Include(k => k.KoszykProdukty)
                    .ThenInclude(kp => kp.Produkt)
                    .FirstOrDefault(k => k.UzytkownikId == guestUser.Id);

                if (koszyk == null)
                {
                    koszyk = new Koszyk { UzytkownikId = guestUser.Id };
                    _dbContext.Koszyki.Add(koszyk);
                    _dbContext.SaveChanges();
                }

                return View("IndexLoggedIn", koszyk);
            }
        }

        public IActionResult AddToBasket(int produktId)
        {
            var produkt = _dbContext.Produkty.Find(produktId);
            if (produkt == null)
                return NotFound();

            string userId;

            if (User.Identity.IsAuthenticated)
            {
                userId = _userManager.GetUserId(User);
            }
            else
            {
                var guestUser = EnsureGuestUser();
                userId = guestUser.Id;
            }

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
            return RedirectToAction("Index", "Menu");
        }

        private Uzytkownik EnsureGuestUser()
        {
            return EnsureGuestUserAsync().GetAwaiter().GetResult();
        }

        private async Task<Uzytkownik> EnsureGuestUserAsync()
        {
            var guestUser = await _userManager.FindByIdAsync("4000000");

            if (guestUser == null)
            {
                guestUser = new Uzytkownik
                {
                    Id = "40000000",
                    UserName = "guest-user",
                    Email = "guest@example.com",
                    EmailConfirmed = true,
                    Imie = "Gość",
                    Nazwisko = "Systemowy",
                    AccessLevel = 0,
                    Koszyk = new Koszyk()
                };

                var result = await _userManager.CreateAsync(guestUser);

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception("Guest user creation failed: " + errors);
                }
            }

            return guestUser;
        }

        public IActionResult ShowMyOrders()
        {
            var userId = _userManager.GetUserId(User);
            var user = _dbContext.Uzytkownicy.Find(userId);
            var zamowienia = _dbContext.Zamowienia;

                var mojeZamowienia = zamowienia.Where(z => z.UzytkownikId == user.Id).OrderByDescending(z => z.Id).ToList();
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
            if(opinia.Ocena < 1 || opinia.Ocena > 5 || opinia.Komentarz.IsNullOrEmpty())
            {
                TempData["ErrorMessage"] = "Wszystkie pola muszą być wypełnione!";
                return RedirectToAction("AddReview", new { opinia.ZamowienieId });
            }
            opinia.UzytkownikId = _userManager.GetUserId(User);
            _dbContext.Opinie.Add(opinia);

            var zamowienie = _dbContext.Zamowienia.Include(z => z.Opinia).FirstOrDefault(z => z.Id == opinia.ZamowienieId);
            zamowienie.Opinia = opinia;

            _dbContext.SaveChanges();
            TempData["SuccessMessage"] = "Pomyślnie dodano opinię!";
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
            TempData["SuccessMessage"] = "Pomyślnie anulowano zamówienie!";
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
        [HttpPost]
        public IActionResult ReOrder(int id, bool sposobDostawy, string Miejscowosc, string Ulica, string NumerBudynku, string KodPocztowy)
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
                SposobDostawy = sposobDostawy,
                Imie = oldOrder.Imie,
                Nazwisko = oldOrder.Nazwisko,
                Email = oldOrder.Email,
                NumerTelefonu = oldOrder.NumerTelefonu,
                Ulica = sposobDostawy ? Ulica : "",
                NumerBudynku = sposobDostawy ? NumerBudynku : "",
                Miejscowosc = sposobDostawy ? Miejscowosc : "",
                KodPocztowy = sposobDostawy ? KodPocztowy : "",
                DataZamowienia = DateTime.Now,
                ZamowioneProdukty = oldOrder.ZamowioneProdukty.Select(zp => new ZamowienieProdukt
                {
                    ProduktId = zp.ProduktId,
                    Ilosc = zp.Ilosc,
                    Cena = zp.Cena
                }).ToList()
            };
            _dbContext.Zamowienia.Add(newOrder);
            _dbContext.SaveChanges();
            var user = _dbContext.Uzytkownicy.FirstOrDefault(u => u.Id == userId);
            string message = $@"
                <html>
                    <body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
                        <h2 style='color: #4CAF50;'>Dziękujemy za Twoje zamówienie!</h2>
                        <p><strong>Numer zamówienia:</strong> {newOrder.Id}</p>
                        <p><strong>Data zamówienia:</strong> {newOrder.DataZamowienia:dd.MM.yyyy HH:mm}</p>
                        <p><strong>Przewidywany czas realizacji:</strong> {newOrder.PrzewidywanyCzasRealizacji} minut</p>
                        <p><strong>Cena całkowita:</strong> {newOrder.CenaCalkowita} zł</p>
                        <p><strong>Adres dostawy:</strong><br />
                            {newOrder.Miejscowosc}, {newOrder.Ulica} {newOrder.NumerBudynku}<br />
                            {newOrder.KodPocztowy}
                        </p>
                        <hr style='margin: 20px 0;' />
                        <p>W razie pytań prosimy o kontakt z naszym działem obsługi klienta.</p>
                        <p style='color: #777;'>Pozdrawiamy,<br />Zespół BistroBoss</p>
                    </body>
                </html>";
            string message2 = $@"
                <html>
                    <body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
                        <h2 style='color: #4CAF50;'>Dziękujemy za Twoje zamówienie!</h2>
                        <p><strong>Numer zamówienia:</strong> {newOrder.Id}</p>
                        <p><strong>Data zamówienia:</strong> {newOrder.DataZamowienia:dd.MM.yyyy HH:mm}</p>
                        <p><strong>Przewidywany czas realizacji:</strong> {newOrder.PrzewidywanyCzasRealizacji} minut</p>
                        <p><strong>Cena całkowita:</strong> {newOrder.CenaCalkowita} zł</p>
                        <hr style='margin: 20px 0;' />
                        <p>W razie pytań prosimy o kontakt z naszym działem obsługi klienta.</p>
                        <p style='color: #777;'>Pozdrawiamy,<br />Zespół BistroBoss</p>
                    </body>
                </html>";
            if (!newOrder.SposobDostawy)
            {
                _emailService.SendEmail(user.Email, "Nowe zamówienie", message2);
            }
            else
            {
                _emailService.SendEmail(user.Email, "Nowe zamówienie", message);
            }
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
                if (produkt.Ilosc > 1)
                {
                    produkt.Ilosc = produkt.Ilosc - 1;
                    _dbContext.SaveChanges();
                    TempData["SuccessMessage"] = "Usunięto sztukę produktu z koszyka!";
                }
                else
                {
                    koszyk.Produkty.Remove(produkt);
                    _dbContext.SaveChanges();
                    TempData["SuccessMessage"] = "Usunięto produkt z koszyka!";
                }
                koszyk.Produkty.Remove(produkt);
                SaveSessionKoszyk(koszyk); 
            }

            return RedirectToAction("Index");
        }


    }

}
