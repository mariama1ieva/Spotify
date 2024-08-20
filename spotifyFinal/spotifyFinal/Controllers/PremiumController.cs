using Microsoft.AspNetCore.Mvc;

namespace spotifyFinal.Controllers
{
    public class PremiumController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Payment()
        {
            return View();
        }
    }
}
