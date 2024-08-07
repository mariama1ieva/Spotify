using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;

namespace spotifyFinal.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AlbumController : Controller
    {
        private readonly IAlbumService _albumService;
        private readonly IWebHostEnvironment _env;

        public AlbumController(IAlbumService albumService, IWebHostEnvironment env)
        {
            _albumService = albumService;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _albumService.GetAllWithCategoryArtistGroup());
        }
    }
}
