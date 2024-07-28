using Microsoft.AspNetCore.Mvc;

namespace spotifyFinal.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
