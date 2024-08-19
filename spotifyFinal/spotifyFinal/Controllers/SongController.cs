using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Service.ViewModels;

namespace spotifyFinal.Controllers
{
    public class SongController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public SongController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Detail(int id)
        {
            if (id == null) return BadRequest();

            Song existSong = await _context.Songs.Include(m => m.ArtistSongs).ThenInclude(m => m.Artist).Where(m => m.Id == id && !m.SoftDelete).FirstOrDefaultAsync();

            UISongDetail songDetailVM = new()
            {
                Songs = await _context.Songs
              .Where(s => s.CategoryId == existSong.CategoryId)
              .Include(s => s.ArtistSongs)
              .ThenInclude(s => s.Artist)
              .Include(m => m.Album)
              .Include(p => p.Comments.Where(m => m.SongId == id)).ThenInclude(c => c.AppUser)
              .Include(p => p.Comments.Where(m => m.SongId == id)).ThenInclude(c => c.Rating)
              //.OrderByDescending(m => m.Id)
              .Take(5)
              .ToListAsync(),

                Playlists = await _context.Playlist.ToListAsync(),
                Song = existSong,
                Comments = await _context.Comments.Where(c => c.SongId == id).ToListAsync()
            };
            songDetailVM.Artists = songDetailVM.Song?.ArtistSongs?.Select(s => s.Artist)?.ToList();
            songDetailVM.Artist = songDetailVM.Artists?.FirstOrDefault();

            songDetailVM.Comments = await _context.Comments.ToListAsync();

            if (songDetailVM.Song == null) return NotFound();

            ViewBag.Ratings = _context.Ratings
             .Where(r => r.Comment.SongId == id)
             .Include(r => r.Comment)
             .ThenInclude(r => r.Song)
             .ToList();

            ViewBag.Song = await _context.Songs.Include(m => m.Comments).ThenInclude(m => m.AppUser).Where(m => m.Id == id && !m.SoftDelete).FirstOrDefaultAsync();

            ViewBag.CommentCount = await _context.Comments.Where(m => !m.SoftDelete).CountAsync();
            return View(songDetailVM);


        }
    }
}
