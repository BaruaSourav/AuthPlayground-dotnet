using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClaimBasedAuthorization.Controllers
{
    public class PatientHealthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult PatientHealthInfo()
        {
            return View();
        }
        
        public IActionResult Login()
        {
            return View();
        }
        

    }
}