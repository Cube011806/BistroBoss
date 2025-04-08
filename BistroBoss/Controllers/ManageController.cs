using Microsoft.AspNetCore.Mvc;

namespace BistroBoss.Controllers
{
    public class ManageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
