using Microsoft.AspNetCore.Mvc;
using PROG6212_MVC_POE_ST10259527.Models;

namespace PROG6212_MVC_POE_ST10259527.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Claims()
        {
            return View();
        }

        public IActionResult StatusView()
        {
            return View();
        }

        public IActionResult VerifyClaimsView()
        {
            return View();
        }
        
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult AdminLecturer()
        {
            return View();
        }
        // POST: /Home/Submit
        [HttpPost]
        public IActionResult Submit(ClaimModel model)
        {
            // Handle the submission of the claim here
            // Redirect to a status page or list of claims
            return RedirectToAction("StatusView");
        }

        [HttpPost]
        public IActionResult ApproveClaim(int claimId)
        {
            // Handle claim approval logic here
            return RedirectToAction("VerifyClaimsView");
        }
    }
}
