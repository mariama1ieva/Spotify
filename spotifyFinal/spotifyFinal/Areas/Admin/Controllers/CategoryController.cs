using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;

namespace spotifyFinal.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
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
    }
}
