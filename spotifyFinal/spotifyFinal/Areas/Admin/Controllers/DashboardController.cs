using Microsoft.AspNetCore.Mvc;

namespace spotifyFinal.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
