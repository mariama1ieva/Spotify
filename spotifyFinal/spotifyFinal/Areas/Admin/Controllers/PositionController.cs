using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Repository.Data;
using Service.Services.Interfaces;
using Service.ViewModels.PositionVMs;

namespace spotifyFinal.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PositionController : Controller
    {
        private readonly IArtistPositionService _positionArtistService;
        private readonly IPositionService _positionService;
        private readonly IArtistService _artistService;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public PositionController(IArtistPositionService positionArtistService, IPositionService positionService, IArtistService artistService, IWebHostEnvironment env, AppDbContext context)
        {
            _positionArtistService = positionArtistService;
            _positionService = positionService;
            _artistService = artistService;
            _env = env;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await _positionService.GetAllWithDatas();
            return View(model);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PositionCreateVM position)
        {
            if (!ModelState.IsValid) return View();

            Position newPosition = new()
            {
                Name = position.Name
            };

            await _context.Positions.AddAsync(newPosition);
            await _context.SaveChangesAsync();

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
        public IActionResult Edit(int id)
        {
            if (id == 0) return BadRequest();

            PositionEditVM position = _context.Positions.Select(p => new PositionEditVM()
            {
                Id = p.Id,
                Name = p.Name,

            }).FirstOrDefault(p => p.Id == id)!;
            if (position is null) return NotFound();

            return View(position);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, PositionEditVM positionUpdateVM)
        {
            if (id == 0) return BadRequest();

            var position = await _context.Positions.FindAsync(id);

            if (position == null) return NotFound();

            if (!ModelState.IsValid) return View(positionUpdateVM);

            position.Name = positionUpdateVM.Name;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
