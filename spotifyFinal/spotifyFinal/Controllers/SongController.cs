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
        [HttpPost]
        public async Task<IActionResult> AddComment(Comment comment, int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (User.Identity.IsAuthenticated)
            {
                Song? song = _context.Songs.Include(s => s.Comments).FirstOrDefault(c => c.Id == id);
                if (song == null) return NotFound();
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

                Rating ratingRegistered = new()
                {
                    Point = comment.Rating.Point
                };

                Comment commentRegistered = new()
                {
                    Message = comment.Message,
                    AppUser = user,
                    Song = song,
                    Rating = ratingRegistered,
                    CreateDate = DateTime.Now
                };
                double rateCount = _context.Ratings.Where(r => r.Comment.SongId == id).Count();


                if (song.PointRayting.HasValue)
                {
                    double currentTotalRating = song.PointRayting.Value * rateCount;
                    double newTotalRating = currentTotalRating + comment.Rating.Point;
                    rateCount++;
                    double averageRating = newTotalRating / rateCount;
                    song.PointRayting = Math.Min(10.0, averageRating);
                }
                else
                {
                    song.PointRayting = Math.Min(10.0, comment.Rating.Point);
                }

                _context.Comments.Add(commentRegistered);
                _context.Songs.Update(song);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Detail), new { id });
        }

        public async Task<IActionResult> AddWishlist(int? id)
        {
            if (id == null) return BadRequest();

            Song song = await _context.Songs.FirstOrDefaultAsync(m => m.Id == id && !m.SoftDelete);

            if (song == null) return NotFound();

            AppUser? user = await _userManager.FindByNameAsync(User.Identity?.Name);

            if (user == null) return RedirectToAction("Login", "Account");

            IEnumerable<Wishlist> dbWishlists = await _context.Wishlists.Include(m => m.WishlistItems).Where(m => m.AppUserId == user.Id && !m.SoftDelete).ToListAsync();

            int songCount = 0;

            if (dbWishlists != null)
            {
                foreach (var dbWishlist in dbWishlists)
                {
                    foreach (var dbWishlistItem in dbWishlist.WishlistItems)
                    {
                        if (dbWishlistItem.SongId == id)
                        {
                            _context.Wishlists.Remove(dbWishlist);
                            await _context.SaveChangesAsync();

                            songCount = await _context.WishlistItems.Where(m => m.Wishlist.AppUserId == user.Id && !m.SoftDelete).CountAsync();
                            return Json(new { id = id, songCount = songCount, isDeleted = true });
                        }
                    }
                }
            }

            Wishlist newWishlist = new() { AppUserId = user.Id };

            await _context.Wishlists.AddAsync(newWishlist);
            await _context.SaveChangesAsync();

            WishlistItem newWishlistItem = new() { WishlistId = newWishlist.Id, SongId = (int)id, CreateDate = DateTime.Now };

            await _context.WishlistItems.AddAsync(newWishlistItem);
            await _context.SaveChangesAsync();

            songCount = await _context.WishlistItems.Where(m => m.Wishlist.AppUserId == user.Id && !m.SoftDelete).CountAsync();
            return Json(new { id = id, songCount = songCount, isDeleted = false });
        }

        private async Task<IEnumerable<Song>> GetOtherSongsInCategory(int songId)
        {
            var song = await _context.Songs
                .Include(s => s.Category)
                .FirstOrDefaultAsync(s => s.Id == songId);

            if (song == null || song.Category == null)
            {
                return null;
            }

            var otherSongs = await _context.Songs
                .Where(s => s.Id != songId && s.CategoryId == song.CategoryId)
                .ToListAsync();

            return otherSongs;
        }
    }
}
