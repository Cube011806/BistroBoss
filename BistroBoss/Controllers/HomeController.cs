using BistroBoss.Data;
using BistroBoss.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BistroBoss.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _dbContext;

        public HomeController(ApplicationDbContext dbContext, ILogger<HomeController> logger)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var users = _dbContext.Uzytkownicy;

            if (users.Where(u=>u.AccessLevel == 1 && u.Email!=null).Count() < 1)
            {
                var admin = users.FirstOrDefault();
                admin.AccessLevel = 1;
                _dbContext.Uzytkownicy.Update(admin);
                _dbContext.SaveChanges();
                Console.WriteLine("Podniesiono poziom uprawnieñ u¿ytkownika: " + admin.Email);
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
