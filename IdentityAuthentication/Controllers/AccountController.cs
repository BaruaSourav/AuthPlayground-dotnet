using System.Security.Claims;
using System.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NETCore.MailKit.Core;

namespace IdentityAuthentication.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailService _emailService;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] string email, string password, string uname)
        {
            var user = new IdentityUser
            {
                UserName = uname,
                Email = email
            };
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                var emailConfToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var urlToConfirm = Url.Action("ConfirmEmail","Account", new {userId = user.Id, token=HttpUtility.UrlEncode(emailConfToken)},Request.Scheme, Request.Host.ToString());
                await _emailService.SendAsync(user.Email,"Verification Email", $"<html><a href=\"{urlToConfirm}\">Confirm here</a></html>");
                return RedirectToAction("EmailVerification");
            }
            return View();
        }
        public IActionResult EmailVerification()
        {
            return View();
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return BadRequest();
            var confirmEmailResult  = await _userManager.ConfirmEmailAsync(user, HttpUtility.UrlDecode(token));
            if (confirmEmailResult.Succeeded)
            {
                return View();
            }
            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {

                var loginResult = await _signInManager.PasswordSignInAsync(user, password, false, false);
                if (loginResult.Succeeded)
                {
                    return RedirectToAction("Index", new { Controller = "Patient" });
                }
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", new { Controller = "Account" });
        }
    }
}