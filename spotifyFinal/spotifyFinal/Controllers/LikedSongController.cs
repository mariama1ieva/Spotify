using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Service.ViewModels;

namespace spotifyFinal.Controllers
{
    public class LikedSongController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public LikedSongController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            AppUser user = await _userManager.GetUserAsync(User);

            if (user == null) return Unauthorized();
            var wishlist = await _context.Wishlists
               .Include(m => m.WishlistItems)
               .ThenInclude(m => m.Song)
               .ThenInclude(m => m.Album)
               .ThenInclude(m => m.Artist)
               .Where(m => m.AppUserId == user.Id)
               .OrderByDescending(m => m.Id)
               .ToListAsync();

            WishListVM model = new();

            if (wishlist == null) return View(model);

            foreach (var dbWishlist in wishlist)
            {
                foreach (var dbWishlistSong in dbWishlist.WishlistItems)
                {

                    WishListSongVM wishliatSong = new()
                    {
                        Id = dbWishlistSong.Song.Id,
                        Name = dbWishlistSong.Song.Name,
                        Image = dbWishlistSong.Song.ImageUrl,
                        SongId = dbWishlistSong.SongId,
                        Path = dbWishlistSong.Song.Path,
                        ArtistName = dbWishlistSong.Song.Album.Artist.FullName,
                        AlbumName = dbWishlistSong.Song.Album.Name,
                        AlbumId = dbWishlistSong.Song.Album.Id,
                        ArtistId = dbWishlistSong.Song.Album.Artist.Id,
                        CreateDate = dbWishlistSong.CreateDate
                    };
                    model.WishlistSongs.Add(wishliatSong);
                }
            }
            ViewBag.Songs = new SelectList(await _context.Songs
     .Where(m => !m.SoftDelete)
     .Include(m => m.Album)
     .ToListAsync(), "Id", "Name");
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> RemoveFromWishlist([FromBody] RemoveSongRequest request)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Invalid data." });
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "User not found." });
            }

            var wishlistItem = _context.WishlistItems
                .FirstOrDefault(wi => wi.SongId == request.SongId);

            if (wishlistItem == null)
            {
                return Json(new { success = false, message = "Song not found in wishlist." });
            }

            _context.WishlistItems.Remove(wishlistItem);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        public class RemoveSongRequest
        {
            public int SongId { get; set; }
        }





    }

}
