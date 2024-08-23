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

            string link = Url.Action(nameof(ResetPassword), "Account", new { userId = existUser.Id, token }, Request.Scheme, Request.Host.ToString());

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("maryamfa@code.edu.az"));
            email.To.Add(MailboxAddress.Parse(existUser.Email));
            email.Subject = "Reset Password";
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = $@"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Reset Password</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #f5f5f5;
            margin: 0;
            padding: 0;
        }}
        .container {{
            max-width: 600px;
            margin: 40px auto;
            padding: 20px;
            background-color: #ffffff;
            border-radius: 8px;
            box-shadow: 0 2px 6px rgba(0, 0, 0, 0.1);
        }}
        .header {{
            background-color: #1db954;
            color: black;
            text-align: center;
            padding: 30px 20px;
            border-top-left-radius: 8px;
            border-top-right-radius: 8px;
        }}
        .header img {{
            max-width: 120px;
            margin-bottom: 20px;
        }}
        .header h2 {{
            margin: 0;
            font-size: 26px;
        }}
        .header p {{
            font-size: 18px;
            margin: 10px 0 0;
        }}
        .content {{
            padding: 20px;
            text-align: center;
        }}
        .content p {{
            font-size: 16px;
            margin: 10px 0;
        }}
        .button {{
            display: inline-block;
            padding: 15px 30px;
            background-color: #1db954;
            color: white;
            text-decoration: none;
            border-radius: 5px;
            font-size: 18px;
            font-weight: bold;
            transition: background-color 0.3s ease;
        }}
        .button:hover {{
            background-color: #17a34a;
        }}
        .footer {{
            margin-top: 20px;
            color: black;
            text-align: center;
            font-size: 14px;
        }}
        .footer p {{
            margin: 5px 0;
        }}
        .footer a {{
            color: #1db954;
            text-decoration: none;
        }}
        .footer a:hover {{
            text-decoration: underline;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <img src='https://upload.wikimedia.org/wikipedia/commons/6/64/Spotify_logo_green.png' alt='Spotify Logo'/>
            <h2>Reset Your Password</h2>
            <p>Click the button below to reset your password:</p>
        </div>
        <div class='content'>
            <p>Dear {existUser.Fullname},</p>
            <p>To reset your password, please click the button below:</p>
            <p>
                <a href='{link}' class='button'>Reset Password</a>
            </p>
            <p>If you did not request a password reset, please ignore this email.</p>
        </div>
        <div class='footer'>
            <p>Best regards,<br/>The Spotify Team</p>
            <p>© {DateTime.Now.Year} Spotify. All rights reserved.</p>
            <p>If you have any questions, please contact <a href='mailto:support@spotify.com'>support@spotify.com</a>.</p>
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
                ModelState.AddModelError("", "This password already exists!");
                return View(resetPassword);
            }

            await _userManager.ResetPasswordAsync(existUser, resetPassword.Token, resetPassword.Password);

            return RedirectToAction("Login", "Account");
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
