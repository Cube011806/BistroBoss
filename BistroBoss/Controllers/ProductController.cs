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
            var kategorie = _dbContext.Kategorie.ToList();
            ViewBag.ListaKategorii = new SelectList(kategorie, "Id", "Nazwa");
            return View();
        }

        [HttpPost]
        public IActionResult Add(Produkt produkt, string? nowaKategoria)
        {
            if (produkt.Nazwa.IsNullOrEmpty() || produkt.Opis.IsNullOrEmpty())
            {
                TempData["ErrorMessage"] = "Nazwa oraz opis produktu nie mogą być puste!";
                return View();
            }
            if (produkt.Cena <= 0)
            {
                TempData["ErrorMessage"] = "Cena produktu nie może być równa bądz mniejsza niż 0!";
                return View();
            }
            if (produkt.CzasPrzygotowania < 0)
            {
                TempData["ErrorMessage"] = "Czas przygotowania produktu nie może być mniejszy niż 0!";
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
                    return View();
                }
                //Jeżeli kategoria wpisana jako nowa znajduje się już na liście kategorii (czyli w bazie), to
                //wyrzuca błąd, bo to też jest działaniem niepoprawnym
                if (_dbContext.Kategorie.Any(k => k.Nazwa == nowaKategoria))
                {
                    TempData["ErrorMessage"] = "Wpisana nazwa kategorii produktu znajduje się już na liście kategorii!";
                    return View();
                }
                var nowaKategoriaDoDodania = new Kategoria { Nazwa = nowaKategoria };
                _dbContext.Kategorie.Add(nowaKategoriaDoDodania);
                _dbContext.SaveChanges();
                produkt.KategoriaId = nowaKategoriaDoDodania.Id;
                _dbContext.Produkty.Add(produkt); 
                _dbContext.SaveChanges();
                TempData["SuccessMessage"] = "Produkt został pomyślnie dodany!";
                return RedirectToAction("Index", "Home");
            }
            //Jeżeli kategoria wpisana jest pusta bądz jest białym znakiem (poprzedni warunek) oraz kategoria wybrana z listy też jest pusta (jej id to 0)
            //To znaczy, że użytkownik nie wybrał żadnej kategorii z listy i żadnej również nie wpisał (co nie jest poprawne).
            if (produkt.KategoriaId == 0)
            {
                TempData["ErrorMessage"] = "Musisz wybrać kategorie z listy, bądz dodać całkiem nową kategorie produktu!";
                return View();
            }
            //Dalej jest sytuacja w której użytkownik wybrał kategorię produktu z listy bez wpisania żadnej nowej w polu formularza
            _dbContext.Produkty.Add(produkt);
            _dbContext.SaveChanges();
            TempData["SuccessMessage"] = "Produkt został pomyślnie dodany!";
            return RedirectToAction("Index", "Home");
        }
    }
}
