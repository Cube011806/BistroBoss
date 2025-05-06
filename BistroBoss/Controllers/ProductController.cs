using BistroBoss.Data;
using BistroBoss.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;

namespace BistroBoss.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<Uzytkownik> _userManager;

        public ProductController(ApplicationDbContext dbContext, UserManager<Uzytkownik> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Add()
        {
            UstawListeKategorii();
            return View();
        }

        [HttpPost]
        public IActionResult Add(Produkt produkt, string? nowaKategoria)
        {
            if (produkt.Nazwa.IsNullOrEmpty() || produkt.Opis.IsNullOrEmpty())
            {
                TempData["ErrorMessage"] = "Nazwa oraz opis produktu nie mogą być puste!";
                UstawListeKategorii();
                return View();
            }
            if (produkt.Cena <= 0)
            {
                TempData["ErrorMessage"] = "Cena produktu nie może być równa bądz mniejsza niż 0!";
                UstawListeKategorii();
                return View();
            }
            if (produkt.CzasPrzygotowania < 0)
            {
                TempData["ErrorMessage"] = "Czas przygotowania produktu nie może być mniejszy niż 0!";
                UstawListeKategorii();
                return View();
            }
            //Jeżeli nowa kategoria wpisana w formularzu nie jest pusta i nie jest białym znakiem to idziemy dalej
            if (!string.IsNullOrWhiteSpace(nowaKategoria))
            {
                //Jeżeli równocześnie kategoria z listy również została wybrana, to użytkownik wybrał
                //dwie kategorie dla produktu co jest niepoprawne.
                if (produkt.KategoriaId != 0)
                {
                    TempData["ErrorMessage"] = "Nie możesz wybrać kategorii z listy oraz podać nowej kategorii jednocześnie!";
                    UstawListeKategorii();
                    return View();
                }
                //Jeżeli kategoria wpisana jako nowa znajduje się już na liście kategorii (czyli w bazie), to
                //wyrzuca błąd, bo to też jest działaniem niepoprawnym
                if (_dbContext.Kategorie.Any(k => k.Nazwa == nowaKategoria))
                {
                    TempData["ErrorMessage"] = "Wpisana nazwa kategorii produktu znajduje się już na liście kategorii!";
                    UstawListeKategorii();
                    return View();
                }
                var nowaKategoriaDoDodania = new Kategoria { Nazwa = nowaKategoria };
                _dbContext.Kategorie.Add(nowaKategoriaDoDodania);
                _dbContext.SaveChanges();
                produkt.KategoriaId = nowaKategoriaDoDodania.Id;
                _dbContext.Produkty.Add(produkt);
                _dbContext.SaveChanges();
                TempData["SuccessMessage"] = "Produkt został pomyślnie dodany!";
                return RedirectToAction("Index", "Menu");
            }
            //Jeżeli kategoria wpisana jest pusta bądz jest białym znakiem (poprzedni warunek) oraz kategoria wybrana z listy też jest pusta (jej id to 0)
            //To znaczy, że użytkownik nie wybrał żadnej kategorii z listy i żadnej również nie wpisał (co nie jest poprawne).
            if (produkt.KategoriaId == 0)
            {
                TempData["ErrorMessage"] = "Musisz wybrać kategorie z listy, bądz dodać całkiem nową kategorie produktu!";
                UstawListeKategorii();
                return View();
            }
            //Dalej jest sytuacja w której użytkownik wybrał kategorię produktu z listy bez wpisania żadnej nowej w polu formularza
            _dbContext.Produkty.Add(produkt);
            _dbContext.SaveChanges();
            TempData["SuccessMessage"] = "Produkt został pomyślnie dodany!";
            return RedirectToAction("Index", "Menu");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var produkt = _dbContext.Produkty.FirstOrDefault(p => p.Id == id);
            if (produkt != null)
            {
                UstawListeKategorii();
                return View(produkt);
            }
            else
            {
                TempData["ErrorMessage"] = "Nie odnaleziono podanego produktu w menu!";
                return RedirectToAction("Index", "Menu");
            }
        }

        [HttpPost]
        public IActionResult Edit(Produkt produkt, string? nowaKategoria)
        {
            if (produkt.Nazwa.IsNullOrEmpty() || produkt.Opis.IsNullOrEmpty())
            {
                TempData["ErrorMessage"] = "Nazwa oraz opis produktu nie mogą być puste!";
                UstawListeKategorii();
                return View();
            }
            if (produkt.Cena <= 0)
            {
                TempData["ErrorMessage"] = "Cena produktu nie może być równa bądz mniejsza niż 0!";
                UstawListeKategorii();
                return View();
            }
            if (produkt.CzasPrzygotowania < 0)
            {
                TempData["ErrorMessage"] = "Czas przygotowania produktu nie może być mniejszy niż 0!";
                UstawListeKategorii();
                return View();
            }
            //Jeżeli nowa kategoria wpisana w formularzu nie jest pusta i nie jest białym znakiem to idziemy dalej
            if (!string.IsNullOrWhiteSpace(nowaKategoria))
            {
                //Jeżeli równocześnie kategoria z listy również została wybrana, to użytkownik wybrał
                //dwie kategorie dla produktu co jest niepoprawne.
                if (produkt.KategoriaId != 0)
                {
                    TempData["ErrorMessage"] = "Nie możesz wybrać kategorii z listy oraz podać nowej kategorii jednocześnie!";
                    UstawListeKategorii();
                    return View();
                }
                //Jeżeli kategoria wpisana jako nowa znajduje się już na liście kategorii (czyli w bazie), to
                //wyrzuca błąd, bo to też jest działaniem niepoprawnym
                if (_dbContext.Kategorie.Any(k => k.Nazwa == nowaKategoria))
                {
                    TempData["ErrorMessage"] = "Wpisana nazwa kategorii produktu znajduje się już na liście kategorii!";
                    UstawListeKategorii();
                    return View();
                }
                var nowaKategoriaDoDodania = new Kategoria { Nazwa = nowaKategoria };
                _dbContext.Kategorie.Add(nowaKategoriaDoDodania);
                _dbContext.SaveChanges();
                produkt.KategoriaId = nowaKategoriaDoDodania.Id;
                _dbContext.Produkty.Update(produkt);
                _dbContext.SaveChanges();
                TempData["SuccessMessage"] = "Produkt został pomyślnie edytowany!";
                return RedirectToAction("Index", "Menu");
            }
            //Jeżeli kategoria wpisana jest pusta bądz jest białym znakiem (poprzedni warunek) oraz kategoria wybrana z listy też jest pusta (jej id to 0)
            //To znaczy, że użytkownik nie wybrał żadnej kategorii z listy i żadnej również nie wpisał (co nie jest poprawne).
            if (produkt.KategoriaId == 0)
            {
                TempData["ErrorMessage"] = "Musisz wybrać kategorie z listy, bądz dodać całkiem nową kategorie produktu!";
                UstawListeKategorii();
                return View();
            }
            //Dalej jest sytuacja w której użytkownik wybrał kategorię produktu z listy bez wpisania żadnej nowej w polu formularza
            _dbContext.Produkty.Update(produkt);
            _dbContext.SaveChanges();
            TempData["SuccessMessage"] = "Produkt został pomyślnie edytowany!";
            return RedirectToAction("Index", "Menu");
        }

        private void UstawListeKategorii()
        {
            var kategorie = _dbContext.Kategorie.ToList();
            var listaKategorii = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = "",
                    Text = "-- Wybierz kategorię produktu --",
                    Selected = true
                }
            };
            listaKategorii.AddRange(kategorie.Select(k => new SelectListItem
            {
                Value = k.Id.ToString(),
                Text = k.Nazwa
            }));
            ViewBag.ListaKategorii = listaKategorii;
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var produkt = _dbContext.Produkty.FirstOrDefault(p => p.Id == id);
            if(produkt != null)
            {
                return View(produkt);
            }
            else
            {
                TempData["ErrorMessage"] = "Nie odnaleziono podanego produktu w menu!";
                return RedirectToAction("Index", "Menu");
            }
        }

        [HttpPost]
        public IActionResult Delete(Produkt produkt)
        {
            _dbContext.Remove(produkt);
            _dbContext.SaveChanges();
            TempData["SuccessMessage"] = "Pomyślnie usunięto produkt z menu!";
            return RedirectToAction("Index", "Menu");
        }
    }
}