using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityAuthentication.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
                var loginResult = await _signInManager.PasswordSignInAsync(user, password, false, false);
                return RedirectToAction("Login", new { Controller = "Account" });
            }
            return View();
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