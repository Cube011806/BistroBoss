using BistroBoss.Data;
using BistroBoss.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BistroBoss.Controllers
{
    public class ProductController : BaseController
    {
        private readonly UserManager<Uzytkownik> _userManager;

        public ProductController(ApplicationDbContext dbContext, UserManager<Uzytkownik> userManager) : base(dbContext)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Add()
        {
            UstawListeKategorii();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Produkt produkt, string? nowaKategoria, IFormFile? zdjeciePlik)
        {
            if (string.IsNullOrWhiteSpace(produkt.Nazwa) || string.IsNullOrWhiteSpace(produkt.Opis))
            {
                TempData["ErrorMessage"] = "Nazwa oraz opis produktu nie mogą być puste!";
                UstawListeKategorii();
                return View();
            }
            if (produkt.Cena <= 0)
            {
                TempData["ErrorMessage"] = "Cena produktu nie może być równa bądź mniejsza niż 0!";
                UstawListeKategorii();
                return View();
            }
            if (produkt.CzasPrzygotowania < 0)
            {
                TempData["ErrorMessage"] = "Czas przygotowania produktu nie może być mniejszy niż 0!";
                UstawListeKategorii();
                return View();
            }

            // Obsługa dodawania zdjęcia
            if (zdjeciePlik != null && zdjeciePlik.Length > 0)
            {
                var folderPath = Path.Combine("wwwroot", "images", "produkty");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(zdjeciePlik.FileName);
                var filePath = Path.Combine(folderPath, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await zdjeciePlik.CopyToAsync(stream);
                }

                produkt.Zdjecie = "/images/produkty/" + uniqueFileName;
            }

            // Dodanie nowej kategorii
            if (!string.IsNullOrWhiteSpace(nowaKategoria))
            {
                if (produkt.KategoriaId != 0)
                {
                    TempData["ErrorMessage"] = "Nie możesz wybrać kategorii z listy oraz podać nowej kategorii jednocześnie!";
                    UstawListeKategorii();
                    return View();
                }

                if (_dbContext.Kategorie.Any(k => k.Nazwa == nowaKategoria))
                {
                    TempData["ErrorMessage"] = "Wpisana nazwa kategorii produktu znajduje się już na liście kategorii!";
                    UstawListeKategorii();
                    return View();
                }

                var nowaKategoriaDoDodania = new Kategoria { Nazwa = nowaKategoria };
                _dbContext.Kategorie.Add(nowaKategoriaDoDodania);
                await _dbContext.SaveChangesAsync();

                produkt.KategoriaId = nowaKategoriaDoDodania.Id;
                _dbContext.Produkty.Add(produkt);
                await _dbContext.SaveChangesAsync();

                TempData["SuccessMessage"] = "Produkt został pomyślnie dodany!";
                return RedirectToAction("Index", "Menu");
            }

            if (produkt.KategoriaId == 0)
            {
                TempData["ErrorMessage"] = "Musisz wybrać kategorię z listy bądź dodać nową kategorię produktu!";
                UstawListeKategorii();
                return View();
            }

            // Kategoria z listy
            _dbContext.Produkty.Add(produkt);
            await _dbContext.SaveChangesAsync();

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
        public IActionResult Edit(Produkt produkt, string? nowaKategoria, IFormFile? zdjeciePlik)
        {
            if (produkt.Nazwa.IsNullOrEmpty() || produkt.Opis.IsNullOrEmpty())
            {
                TempData["ErrorMessage"] = "Nazwa oraz opis produktu nie mogą być puste!";
                UstawListeKategorii();
                return View(produkt);
            }

            if (produkt.Cena <= 0)
            {
                TempData["ErrorMessage"] = "Cena produktu nie może być równa bądz mniejsza niż 0!";
                UstawListeKategorii();
                return View(produkt);
            }

            if (produkt.CzasPrzygotowania < 0)
            {
                TempData["ErrorMessage"] = "Czas przygotowania produktu nie może być mniejszy niż 0!";
                UstawListeKategorii();
                return View(produkt);
            }

            // Obsługa nowej kategorii
            if (!string.IsNullOrWhiteSpace(nowaKategoria))
            {
                if (produkt.KategoriaId != 0)
                {
                    TempData["ErrorMessage"] = "Nie możesz wybrać kategorii z listy oraz podać nowej kategorii jednocześnie!";
                    UstawListeKategorii();
                    return View(produkt);
                }

                if (_dbContext.Kategorie.Any(k => k.Nazwa == nowaKategoria))
                {
                    TempData["ErrorMessage"] = "Wpisana nazwa kategorii produktu znajduje się już na liście kategorii!";
                    UstawListeKategorii();
                    return View(produkt);
                }

                var nowaKategoriaDoDodania = new Kategoria { Nazwa = nowaKategoria };
                _dbContext.Kategorie.Add(nowaKategoriaDoDodania);
                _dbContext.SaveChanges();
                produkt.KategoriaId = nowaKategoriaDoDodania.Id;
            }

            if (produkt.KategoriaId == 0)
            {
                TempData["ErrorMessage"] = "Musisz wybrać kategorię z listy, bądź dodać nową!";
                UstawListeKategorii();
                return View(produkt);
            }

            // 🖼️ Obsługa zdjęcia
            if (zdjeciePlik != null && zdjeciePlik.Length > 0)
            {
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/produkty");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(zdjeciePlik.FileName);
                var filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    zdjeciePlik.CopyTo(stream);
                }

                // Zaktualizuj ścieżkę zdjęcia (np. /images/produkty/nazwa.jpg)
                produkt.Zdjecie = "/images/produkty/" + fileName;
            }

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
            var produkt = _dbContext.Produkty.Include(p => p.Kategoria).FirstOrDefault(p => p.Id == id);
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
            var produktZKategoria = _dbContext.Produkty.FirstOrDefault(p => p.Id == produkt.Id);
            var kategoriaId = produktZKategoria.KategoriaId;

            _dbContext.Remove(produktZKategoria);
            _dbContext.SaveChanges();

            bool czyToOstatniZKategorii = !_dbContext.Produkty.Any(p => p.KategoriaId == kategoriaId);

            if (czyToOstatniZKategorii)
            {
                var kategoria = _dbContext.Kategorie.FirstOrDefault(k => k.Id == kategoriaId);
                if (kategoria != null)
                {
                    _dbContext.Kategorie.Remove(kategoria);
                    _dbContext.SaveChanges();
                }
            }

            TempData["SuccessMessage"] = "Produkt został pomyślnie usunięty!";
            return RedirectToAction("Index", "Menu");
        }
    }
}