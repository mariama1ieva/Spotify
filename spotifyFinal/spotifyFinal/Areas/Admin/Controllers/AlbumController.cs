using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;
using Service.ViewModels.AlbumVMs;
using spotifyFinal.Helpers.Extentions;

namespace spotifyFinal.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AlbumController : Controller
    {
        private readonly IAlbumService _albumService;
        private readonly IWebHostEnvironment _env;
        private readonly IArtistService _artistService;
        private readonly ICategoryService _categoryService;
        private readonly IGroupService _groupService;



        public AlbumController(IAlbumService albumService, IWebHostEnvironment env, IArtistService artistService,
                               ICategoryService categoryService, IGroupService groupService)
        {
            _albumService = albumService;
            _env = env;
            _artistService = artistService;
            _categoryService = categoryService;
            _groupService = groupService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _albumService.GetAllWithCategoryArtistGroup());
        }

        [HttpGet]

        public async Task<IActionResult> Create()
        {
            ViewBag.Artists = await _artistService.GetALlBySelectedAsync();
            ViewBag.Categories = await _categoryService.GetALlBySelectedAsync();
            ViewBag.Categories = await _groupService.GetALlBySelectedAsync();

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AlbumCreateVM request)
        {
            ViewBag.Artists = await _artistService.GetALlBySelectedAsync();
            ViewBag.Categories = await _categoryService.GetALlBySelectedAsync();
            ViewBag.Categories = await _groupService.GetALlBySelectedAsync();
            if (!ModelState.IsValid) return View(request);

            if (await _albumService.AnyAsync(request.Name, request.Image))
            {
                ModelState.AddModelError("Name", $"{request.Name} is already exist!");
                ModelState.AddModelError("Image", $"{request.Image} is already exist!");
                return View(request);
            }


            if (!request.Photo.CheckFileFormat("image/"))
            {
                ModelState.AddModelError("Photo", "File must be Image Format");
                return View(request);
            }

            if (!request.Photo.CheckFileSize(200))
            {
                ModelState.AddModelError("Photo", "Max File capacity must be 200KB");
                return View();
            }

            string fileName = Guid.NewGuid().ToString() + "-" + request.Photo.FileName;
            string path = Path.Combine(_env.WebRootPath, "assets/images", fileName);
            await request.Photo.SaveFileToLocalAsync(path);

            request.Image = fileName;

            await _albumService.CreateAsync(request);
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {

            return View(await _albumService.GetByIdAsync(id));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (!ModelState.IsValid) return View();

            if (id == null) return BadRequest();
            var album = await _albumService.GetByIdAsync((int)id);

            if (album == null) return NotFound();

            await _albumService.DeleteAsync((int)id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Artists = await _artistService.GetALlBySelectedAsync();
            ViewBag.Categories = await _categoryService.GetALlBySelectedAsync();
            ViewBag.Categories = await _groupService.GetALlBySelectedAsync();

            if (!ModelState.IsValid) return View();

            if (id == null) return BadRequest();

            var album = await _albumService.GetByIdAsync((int)id);

            AlbumEditVM model = new()
            {
                Image = album.Image,
                Name = album.Name,
                Category = album.Category,
                Group = album.Group,
                Artist = album.Artist,

            };

            if (model == null) return NotFound();

            return View(model);
        }
    }
}
