using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;
using Service.ViewModels.CategoryVMs;
using spotifyFinal.Helpers.Extentions;

namespace spotifyFinal.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _env;
        public CategoryController(ICategoryService categoryService, IWebHostEnvironment env)
        {
            _categoryService = categoryService;
            _env = env;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {

            return View(await _categoryService.GetAllAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {

            return View(await _categoryService.GetByIdAsync(id));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateVM request)
        {
            if (!ModelState.IsValid) return View(request);

            if (await _categoryService.AnyAsync(request.Name))
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

            request.ImageUrl = fileName;

            await _categoryService.CreateAsync(request);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (!ModelState.IsValid) return View();

            if (id == null) return BadRequest();
            var setting = await _categoryService.GetByIdAsync((int)id);

            if (setting == null) return NotFound();

            await _categoryService.DeleteAsync((int)id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (!ModelState.IsValid) return View();

            if (id == null) return BadRequest();

            var category = await _categoryService.GetByIdAsync((int)id);

            CategoryEditVM model = new()
            {
                ImageUrl = category.ImageUrl,
                Name = category.Name,
                Color = category.Color
            };

            if (category == null) return NotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, CategoryEditVM request)
        {
            if (!ModelState.IsValid) return View(request);

            if (id == null) return BadRequest();

            if (await _categoryService.AnyAsync(request.Name))
            {
                ModelState.AddModelError("Name", $"{request.Name} is already exist!");
                return View(request);
            }

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
            }
            FileExtention.DeleteFileFromLocalAsync(Path.Combine(_env.WebRootPath, "img"), request.ImageUrl);

            await _categoryService.UpdateAsync((int)id, request);

            return RedirectToAction(nameof(Index));
        }
    }
}
