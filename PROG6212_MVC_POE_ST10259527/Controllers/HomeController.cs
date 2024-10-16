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

        //-----------------------------------------------------------------------------------------------------
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
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var users = await _tableServices.GetAllUsers();
                var user = UserProfileModel.LoginUser(users, loginModel.Email, loginModel.Password);

                if (user == null)
                {
                    ViewData["LoginResult"] = "1"; // 1 indicates failure
                    return View(loginModel);
                }
                else
                {
                    if (user.IsAdmin)
                    {
                        _httpContextAccessor.HttpContext.Session.SetString("UserID", user.RowKey);
                        // Redirect to Admin's dashboard
                        return RedirectToAction("VerifyClaimsView", "Admin");
                    }
                    else
                    {
                        _httpContextAccessor.HttpContext.Session.SetString("UserID", user.RowKey);
                        // Redirect to Lecturer's dashboard
                        return RedirectToAction("StatusView", "Lecturer");
                    } 
                }
            }
            return View(loginModel);
        }
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        //This method allows a admin to create an account
        //-----------------------------------------------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> SignUp(UserProfileModel userProfile)
        {
            if (ModelState.IsValid)
            {
                await _tableServices.AddEntityAsync(userProfile);
                return RedirectToAction("Login");
            }
            return View(userProfile);
        }
        //-----------------------------------------------------------------------------------------------------
    }
}
//-----------------------------------------------End-Of-File----------------------------------------------------
