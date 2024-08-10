using Microsoft.AspNetCore.Mvc;

namespace PROG6212_MVC_POE_ST10259527.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }
        public IActionResult VerifyClaimsView()
        {
            return View();
        }
       
        [HttpPost]
        public IActionResult Login(int claimId)
        {
            // Handle claim approval logic here
            return RedirectToAction("VerifyClaimsView");
        }
    }
}

