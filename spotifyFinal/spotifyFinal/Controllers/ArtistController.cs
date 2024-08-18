using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Service.ViewModels;

namespace spotifyFinal.Controllers
{
    public class ArtistController : Controller
    {
        private readonly AppDbContext _context;
        public ArtistController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Detail(int id)
        {
            if (id == null) return NotFound();

            SearchArtistDetailVM artistDetailVM = new()
            {
                Artists = await _context.Artists
                .Include(s => s.ArtistSongs)
                .ThenInclude(s => s.Song)
                .OrderByDescending(m => m.Id)
                .ToListAsync(),

                Artist = await _context.Artists
                .Include(a => a.ArtistSongs)
                .ThenInclude(a => a.Song)
                .Where(a => a.Id == id && !a.SoftDelete)
               .FirstOrDefaultAsync(),

                Albums = await _context.Albums
                .Where(a => a.ArtistId == id && !a.SoftDelete)
                .ToListAsync(),

                SamePosition = await _context.ArtistPositions
                 .Include(ap => ap.Artist)
                  .Include(ap => ap.Position)
                   .Where(ap => ap.Position.Name == _context.ArtistPositions
                        .Where(innerAp => innerAp.ArtistId == id)
                        .Select(innerAp => innerAp.Position.Name)
                        .FirstOrDefault()
                        && ap.ArtistId != id)
                        .Select(ap => ap.Artist)
                         .ToListAsync()

            };

            if (artistDetailVM.Artist != null)
            {
                if (artistDetailVM.Artist != null)
                {
                    var artistAlbumCategories = await _context.Albums
                        .Where(a => a.ArtistId == id && !a.SoftDelete)
                        .Select(a => a.CategoryId)
                        .ToListAsync();

                }
            }

            //ViewBag.Ratings = _context.Ratings.Include(c => c.Comment).ToList();

            return View(artistDetailVM);
        }



    }
}
