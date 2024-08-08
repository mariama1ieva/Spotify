using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;
using Service.ViewModels.SongArtistVMs;
using Service.ViewModels.SongVMs;
using spotifyFinal.Helpers.Extentions;

namespace spotifyFinal.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SongController : Controller
    {
        private readonly ISongArtistService _songArtistService;
        private readonly ISongService _songService;
        private readonly IArtistService _artistService;
        private readonly ICategoryService _categoryService;
        private readonly IAlbumService _albumService;

        private readonly IWebHostEnvironment _env;

        public SongController(ISongArtistService songArtistService, ISongService songService, IArtistService artistService, IWebHostEnvironment env,
                             ICategoryService categoryService, IAlbumService albumService)
        {
            _songArtistService = songArtistService;
            _songService = songService;
            _artistService = artistService;
            _env = env;
            _categoryService = categoryService;
            _albumService = albumService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _songService.GetAllWithDatas());
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Artists = await _artistService.GetALlBySelectedAsync();
            ViewBag.Categories = await _categoryService.GetALlBySelectedAsync();
            ViewBag.Albums = await _albumService.GetALlBySelectedAsync();


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SongCreateVM request)
        {
            ViewBag.Artists = await _artistService.GetALlBySelectedAsync();
            ViewBag.Categories = await _categoryService.GetALlBySelectedAsync();
            ViewBag.Albums = await _albumService.GetALlBySelectedAsync();

            if (!ModelState.IsValid) return View(request);

            if (await _songService.AnyAsync(request.Name))
            {
                ModelState.AddModelError("Name", $"{request.Name} is already exist!");
                return View(request);
            }

            if (!request.PhotoUrl.CheckFileFormat("image/"))
            {
                ModelState.AddModelError("Photo", "File must be Image Format");
                return View(request);
            }

            if (!request.PhotoUrl.CheckFileSize(200))
            {
                ModelState.AddModelError("Photo", "Max File capacity must be 200KB");
                return View();
            }
            if (!request.SongUrl.CheckFileFormat("mp3/"))
            {
                ModelState.AddModelError("Song", "File must be mp3 format");
                return View(request);
            }


            string fileName = Guid.NewGuid().ToString() + "-" + request.PhotoUrl.FileName;
            string path = Path.Combine(_env.WebRootPath, "assets/images", fileName);
            await request.PhotoUrl.SaveFileToLocalAsync(path);

            string fileName2 = Guid.NewGuid().ToString() + "-" + request.SongUrl.FileName;
            string path2 = Path.Combine(_env.WebRootPath, "assets/music", fileName2);
            await request.PhotoUrl.SaveFileToLocalAsync(path2);

            int songId = await _songService.CreateAsync(request);

            foreach (var artistId in request.ArtistIds)
            {
                await _songArtistService.CreateAsync(new SongArtistCreateVM() { ArtistId = artistId, SongId = songId });
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
