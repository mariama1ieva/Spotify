using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;

namespace spotifyFinal.Controllers
{
    public class SearchController : Controller
    {
        private readonly ICategoryService _categoryService;

        public SearchController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {

            return View(await _categoryService.GetAllAsync());
        }

    }
}
