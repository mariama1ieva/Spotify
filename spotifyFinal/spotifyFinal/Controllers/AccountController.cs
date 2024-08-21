using Domain.Entities;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using Service.Helpers.Enum;
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
