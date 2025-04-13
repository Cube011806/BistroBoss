using BistroBoss.Data;
using BistroBoss.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BistroBoss.Controllers
{
    public class ManageController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<Uzytkownik> _userManager;

        public ManageController(ApplicationDbContext dbContext, UserManager<Uzytkownik> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
