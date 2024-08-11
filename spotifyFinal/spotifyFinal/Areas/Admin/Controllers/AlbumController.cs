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

            if (!ModelState.IsValid) return View();

            if (id == null) return BadRequest();

            var album = await _albumService.GetDataIdWithCategoryArtistGroup((int)id);



            AlbumEditVM model = new()
            {
                Name = album.Name,
                CategoryName = album.CategoryName,
                ArtistFullName = album.ArtistFullName,
                GroupName = album.GroupName,
                //CategoryId = album.CategoryId,
                //ArtistId = album.ArtistId,
                //GroupId = album.GroupId,
                Image = album.Image,

            };

            if (album == null) return NotFound();

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id, AlbumEditVM request)
        {
            ViewBag.Artists = await _artistService.GetALlBySelectedAsync();
            ViewBag.Categories = await _categoryService.GetALlBySelectedAsync();
            ViewBag.Groups = await _groupService.GetALlBySelectedAsync();

            var existAlbum = await _albumService.GetDataIdWithCategoryArtistGroup((int)id);


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

                request.Image = fileName;

                FileExtention.DeleteFileFromLocalAsync(Path.Combine(_env.WebRootPath, "img"), request.Image);
            }
            else
            {
                existAlbum.Image = request.Image;


            }
            if (!await _albumService.AnyAsync(request.Name))
            {
                ModelState.AddModelError("Name", $"{request.Name} is already exist!");
                return View(request);
            }

            //existAlbum.Name = request.Name;
            //existAlbum.CategoryId = request.CategoryId;
            //existAlbum.GroupId = request.GroupId;
            //existAlbum.ArtistId = request.ArtistId;

            await _albumService.UpdateAsync((int)id, request);

            return RedirectToAction(nameof(Index));


        }
    }
}
