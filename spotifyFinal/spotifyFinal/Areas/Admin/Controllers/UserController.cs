using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public UserController(IAccountService accountService,
                              UserManager<AppUser> userManager,
                              RoleManager<IdentityRole> roleManager)
        {
            _accountService = accountService;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<UserVM> userRoles = new();
            var users = await _accountService.GetAll();

            foreach (var user in users)
            {
                var roles = await _accountService.GetRoles(user);
                string rolesStr = string.Join(",", roles);

                userRoles.Add(new UserVM
                {
                    Email = user.Email,
                    FullName = user.Fullname,
                    UserName = user.UserName,
                    Roles = rolesStr
                });
            }
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
    }
}
