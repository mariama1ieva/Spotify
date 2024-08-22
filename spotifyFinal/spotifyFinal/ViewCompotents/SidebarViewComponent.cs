using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Service.ViewModels;
using System.Security.Claims;

namespace spotifyFinal.ViewCompotents
{
    public class SidebarViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _ctx;

        public SidebarViewComponent(AppDbContext context, IHttpContextAccessor ctx)
        {
            _context = context;
            _ctx = ctx;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string userId = _ctx.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            int songCount = await _context.WishlistItems.Where(m => m.Wishlist.AppUserId == userId && !m.SoftDelete).CountAsync();

            var playlist = await _context.Playlist.Include(m => m.AppUser).Where(m => m.AppUserId == userId).ToListAsync();

            SidebarVM model = new() { SongCount = songCount, Playlists = playlist };

            return await Task.FromResult(View(model));
        }
    }
}
