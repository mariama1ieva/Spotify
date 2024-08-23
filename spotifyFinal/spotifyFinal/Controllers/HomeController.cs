using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Service.ViewModels;

namespace spotifyFinal.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var homeVM = new HomeVM
            {
                Albums = await _context.Albums.Include(m => m.Category).Include(m => m.Artist).OrderByDescending(a => a.Id).Take(5).ToListAsync(),
                Playlists = await _context.Playlist.ToListAsync(),
                Artists = await _context.Artists.Include(m => m.ArtistSongs).OrderByDescending(a => a.Id).Take(5).ToListAsync(),
            };

            return View(homeVM);
        }

        [Route("/StatusCodeError/{statusCode}")]
        public IActionResult Error(int statusCode)
        {
            if (statusCode == 404)
            {
                ViewBag.ErrorMessage = "Page could not be found !";
            }

            return View("Error");

        }
    }
}
