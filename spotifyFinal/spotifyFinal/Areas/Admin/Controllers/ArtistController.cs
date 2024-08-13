using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;
using Service.ViewModels.ArtistPositionVMs;
using Service.ViewModels.ArtistVMs;
using Service.ViewModels.SongArtistVMs;
using spotifyFinal.Helpers.Extentions;

namespace spotifyFinal.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArtistController : Controller
    {
        private readonly IArtistPositionService _positionArtistService;
        private readonly IArtistService _artistService;
        private readonly IPositionService _positionService;
        private readonly ISongArtistService _songArtistService;
        private readonly ISongService _songService;

        private readonly IWebHostEnvironment _env;

        public ArtistController(IArtistPositionService positionArtistService,
               IPositionService positionService, IArtistService artistService, IWebHostEnvironment env, ISongArtistService songArtistService, ISongService songService)
        {
            _positionArtistService = positionArtistService;
            _positionService = positionService;
            _artistService = artistService;
            _env = env;
            _songArtistService = songArtistService;
            _songService = songService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await _artistService.GetAllWithDatas();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Positions = await _positionService.GetALlBySelectedAsync();
            ViewBag.Songs = await _songService.GetALlBySelectedAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ArtistCreateVM request)
        {
            ViewBag.Positions = await _positionService.GetALlBySelectedAsync();
            ViewBag.Songs = await _songService.GetALlBySelectedAsync();


            if (!ModelState.IsValid) return View(request);

            if (await _artistService.AnyAsync(request.FullName))
            {
                ModelState.AddModelError("FullName", $"{request.FullName} is already exist!");
                return View(request);
            }

            if (!request.Photo.CheckFileFormat("image/"))
            {
                ModelState.AddModelError("Photo", "File must be Image Format");
                return View(request);
            }

            string fileName = Guid.NewGuid().ToString() + "-" + request.Photo.FileName;
            string path = Path.Combine(_env.WebRootPath, "assets/images", fileName);
            await request.Photo.SaveFileToLocalAsync(path);

            request.ImageUrl = fileName;


            int artistId = await _artistService.CreateAsync(request);

            foreach (var songId in request.SongIds)
            {
                await _songArtistService.CreateAsync(new SongArtistCreateVM() { ArtistId = artistId, SongId = songId });
            }
            foreach (var positionId in request.PositionIds)
            {
                await _positionArtistService.CreateAsync(new ArtistPositionCreateVM() { ArtistId = artistId, PositionId = positionId });

            }


            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (!ModelState.IsValid) return View();

            if (id == null) return BadRequest();
            var song = await _artistService.GetByIdAsync((int)id);

            if (song == null) return NotFound();

            await _artistService.DeleteAsync((int)id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {

            return View(await _artistService.GetAllWithDatasById(id));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.Positions = await _positionService.GetALlBySelectedAsync();
            ViewBag.Songs = await _songService.GetALlBySelectedAsync();

            if (!ModelState.IsValid) return View();

            if (id == null) return BadRequest();

            var artist = await _artistService.GetAllWithDatasById((int)id);



            ArtistEditVM model = new()
            {
                FullName = artist.FullName,
                AboutImg = artist.AboutImg,
                Song = artist.ArtistSongs,
                Position = artist.ArtistPositions,
                //CategoryId = album.CategoryId,
                //ArtistId = album.ArtistId,
                //GroupId = album.GroupId,
                ImageUrl = artist.ImageUrl

            };

            if (artist == null) return NotFound();

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, ArtistEditVM request)
        {
            ViewBag.Positions = await _positionService.GetALlBySelectedAsync();
            ViewBag.Songs = await _songService.GetALlBySelectedAsync();
            var existArtist = await _artistService.GetAllWithDatasById((int)id);

            if (!ModelState.IsValid) return View(request);

            if (id == null) return BadRequest();


            if (request.Photo != null)
            {
                if (!request.Photo.CheckFileFormat("image/"))
                {
                    ModelState.AddModelError("Photo", "File must be Image Format");
                    return View(request);
                }

                if (!request.Photo.CheckFileSize(200))
                {
                    ModelState.AddModelError("Photo", "Max File capacity must be 200KB");
                    return View(request);
                }

                string fileName = Guid.NewGuid().ToString() + "-" + request.Photo.FileName;
                string path = Path.Combine(_env.WebRootPath, "assets/images", fileName);
                await request.Photo.SaveFileToLocalAsync(path);

                request.ImageUrl = fileName;

                FileExtention.DeleteFileFromLocalAsync(Path.Combine(_env.WebRootPath, "img"), request.ImageUrl);
            }
            else
            {
                existArtist.ImageUrl = request.ImageUrl;


            }
            if (!await _artistService.AnyAsync(request.FullName))
            {
                ModelState.AddModelError("FullName", $"{request.FullName} is already exist!");
                return View(request);
            }




            await _artistService.UpdateAsync((int)id, request);

            return RedirectToAction(nameof(Index));
        }
    }
}
