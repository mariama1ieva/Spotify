using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Service.Services.Interfaces;
using Service.ViewModels.AccountVMs;

namespace spotifyFinal.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        public UserController(IAccountService accountService,
                              UserManager<AppUser> userManager,
                              RoleManager<IdentityRole> roleManager,
                              SignInManager<AppUser> signInManager)
        {
            _accountService = accountService;
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<UserVM> userRoles = new();
            var users = await _accountService.GetAll();
            var roles = await _roleManager.Roles.ToListAsync();

            foreach (var user in users)
            {
                var userRolesList = await _accountService.GetRoles(user);
                string rolesStr = string.Join(",", userRolesList);

                userRoles.Add(new UserVM
                {
                    Email = user.Email,
                    FullName = user.Fullname,
                    UserName = user.UserName,
                    Roles = rolesStr,
                    UserId = user.Id,
                });
            }

            ViewBag.Roles = roles.Select(role => new SelectListItem
            {
                Value = role.Name,
                Text = role.Name
            }).ToList();

            return View(userRoles);
        }



        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> AddRole()
        {
            ViewBag.users = new SelectList(_userManager.Users.ToList(), "Id", "Fullname");
            ViewBag.roles = new SelectList(_roleManager.Roles.ToList(), "Id", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRole(AddRoleVM request)
        {
            var user = _userManager.FindByIdAsync(request.UserId).Result;
            var role = _roleManager.FindByIdAsync(request.RoleId).Result;

            await _userManager.AddToRoleAsync(user, role.ToString());
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRole(string userName, string roleToDelete)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("SuperAdmin"))
            {
                return Unauthorized();
            }

            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles.Count == 1 && userRoles.Contains(roleToDelete))
            {
                return Json(new { success = false, message = "Cannot delete the only role assigned to this user." });
            }

            var result = await _userManager.RemoveFromRoleAsync(user, roleToDelete);

            if (!result.Succeeded)
            {
                return Json(new { success = false, message = "Failed to delete role." });
            }

            return Json(new { success = true });
        }







    }
}
