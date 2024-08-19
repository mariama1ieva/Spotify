using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Service.Services.Interfaces;

namespace spotifyFinal.Controllers
{
    public class AlbumController : Controller
    {
        private readonly IAlbumService _albumService;
        private readonly ICategoryService _categoryService;
        private readonly AppDbContext _context;

        public AlbumController(IAlbumService albumService, ICategoryService categoryService, AppDbContext context)

        {
            _albumService = albumService;
            _categoryService = categoryService;
            _context = context;
        }

        public async Task<IActionResult> Detail(int id)
        {
            ViewBag.TotalSongCount = await _context.Songs.CountAsync(s => s.AlbumId == id);

            IQueryable<Album> albums = _context.Albums.AsNoTracking().AsQueryable();
            Album? album = await albums
               .Include(a => a.Songs.Take(5))
               .ThenInclude(a => a.ArtistSongs)
               .Include(a => a.Artist)
               .FirstOrDefaultAsync(a => a.Id == id);
            if (album == null) return NotFound();
            ViewBag.OtherAlbums = await _context.Albums
    .Where(a => a.ArtistId == album.ArtistId && a.Id != id)
    .ToListAsync();

            return View(album);
        }
        public async Task<IActionResult> LoadMore(int albumId, int skip)
        {
            IQueryable<Album> albums = _context.Albums.AsNoTracking().AsQueryable();
            Album? album = await albums
               .Include(a => a.Songs.Skip(skip).Take(5))
               .ThenInclude(a => a.ArtistSongs)
               .Include(a => a.Artist)
               .FirstOrDefaultAsync(a => a.Id == albumId);

            return PartialView("_AlbumSongListPartial", album);
        }
    }
}
