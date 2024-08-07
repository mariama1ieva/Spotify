using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;
using Service.ViewModels.SongArtistVMs;
using Service.ViewModels.SongVMs;

namespace spotifyFinal.Areas.Admin.Controllers
{
    public class SongController : Controller
    {
        private readonly ISongArtistService _songArtistService;
        private readonly ISongService _songService;
        private readonly IArtistService _artistService;

        public SongController(ISongArtistService songArtistService, ISongService songService, IArtistService artistService)
        {
            _songArtistService = songArtistService;
            _songService = songService;
            _artistService = artistService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Artists = await _artistService.GetALlBySelectedAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SongCreateVM request)
        {
            int songId = await _songService.CreateAsync(request);

            foreach (var artistId in request.ArtistIds)
            {
                await _songArtistService.CreateAsync(new SongArtistCreateVM() { ArtistId = artistId, SongId = songId });
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
