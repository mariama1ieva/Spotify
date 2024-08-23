using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Service.Services.Interfaces;
using Service.ViewModels.CategoryVMs;
using spotifyFinal.Areas.Admin.PaginateVM;


using spotifyFinal.Helpers.Extentions;

namespace spotifyFinal.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context;
        public CategoryController(ICategoryService categoryService, IWebHostEnvironment env, AppDbContext context)
        {
            _categoryService = categoryService;
            _env = env;
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 4)
        {

            var categories = await _categoryService.GetAllAsync();

            Paginate<Category> pageCategory = Paginate<Category>.Create(categories, pageNumber, pageSize);

            var paginate = new PaginateCategoryListVM
            {
                Categories = pageCategory,
                PageNumber = pageNumber,
                TotalPages = pageCategory.TotalPages
            };

            return View(paginate);

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

            if (await _categoryService.AnyAsync(request.Name.Trim().ToLower()))
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

            if (id == null) return NotFound();
            Category? category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category == null) return NotFound();
            return View(new CategoryEditVM
            {
                ImageUrl = category.ImageUrl,
                Name = category.Name,
                Color = category.Color
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, CategoryEditVM request)
        {
            if (id == null) return NotFound();
            Category? category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category == null) return NotFound();
            if (request.Photo != null)
            {
                if (request.Photo == null)
                {
                    ModelState.AddModelError("Photo", "Can not be empty!");
                    return View(request = new() { ImageUrl = category.ImageUrl });
                }
                if (!request.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "only image select");
                    return View(request = new() { ImageUrl = category.ImageUrl });
                }
                string fullPath = Path.Combine(_env.WebRootPath, "assets/images", category.ImageUrl);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                category.ImageUrl = request.Photo.SaveImage(_env, "assets/images", request.Photo.FileName);
                _context.SaveChanges();
            }
            category.Name = request.Name;
            category.Color = request.Color;


            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
