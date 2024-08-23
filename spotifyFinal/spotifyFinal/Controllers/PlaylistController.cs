using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Service.Helpers.Extentions;
using Service.ViewModels;

namespace spotifyFinal.Controllers
{
    public class PlaylistController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _env;


        public PlaylistController(AppDbContext context, UserManager<AppUser> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            ViewBag.Songs = new SelectList(await _context.Songs.Where(m => !m.SoftDelete).Include(m => m.Album).ToListAsync(), "Id", "Name");

            return View();
        }
        public async Task<IActionResult> Detail(int id)
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            Playlist playlist = await _context.Playlist.Include(m => m.MusicPlaylists).ThenInclude(m => m.Song).ThenInclude(m => m.ArtistSongs).ThenInclude(m => m.Artist).Where(m => m.Id == id && !m.SoftDelete && m.AppUserId == user.Id).FirstOrDefaultAsync();

            return View(playlist);
        }
        public async Task<IActionResult> Search(string searchText)
        {
            if (!string.IsNullOrEmpty(searchText) && !string.IsNullOrWhiteSpace(searchText))
            {
                SearchVM model = new()
                {
                    ArtistSongs = await _context.ArtistSongs.Include(m => m.Artist).Include(m => m.Song).ThenInclude(m => m.Album).Include(m => m.Song.MusicPlaylists).Where(m => m.Song.Name.ToLower().Contains(searchText.ToLower()) && !m.SoftDelete).ToListAsync(),
                };

                return PartialView("_SearchPlaylist", model);
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AddPlaylist(AddPlaylistVM request)
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user == null) return NotFound();

            string? playlistName = "";
            string? imgUrl = "";

            if (request.Name != null)
            {
                playlistName = request.Name;
            }
            else
            {
                playlistName = "My Playlist";
            }

            if (request.Photo != null)
            {
                imgUrl = request.Photo?.SaveImage(_env, "assets/images", request.Photo.FileName);
            }

            Playlist newPlaylist = new() { Name = playlistName, ImgUrl = imgUrl, AppUserId = user.Id };

            await _context.Playlist.AddAsync(newPlaylist);
            await _context.SaveChangesAsync();

            if (request.Ids != null)
            {
                foreach (var songId in request.Ids)
                {
                    await _context.MusicPlaylists.AddAsync(new() { PlaylistId = newPlaylist.Id, SongId = songId, CreateDate = DateTime.Now });
                }
                await _context.SaveChangesAsync();
            }

            IEnumerable<MusicPlaylist> musicPlaylists = await _context.MusicPlaylists
                .Include(m => m.Song)
                .ThenInclude(m => m.ArtistSongs)
                .ThenInclude(m => m.Artist)
                .Include(m => m.Song.Album)
                .Where(m => !m.SoftDelete && m.PlaylistId == newPlaylist.Id && m.Playlist.AppUserId == user.Id)
                .ToListAsync();

            return RedirectToAction("Index", "Playlist");
        }
        public async Task<IActionResult> Delete(int id)
        {
            var playlist = await _context.Playlist.FindAsync(id);
            if (playlist == null)
            {
                return NotFound();
            }

            _context.Playlist.Remove(playlist);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
