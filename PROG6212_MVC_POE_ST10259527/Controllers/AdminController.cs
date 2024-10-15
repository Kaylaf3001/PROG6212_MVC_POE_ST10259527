using Microsoft.AspNetCore.Mvc;
using PROG6212_MVC_POE_ST10259527.Models;
using PROG6212_MVC_POE_ST10259527.Services;

namespace PROG6212_MVC_POE_ST10259527.Controllers
{
    public class AdminController : Controller
    {
        private readonly TableServices _tableServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AdminController(TableServices tableServices, IHttpContextAccessor httpContextAccessor)
        {
            _tableServices = tableServices;
            _httpContextAccessor = httpContextAccessor;
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
            // Ensure the role is set as Admin
            userProfile.Role = "Admin";

            if (ModelState.IsValid)
            {
                await _tableServices.AddEntityAsync(userProfile);
                return RedirectToAction("Login");
            }
            return View(userProfile);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _tableServices.GetUserByEmailAsync(loginModel.Email);
                if (user != null && user.Password == loginModel.Password)
                {
                    if (user.Role == "Admin")
                    {
                        _httpContextAccessor.HttpContext.Session.SetString("UserID", user.RowKey);
                        // Redirect to Admin's dashboard
                        return RedirectToAction("VerifyClaimsView");
                    }
                    else
                    {
                        ModelState.AddModelError("", "You are not authorized to access the admin dashboard.");
                        return View(loginModel);
                    }
                }
                ModelState.AddModelError("", "Invalid login attempt.");
            }
            return View(loginModel);
        }

    }
}
