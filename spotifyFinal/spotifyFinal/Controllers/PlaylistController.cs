
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Service.ViewModels;
using spotifyFinal.Helpers.Extentions;
using System.Security.Claims;

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
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var userName = User.Identity.Name;
            if (userName == null)
            {
                return BadRequest("User is not authenticated.");
            }

            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            ViewBag.Songs = new SelectList(await _context.Songs
                .Where(m => !m.SoftDelete)
                .Include(m => m.Album)
                .ToListAsync(), "Id", "Name");

            var playlist = await _context.Playlist
                .Include(m => m.MusicPlaylists)
                .ThenInclude(m => m.Song)
                .ThenInclude(m => m.ArtistSongs)
                .ThenInclude(m => m.Artist)
                .Where(m => m.Id == id && !m.SoftDelete)
                .FirstOrDefaultAsync();

            if (playlist == null)
            {
                return NotFound("Playlist not found.");
            }

            return View(playlist);
        }
        public async Task<IActionResult> ViewPlaylists(int id)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            var playlists = await _context.Playlist
                .Include(p => p.AppUser)
                .Where(p => !p.SoftDelete && p.AppUserId != currentUserId)
                .ToListAsync();

            var viewModel = new SearchVM
            {
                MusicPlaylists = (ICollection<MusicPlaylist>)playlists,
                CurrentUserId = currentUserId
            };

            return View(viewModel);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveSongFromPlaylist([FromBody] RemoveSongRequest request)
        {
            if (ModelState.IsValid)
            {
                var playlist = await _context.Playlist
                    .Include(p => p.MusicPlaylists)
                    .FirstOrDefaultAsync(p => p.Id == request.PlaylistId);

                if (playlist == null)
                {
                    return Json(new { success = false, message = "Playlist not found." });
                }

                var musicPlaylist = playlist.MusicPlaylists
                    .FirstOrDefault(mp => mp.SongId == request.SongId);

                if (musicPlaylist == null)
                {
                    return Json(new { success = false, message = "Song not found in the playlist." });
                }

                _context.MusicPlaylists.Remove(musicPlaylist);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Invalid data." });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToPlaylist([FromBody] AddToPlaylistRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Invalid request." });
            }

            AppUser user = await _userManager.GetUserAsync(User);

            if (user == null) return Unauthorized();

            var playlist = await _context.Playlist
                .Include(m => m.MusicPlaylists)
                .FirstOrDefaultAsync(m => m.Id == request.PlaylistId && m.AppUserId == user.Id);

            if (playlist == null)
            {
                return NotFound(new { success = false, message = "Playlist not found." });
            }

            var existingItem = await _context.MusicPlaylists
                .FirstOrDefaultAsync(m => m.SongId == request.SongId && m.PlaylistId == playlist.Id);

            if (existingItem == null)
            {
                var song = await _context.Songs.FindAsync(request.SongId);
                if (song == null) return NotFound(new { success = false, message = "Song not found." });

                var musicPlaylistItem = new MusicPlaylist
                {
                    SongId = request.SongId,
                    PlaylistId = playlist.Id
                };

                _context.MusicPlaylists.Add(musicPlaylistItem);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = "Song already in the playlist." });
            }
        }





    }

    public class RemoveSongRequest
    {
        public int PlaylistId { get; set; }
        public int SongId { get; set; }
    }


    public class AddToPlaylistRequest
    {
        public int SongId { get; set; }
        public int PlaylistId { get; set; }
    }


}
