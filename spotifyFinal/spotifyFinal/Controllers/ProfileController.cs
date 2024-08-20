using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Service.Helpers.Extentions;
using Service.ViewModels;

namespace spotifyFinal.Controllers
{
    public class ProfileController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public ProfileController(AppDbContext context, UserManager<AppUser> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }
        public async Task<IActionResult> Create()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            List<Playlist> playlists = await _context.Playlist
                .Where(p => p.AppUserId == user.Id)
                .ToListAsync();

            var profileVM = new ProfileVM
            {
                UserName = user.UserName,
                WalletAmount = user.Wallet,
                Playlists = playlists,
                ProfilePhotoUrl = user.ImageUrl
            };

            return View(profileVM);
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProfileVM profileVM)
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            List<Playlist> playlists = await _context.Playlist
                .Where(p => p.AppUserId == user.Id)
                .ToListAsync();

            var model = new ProfileVM
            {
                UserName = user.UserName,
                WalletAmount = user.Wallet,
                Playlists = playlists,
                ProfilePhotoUrl = user.ImageUrl
            };

            if (profileVM.ProfileImg == null)
            {
                ModelState.AddModelError("ProfileImg", "Please select an image");
                return View(model);
            }

            if (!profileVM.ProfileImg.IsImage())
            {
                ModelState.AddModelError("ProfileImg", "Please select a valid image file");
                return View(model);
            }

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + profileVM.ProfileImg.FileName;
            string filePath = Path.Combine(_env.WebRootPath, "assets/images", uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await profileVM.ProfileImg.CopyToAsync(fileStream);
            }
            user.ImageUrl = uniqueFileName;
            await _userManager.UpdateAsync(user);
            return View(model);
        }
    }

}
