using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;

namespace spotifyFinal.Controllers
{
    public class CategoryController : Controller
    {

        private readonly IAlbumService _albumService;
        private readonly ICategoryService _categoryService;

        public CategoryController(IAlbumService albumService, ICategoryService categoryService)
        {
            _albumService = albumService;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Detail(int id)
        {
            var reuqest = await _categoryService.GetCategoryWithAlbums(id);

            return View(reuqest);
        }
    }
}
