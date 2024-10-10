using Microsoft.AspNetCore.Mvc;
using PROG6212_MVC_POE_ST10259527.Models;
using PROG6212_MVC_POE_ST10259527.Services;

namespace PROG6212_MVC_POE_ST10259527.Controllers
{
    public class AdminController : Controller
    {
        private readonly TableServices _tableServices;

        public AdminController(TableServices tableServices)
        {
            _tableServices = tableServices;
        }

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
        public async Task<IActionResult> SignUp(UserProfileModel userProfile)
        {
            if (ModelState.IsValid)
            {
                // Add the user profile to Azure Table Storage
                await _tableServices.AddEntityAsync(userProfile);
                return RedirectToAction("Login");
            }
            return View(userProfile);  // Return to SignUp view if model is invalid
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                // Validate the user credentials
                var user = await _tableServices.GetUserByEmailAsync(loginModel.Email);
                if (user != null && user.Password == loginModel.Password)
                {
                    // Logic for a successful login (e.g., setting session or cookie)
                    // Here you can redirect to a dashboard or home page
                    return RedirectToAction("VerifyClaimsView");
                }
                ModelState.AddModelError("", "Invalid login attempt.");
            }
            return View(loginModel); // Return to login view if model is invalid or login fails
        }
    }
}
