using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Service.Services.Interfaces;
using Service.ViewModels;

namespace spotifyFinal.Controllers
{
    public class SearchController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IAlbumService _albumService;
        private readonly ISongService _songService;
        private readonly IGroupArtistService _groupArtistService;
        private readonly IArtistService _artistService;
        private readonly AppDbContext _context;




        public SearchController(ICategoryService categoryService, IAlbumService albumService,
            ISongService songService, IGroupArtistService groupArtistService,
            IArtistService artistService, AppDbContext context)

        {
            _categoryService = categoryService;
            _albumService = albumService;
            _songService = songService;
            _groupArtistService = groupArtistService;
            _artistService = artistService;
            _context = context;

        }
        public async Task<IActionResult> Index()
        {

            return View(await _categoryService.GetAllAsync());
        }
        public async Task<IActionResult> Detail(int id, string request)
        {
            if (id == null && request == null) return BadRequest();

            SearchCategoryDetailVM model = new()
            {
                Category = await _context.Categories.Where(c => c.Id == id || c.Name == request).FirstOrDefaultAsync(),
                Artist = await _context.Artists.FirstOrDefaultAsync(),
                Albums = await _context.Albums.Include(m => m.Category).Include(a => a.Artist)
                .Where(a => a.CategoryId == id || a.Category.Name == request && !a.SoftDelete).ToListAsync()
            };

            return View(model);
        }

    }
}
