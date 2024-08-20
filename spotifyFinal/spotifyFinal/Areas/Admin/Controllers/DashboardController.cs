using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace spotifyFinal.Areas.Admin.Controllers
{
    [Area("Admin")]


    public class DashboardController : Controller
    {
        [Authorize(Roles = "Admin,SuperAdmin")]


        public IActionResult Index()
        {
            return View();
        }
    }
}
