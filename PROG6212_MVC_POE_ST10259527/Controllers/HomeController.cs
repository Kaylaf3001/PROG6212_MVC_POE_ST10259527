using Microsoft.AspNetCore.Mvc;
using PROG6212_MVC_POE_ST10259527.Models;
using PROG6212_MVC_POE_ST10259527.Services;
using PROG6212_MVC_POE_ST10259527.ViewModels;

namespace PROG6212_MVC_POE_ST10259527.Controllers
{
    //-----------------------------------------------------------------------------------------------------
    // Deals with the privacy view and Admin and Lecturer choices
    //-----------------------------------------------------------------------------------------------------
    public class HomeController : Controller
    {
        private readonly TableServices _tableServices;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(TableServices tableServices, IHttpContextAccessor httpContextAccessor)
        {
            _tableServices = tableServices;
            _httpContextAccessor = httpContextAccessor;
        }
        //-----------------------------------------------------------------------------------------------------

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult SignUp()
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
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        // Allows the user to login
        //-----------------------------------------------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var users = await _tableServices.GetAllUsers();
                var user = UserProfileModel.LoginUser(users, loginViewModel);

                if (user == null)
                {
                    ViewData["LoginResult"] = "1"; // 1 indicates failure
                    return View(loginViewModel);
                }
                else
                {
                    _httpContextAccessor.HttpContext.Session.SetString("UserID", user.RowKey);
                    switch (user.Role)
                    {
                        case "Admin":
                            // Redirect to Admin's dashboard
                            return RedirectToAction("VerifyClaimsView", "Admin");
                        case "Lecturer":
                            // Redirect to Lecturer's dashboard
                            return RedirectToAction("StatusView", "Lecturer");
                        case "HR":
                            // Redirect to HR's dashboard
                            return RedirectToAction("HRDashboard", "HR");
                        default:
                            // Handle unknown role
                            ViewData["LoginResult"] = "1"; // 1 indicates failure
                            return View(loginViewModel);
                    }
                }
            }
            return View(loginViewModel);
        }
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        // This method allows an admin to create an account
        //-----------------------------------------------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel signUpViewModel)
        {
            if (ModelState.IsValid)
            {
                var newUser = UserProfileModel.SignUpUser(signUpViewModel);

                await _tableServices.AddEntityAsync(newUser);
                return RedirectToAction("Login");
            }
            return View(signUpViewModel);
        }
        //-----------------------------------------------------------------------------------------------------
    }
}
//-----------------------------------------------End-Of-File----------------------------------------------------
