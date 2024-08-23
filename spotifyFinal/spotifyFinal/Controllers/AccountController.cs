using Domain.Entities;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using Service.Helpers.Enum;
using Service.ViewModels;
using Service.ViewModels.Account;
using Service.ViewModels.AccountVMs;

namespace spotifyFinal.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<AppUser> userManager,
                                 SignInManager<AppUser> signInManager,
                                 RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM request)
        {
            if (!ModelState.IsValid) { return View(); }

            AppUser user = new()
            {
                UserName = request.UserName,
                Email = request.Email,
                Fullname = request.Fullname
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
                return View(request);
            }

            await _userManager.AddToRoleAsync(user, RoleEnums.Member.ToString());

            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            string url = Url.Action(nameof(ConfirmEmail), "Account", new { userId = user.Id, token }, Request.Scheme, Request.Host.ToString());

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("maryamfa@code.edu.az"));
            email.To.Add(MailboxAddress.Parse(user.Email));
            email.Subject = "Register confirmation";
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = $@" 
      <!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <link rel=""stylesheet"" href=""https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.1/css/all.min.css""
    integrity=""sha512-DTOQO9RWCH3ppGqcWaEA1BIZOC6xxalwEsw9c2QQeAIftl+Vegovlnee1c9QX4TctnWMn13TZye+giMm8e2LwA==""
    crossorigin=""anonymous"" referrerpolicy=""no-referrer"" />
    <title>Confirm Your Email</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            color: #333;
            margin: 0;
            padding: 0;
            background-color: #f9f9f9;
        }}
        .email-container {{
            width: 100%;
            max-width: 600px;
            margin: 0 auto;
            padding: 20px;
            background-color: #fff;
            border: 1px solid #ddd;
            border-radius: 8px;
        }}
        .header {{
            text-align: center;
            padding-bottom: 20px;
        }}
        .header img {{
            max-width: 150px;
        }}
        .content {{
            text-align: center;
        }}
        h4 {{
            color: #1db954; /* Spotify green */
        }}
        .button-container {{
            margin-top: 20px;
        }}
        .button {{
            display: inline-block;
            padding: 15px 25px;
            font-size: 16px;
            color: #fff;
            background-color: #1db954; /* Spotify green */
            border: none;
            border-radius: 4px;
            text-decoration: none;
        }}
        .button:hover {{
            background-color: #1ed760;
        }}
    </style>
</head>
<body>
    <div class=""email-container"">
        <header class=""header"">
            <img src=""https://upload.wikimedia.org/wikipedia/commons/6/6b/Spotify_logo_2015.png"" alt=""Spotify Logo"">
        </header>
        <div class=""content"">
            <h4>Confirm Your Email Address</h4>
            <p>Hi there!</p>
            <p>Thank you for registering with Spotify. Please confirm your email address by clicking the button below:</p>
            <div class=""button-container"">
                <a href=""{url}"" class=""button"">Confirm Email</a>
            </div>
            <p>If you didn't sign up for this account, you can ignore this email.</p>
            <p>Cheers,<br>The Spotify Team</p>
        </div>
    </div>
</body>
</html>

