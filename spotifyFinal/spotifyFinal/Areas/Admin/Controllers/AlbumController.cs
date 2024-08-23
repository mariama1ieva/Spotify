using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
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
        private readonly AppDbContext _context;

        public AlbumController(IAlbumService albumService, IWebHostEnvironment env, IArtistService artistService,
                               ICategoryService categoryService, IGroupService groupService, AppDbContext context)
        {
            _albumService = albumService;
            _env = env;
            _artistService = artistService;
            _categoryService = categoryService;
            _groupService = groupService;
            _context = context;
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
            ViewBag.Groups = await _groupService.GetALlBySelectedAsync();

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AlbumCreateVM request)
        {
            ViewBag.Artists = await _artistService.GetALlBySelectedAsync();
            ViewBag.Categories = await _categoryService.GetALlBySelectedAsync();
            ViewBag.Groups = await _groupService.GetALlBySelectedAsync();
            if (!ModelState.IsValid) return View(request);

            if (await _albumService.AnyAsync(request.Name))
            {
                ModelState.AddModelError("Name", $"{request.Name} is already exist!");
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
            var model = await _albumService.GetDataIdWithCategoryArtistGroup(id);

            return View(model);
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
            ViewBag.Groups = await _groupService.GetALlBySelectedAsync();

            if (id == null) return NotFound();
            Album? album = await _context.Albums.FirstOrDefaultAsync(c => c.Id == id);
            if (album == null) return NotFound();
            return View(new AlbumEditVM
            {
                Image = album.Image,
                Name = album.Name,
                CategoryId = album.CategoryId,
                ArtistId = (int)album.ArtistId,
                GroupId = (int)album.GroupId
            });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id, AlbumEditVM request)
        {
            ViewBag.Artists = await _artistService.GetALlBySelectedAsync();
            ViewBag.Categories = await _categoryService.GetALlBySelectedAsync();
            ViewBag.Groups = await _groupService.GetALlBySelectedAsync();

            if (id == null) return NotFound();
            Album? album = await _context.Albums.FirstOrDefaultAsync(c => c.Id == id);
            if (album == null) return NotFound();
            if (request.Photo != null)
            {
                if (request.Photo == null)
                {
                    ModelState.AddModelError("Photo", "Can not be empty!");
                    return View(request = new() { Image = album.Image });
                }
                if (!request.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "only image select");
                    return View(request = new() { Image = album.Image });
                }
                string fullPath = Path.Combine(_env.WebRootPath, "assets/images", album.Image);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                album.Image = request.Photo.SaveImage(_env, "assets/images", request.Photo.FileName);
                _context.SaveChanges();
            }

            album.Name = request.Name;
            album.ArtistId = request.ArtistId;
            album.CategoryId = request.CategoryId;
            album.GroupId = request.GroupId;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }
    }
}
