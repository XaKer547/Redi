using Microsoft.AspNetCore.Mvc;

namespace Redi.Deliverer.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
