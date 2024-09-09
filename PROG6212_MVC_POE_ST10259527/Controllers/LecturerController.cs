using Microsoft.AspNetCore.Mvc;
using PROG6212_MVC_POE_ST10259527.Models;

namespace PROG6212_MVC_POE_ST10259527.Controllers
{
    public class LecturerController : Controller
    {
        
        public IActionResult LecturerLogin()
        {
            return View();
        }

        public IActionResult LecturerSignUp()
        {
            return View();
        }
        public IActionResult Claims()
        {
            return View();
        }

        public IActionResult StatusView()
        {
            return View();
        }
        [HttpPost]
        public IActionResult submit(ClaimModel model)
        {
            // Handle the submission of the claim here
            // Redirect to a status page or list of claims
            return RedirectToAction("Claims");
        }
        [HttpPost]
        public IActionResult Login(string name, string password)
        {
            // Handle the submission of the claim here
            // Redirect to a status page or list of claims
            return RedirectToAction("StatusView");
        }
    }
}

