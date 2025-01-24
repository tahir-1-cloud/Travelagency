using Microsoft.AspNetCore.Mvc;

namespace Travlweb.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult admindetail() 
        {
            return View();
        }
    }
}
