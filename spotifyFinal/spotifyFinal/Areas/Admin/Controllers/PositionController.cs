using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;
using Service.ViewModels.ArtistPositionVMs;
using Service.ViewModels.PositionVMs;

namespace spotifyFinal.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PositionController : Controller
    {
        private readonly IArtistPositionService _positionArtistService;
        private readonly IPositionService _positionService;
        private readonly IArtistService _artistService;

        private readonly IWebHostEnvironment _env;

        public PositionController(IArtistPositionService positionArtistService, IPositionService positionService, IArtistService artistService, IWebHostEnvironment env)
        {
            _positionArtistService = positionArtistService;
            _positionService = positionService;
            _artistService = artistService;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await _positionService.GetAllWithDatas();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Artists = await _artistService.GetALlBySelectedAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PositionCreateVM request)
        {
            ViewBag.Artists = await _artistService.GetALlBySelectedAsync();

            if (!ModelState.IsValid) return View(request);

            if (await _positionService.AnyAsync(request.Name))
            {
                ModelState.AddModelError("Name", $"{request.Name} is already exist!");
                return View(request);
            }


            int positionId = await _positionService.CreateAsync(request);

            foreach (var artistId in request.ArtistIds)
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
            var song = await _positionService.GetByIdAsync((int)id);

            if (song == null) return NotFound();

            await _positionService.DeleteAsync((int)id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {

            return View(await _positionService.GetAllWithDatasById(id));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Artists = await _artistService.GetALlBySelectedAsync();

            // Validate the ModelState
            if (!ModelState.IsValid) return View();

            if (id == null) return BadRequest();

            var position = await _positionService.GetAllWithDatasById((int)id);

            if (position == null) return NotFound();

            PositionEditVM model = new PositionEditVM
            {
                Name = position.Name,
                ArtistIds = position.ArtistPositions.Select(ap => ap.ArtistId).ToList()
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id, PositionEditVM request)
        {
            ViewBag.Artists = await _artistService.GetALlBySelectedAsync();

            var existPosition = await _positionService.GetAllWithDatasById((int)id);


            if (!ModelState.IsValid) return View(request);

            if (id == null) return BadRequest();

            if (!await _positionService.AnyAsync(request.Name))
            {
                ModelState.AddModelError("Name", $"{request.Name} is already exist!");
                return View(request);
            }

            //existAlbum.Name = request.Name;
            //existAlbum.CategoryId = request.CategoryId;
            //existAlbum.GroupId = request.GroupId;
            //existAlbum.ArtistId = request.ArtistId;

            await _positionService.UpdateAsync((int)id, request);

            return RedirectToAction(nameof(Index));


        }
    }
}