"
            };



            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("maryamfa@code.edu.az", "fgwj fxfg qpul folr");
            smtp.Send(email);
            smtp.Disconnect(true);

            return RedirectToAction(nameof(VerifyEmail));
        }


        [HttpGet]
        public IActionResult VerifyEmail(string email)
        {


            ConfirmAccountVM confirmEmail = new()
            {
                Email = email
            };

            return View(confirmEmail);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            await _userManager.ConfirmEmailAsync(user, token);
            return RedirectToAction(nameof(Login));
        }


        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgetPassword(ForgotPasswordVM request)
        {
            if (!ModelState.IsValid) return View(request);

            AppUser existUser = await _userManager.FindByEmailAsync(request.Email);

            if (existUser is null)
            {
                ModelState.AddModelError("Email", "User not found!");
                return View(request);
            }

            string token = await _userManager.GeneratePasswordResetTokenAsync(existUser);

            string url = Url.Action(nameof(ConfirmEmail), "Account", new { userId = existUser.Id, token }, Request.Scheme, Request.Host.ToString());

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("maryamfa@code.edu.az"));
            email.To.Add(MailboxAddress.Parse(existUser.Email));
            email.Subject = "Reset password";
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Reset Password</title>
</head>
<body style=""margin: 0; padding: 0; background-color: #121212; color: #ffffff; font-family: 'Helvetica Neue', Helvetica, Arial, sans-serif;"">
    <header style=""background-color: #000000; padding: 20px; text-align: center;"">
        <div style=""display: inline-block;"">
            <img src=""https://example.com/assets/images/Spotify_Logo.png"" alt=""logo"" style=""max-width: 150px; height: auto;"">
        </div>
    </header>
    <section style=""display: flex; justify-content: center; align-items: center; height: calc(100vh - 70px); background-color: #121212; padding: 20px;"">
        <div style=""background-color: #282828; padding: 20px; border-radius: 8px; box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2); width: 90%; max-width: 400px;"">
            <h4 style=""font-size: 24px; margin-bottom: 20px; text-align: center; color: #ffffff;"">Reset password</h4>
            <div style=""display: flex; flex-direction: column;"">
                <form >
                    <div style=""margin-bottom: 15px;"">
                        <label for=""password"" style=""font-size: 14px; margin-bottom: 5px; color: #b3b3b3;"">New Password</label>
                        <input id=""password"" type=""text"" placeholder=""new password"" style=""width: 100%; padding: 10px; font-size: 16px; border: 1px solid #333333; border-radius: 4px; background-color: #000000; color: #ffffff;"">
                    </div>
                    <div style=""margin-bottom: 15px;"">
                        <label for=""password2"" style=""font-size: 14px; margin-bottom: 5px; color: #b3b3b3;""></label>
                        <input id=""password2"" type=""text"" placeholder=""confirm password"" style=""width: 100%; padding: 10px; font-size: 16px; border: 1px solid #333333; border-radius: 4px; background-color: #000000; color: #ffffff;"">
                    </div>
                    <a href=""{url}"" style=""display: inline-block; background-color: #1db954; color: #ffffff; text-decoration: none; padding: 10px 15px; border-radius: 4px; font-size: 16px; text-align: center; font-weight: bold; transition: background-color 0.3s;"">Confirm</a>
                </form>
            </div>
        </div>
    </section>
</body>
</html>"
            };



            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("maryamfa@code.edu.az", "fgwj fxfg qpul folr");
            smtp.Send(email);
            smtp.Disconnect(true);

            return RedirectToAction(nameof(CheckEmail));
        }
        [HttpGet]

        public IActionResult ResetPassword(string userId, string token)
        {
            return View(new ResetPasswordVM { Token = token, UserId = userId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPassword)
        {
            if (!ModelState.IsValid) return View(resetPassword);

            AppUser existUser = await _userManager.FindByIdAsync(resetPassword.UserId);

            if (existUser == null) return NotFound();

            if (await _userManager.CheckPasswordAsync(existUser, resetPassword.Password))
            {
                ModelState.AddModelError("", "This password already exist!");
                return View(resetPassword);
            }

            await _userManager.ResetPasswordAsync(existUser, resetPassword.Token, resetPassword.Password);

            return RedirectToAction(nameof(SignIn));
        }
        public IActionResult CheckEmail()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM login)
        {
            if (!ModelState.IsValid) return View();

            AppUser user = await _userManager.FindByEmailAsync(login.UserName);
            if (user is null)
            {
                user = await _userManager.FindByNameAsync(login.UserName);
                if (user is null)
                {
                    ModelState.AddModelError(string.Empty, "Email, UserName or Password is incorrect");
                    return View();
                }
            }
            var result = await _signInManager.PasswordSignInAsync(user, login.Password, login.RememberMe, true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Server is eneabled at the moment,please try again later");
                return View();
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Email, UserName or Password is incorrect");
                return View();
            }


            await _signInManager.SignInAsync(user, login.RememberMe);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> CreateRoles()
        {
            foreach (RoleEnums role in Enum.GetValues(typeof(RoleEnums)))
            {
                if (!await _roleManager.RoleExistsAsync(role.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole
                    {
                        Name = role.ToString(),
                    });
                }
            }
            return RedirectToAction("Index", "Home");
        }



    }

}
