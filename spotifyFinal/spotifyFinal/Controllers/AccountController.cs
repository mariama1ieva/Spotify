using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repository.Data;
using Service.Helpers.Enum;
using Service.Services.Interfaces;
using Service.ViewModels.Account;
using Service.ViewModels.AccountVMs;

namespace spotifyFinal.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;
        private readonly IFileService _fileService;

        public AccountController(AppDbContext context, IEmailService emailService, IFileService fileService, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _fileService = fileService;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterVM register)
        {
            if (!ModelState.IsValid) return View();

            string otp = GenerateOTP();

            AppUser user = new()
            {
                Email = register.Email,
                OTP = otp,
                Fullname = register.Fullname,
                UserName = register.UserName,
                IsActive = true
            };

            IdentityResult result = await _userManager.CreateAsync(user, register.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(register);
            }

            await _userManager.AddToRoleAsync(user, RoleEnums.SuperAdmin.ToString());

            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            string body = string.Empty;

            string path = "wwwroot/verifymail/verify.html";
            body = _fileService.ReadFile(path, body);

            body = body.Replace("{{otp}}", otp);
            body = body.Replace("{{Fullname}}", user.Fullname);


            string subject = "Verify Email";
            _emailService.Send(user.Email, subject, body);

            return RedirectToAction(nameof(VerifyEmail), new { email = user.Email });
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmEmail(ConfirmAccountVM confirmAccount)
        {
            if (string.IsNullOrEmpty(confirmAccount.OTP) || string.IsNullOrEmpty(confirmAccount.OTP))
                return RedirectToAction(nameof(VerifyEmail));

            AppUser user = await _userManager.FindByEmailAsync(confirmAccount.Email);
            if (user == null) return NotFound();

            if (user.OTP != confirmAccount.OTP)
                return RedirectToAction(nameof(VerifyEmail), new { email = confirmAccount.Email });

            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            await _userManager.ConfirmEmailAsync(user, token);

            await _signInManager.SignInAsync(user, false);

            return RedirectToAction(nameof(Login));
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null) return NotFound();

            AppUser user = await _userManager.FindByIdAsync(userId);

            if (user == null) return NotFound();

            await _userManager.ConfirmEmailAsync(user, token);

            await _signInManager.SignInAsync(user, false);

            return RedirectToAction(nameof(Login));
        }

        public async Task<IActionResult> VerifyEmail(string email)
        {
            ConfirmAccountVM confirmEmail = new()
            {
                Email = email
            };

            return View(confirmEmail);
        }
        public IActionResult VerifyAccount()
        {
            return View();
        }
        //public async Task<IActionResult> ResetPassword(string userId, string token)
        //{
        //    ResetPasswordVM resetPassword = new ResetPasswordVM()
        //    {
        //        UserId = userId,
        //        Token = token,
        //    };
        //    return View(resetPassword);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPassword)
        //{
        //    if (!ModelState.IsValid) return View();

        //    AppUser exsistUser = await _userManager.FindByIdAsync(resetPassword.UserId);

        //    bool chekPassword = await _userManager.VerifyUserTokenAsync(exsistUser, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", resetPassword.Token);

        //    if (!chekPassword) return Content("Error");

        //    if (exsistUser == null) return NotFound();

        //    if (await _userManager.CheckPasswordAsync(exsistUser, resetPassword.Password))
        //    {
        //        ModelState.AddModelError("", "This password is your last password");
        //        return View(resetPassword);
        //    }

        //    await _userManager.ResetPasswordAsync(exsistUser, resetPassword.Token, resetPassword.Password);

        //    await _userManager.UpdateSecurityStampAsync(exsistUser);

        //    return RedirectToAction(nameof(Login));
        //}

        //    public IActionResult ForgetPassword()
        //    {
        //        return View();
        //    }
        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public async Task<IActionResult> ForgetPassword(ForgotPasswordVM forgotPassword)
        //    {
        //        if (!ModelState.IsValid) return NotFound();

        //        AppUser exsistUser = await _userManager.FindByEmailAsync(forgotPassword.Email);

        //        if (exsistUser == null)
        //        {
        //            ModelState.AddModelError("Email", "Email isn't found");
        //            return View();
        //        }

        //        string token = await _userManager.GeneratePasswordResetTokenAsync(exsistUser);

        //        string link = Url.Action(nameof(ResetPassword), "Account", new { userId = exsistUser.Id, token },
        //Request.Scheme, Request.Host.ToString());

        //        string body = string.Empty;

        //        string path = "wwwroot/verifymail/forgotVerify.html";
        //        body = _fileService.ReadFile(path, body);

        //        body = body.Replace("{{link}}", link);
        //        body = body.Replace("{{Fullname}}", exsistUser.Fullname);

        //        string subject = "Verify Email";

        //        _emailService.Send(exsistUser.Email, subject, body);

        //        return RedirectToAction(nameof(VerifyAccount));
        //    }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }

            AppUser user = await _userManager.FindByEmailAsync(loginVM.UserName);

            user ??= await _userManager.FindByNameAsync(loginVM.UserName);

            if (user == null)
            {
                ModelState.AddModelError("", "Email or password is wrong!");
                return View(loginVM);
            }

            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe, false);
            //if (!result.IsLockedOut)
            //{
            //    ModelState.AddModelError("", "Your account has been blocked!");
            //    return View(loginVM);
            //}

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Email or password wrong!");
                return View(loginVM);
            }
            if (!user.IsActive)
            {
                ModelState.AddModelError("", "Your account has been blocked");
                return View(loginVM);
            }

            return RedirectToAction("Index", "Home");
        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> CreateRole()
        {
            await _roleManager.CreateAsync(new IdentityRole { Name = "SuperAdmin" });
            //await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
            //await _roleManager.CreateAsync(new IdentityRole { Name = "Member" });
            return Content("role added");
        }
        private static string GenerateOTP()
        {
            Random random = new Random();
            int otpNumber = random.Next(100000, 999999);
            return otpNumber.ToString();
        }

    }
}
