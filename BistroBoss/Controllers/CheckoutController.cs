using Microsoft.AspNetCore.Mvc;

namespace BistroBoss.Controllers
{
    public class CheckoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
