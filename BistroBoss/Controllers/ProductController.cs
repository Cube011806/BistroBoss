using Microsoft.AspNetCore.Mvc;

namespace BistroBoss.Controllers
{
    public class ProductController : Controller
    {
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
    }
}
