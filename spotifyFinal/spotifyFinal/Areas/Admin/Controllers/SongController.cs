using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public SongController(ISongArtistService songArtistService, ISongService songService, IArtistService artistService, IWebHostEnvironment env,
                             ICategoryService categoryService, IAlbumService albumService, IMapper mapper)
        {
            _songArtistService = songArtistService;
            _songService = songService;
            _artistService = artistService;
            _env = env;
            _categoryService = categoryService;
            _albumService = albumService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await _songService.GetAllWithDatas();
            return View(model);
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
                ModelState.AddModelError("PhotoUrl", "File must be Image Format");
                return View(request);
            }

            if (!request.PhotoUrl.CheckFileSize(200))
            {
                ModelState.AddModelError("PhotoUrl", "Max File capacity must be 200KB");
                return View(request);
            }
            if (!request.SongUrl.CheckFileFormat("audio/"))
            {
                ModelState.AddModelError("SongUrl", "File must be mp3 format");
                return View(request);
            }

            string fileName = Guid.NewGuid().ToString() + "-" + request.PhotoUrl.FileName;
            string path = Path.Combine(_env.WebRootPath, "assets/images", fileName);
            await request.PhotoUrl.SaveFileToLocalAsync(path);

            string fileName2 = Guid.NewGuid().ToString() + "-" + request.SongUrl.FileName;
            string path2 = Path.Combine(_env.WebRootPath, "assets/music", fileName2);
            await request.SongUrl.SaveFileToLocalAsync(path2);

            request.ImageUrl = fileName;
            request.Path = fileName2;

            int songId = await _songService.CreateAsync(request);

            foreach (var artistId in request.ArtistIds)
            {
                await _songArtistService.CreateAsync(new SongArtistCreateVM() { ArtistId = artistId, SongId = songId });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (!ModelState.IsValid) return View();

            if (id == null) return BadRequest();
            var song = await _songService.GetByIdAsync((int)id);

            if (song == null) return NotFound();

            await _songService.DeleteAsync((int)id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.Artists = await _artistService.GetAllSelectListAsync(await _songArtistService.GetAllArtistIdsBySongId(id));
            ViewBag.Categories = await _categoryService.GetALlBySelectedAsync();
            ViewBag.Albums = await _albumService.GetALlBySelectedAsync();

            if (!ModelState.IsValid) return View();

            if (id == null) return BadRequest();

            var song = await _songService.GetByIdAsync((int)id);

            var model = _mapper.Map<SongEditVM>(song);
            model.Artists = await _songArtistService.GetAllBySongIdAsync(id);

            if (song == null) return NotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, SongEditVM request)
        {
            ViewBag.Artists = await _artistService.GetAllSelectListAsync(await _songArtistService.GetAllArtistIdsBySongId((int)id));
            ViewBag.Categories = await _categoryService.GetALlBySelectedAsync();
            ViewBag.Albums = await _albumService.GetALlBySelectedAsync();
            var existSong = await _songService.GetByIdAsync((int)id);

            if (!ModelState.IsValid) return View(request);

            if (id == null) return BadRequest();

            if (request.PhotoUrl != null)
            {
                if (!request.PhotoUrl.CheckFileFormat("image/"))
                {
                    ModelState.AddModelError("PhotoUrl", "File must be Image Format");
                    return View(request);
                }

                if (!request.PhotoUrl.CheckFileSize(2000))
                {
                    ModelState.AddModelError("PhotoUrl", "Max File capacity must be 200KB");
                    return View(request);
                }

                string fileName = Guid.NewGuid().ToString() + "-" + request.PhotoUrl.FileName;
                string path = Path.Combine(_env.WebRootPath, "assets/images", fileName);
                await request.PhotoUrl.SaveFileToLocalAsync(path);

                request.ImageUrl = fileName;

                FileExtention.DeleteFileFromLocalAsync(Path.Combine(_env.WebRootPath, "assets/images"), request.ImageUrl);
            }
            else
            {
                existSong.ImageUrl = request.ImageUrl;
            }

            if (!await _songService.AnyAsync(request.Name))
            {
                ModelState.AddModelError("Name", $"{request.Name} is already exist!");
                return View(request);
            }

            if (request.SongUrl != null)
            {
                if (!request.SongUrl.CheckFileFormat("mp3/"))
                {
                    ModelState.AddModelError("SongUrl", "File must be mp3 Format");
                    return View(request);
                }

                if (!request.SongUrl.CheckFileSize(200))
                {
                    ModelState.AddModelError("SongUrl", "Max File capacity must be 200KB");
                    return View(request);
                }

                string fileName1 = Guid.NewGuid().ToString() + "-" + request.SongUrl.FileName;
                string path1 = Path.Combine(_env.WebRootPath, "assets/music", fileName1);
                await request.SongUrl.SaveFileToLocalAsync(path1);

                request.Path = fileName1;

                FileExtention.DeleteFileFromLocalAsync(Path.Combine(_env.WebRootPath, "assets/music"), request.Path);
            }
            else
            {
                existSong.Path = request.Path;
            }

            await _songService.UpdateAsync((int)id, request);

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {

            return View(await _songService.GetDataIdWithCategoryArtistAlbum(id));
        }
    }
}
