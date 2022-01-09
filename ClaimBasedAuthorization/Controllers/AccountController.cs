using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace ClaimBasedAuthorization.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromForm] string name, string email)
        {
            var doctorClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.Email, email)
            };

            var doctorIdentity = new ClaimsIdentity(doctorClaims, "Doctor Identity");
            var userPrincipal = new ClaimsPrincipal(new [] {doctorIdentity}); 

            await HttpContext.SignInAsync(userPrincipal);
            
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index",new {Controller="PatientHealth"});
        }
    }
}