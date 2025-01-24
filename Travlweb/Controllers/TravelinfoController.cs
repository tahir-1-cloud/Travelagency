using Microsoft.AspNetCore.Mvc;

namespace Travlweb.Controllers
{
    public class TravelinfoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult aboutus()
        {
            return View();
        }
    }
}
