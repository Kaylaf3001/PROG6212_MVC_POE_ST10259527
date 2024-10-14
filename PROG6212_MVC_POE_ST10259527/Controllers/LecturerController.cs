using Microsoft.AspNetCore.Mvc;
using PROG6212_MVC_POE_ST10259527.Models;
using PROG6212_MVC_POE_ST10259527.Services;

namespace PROG6212_MVC_POE_ST10259527.Controllers
{
    public class LecturerController : Controller
    {
        private readonly TableServices _tableServices;

        public LecturerController(TableServices tableServices)
        {
            _tableServices = tableServices;
        }

        public IActionResult LecturerLogin()
        {
            return View();
        }

        public async Task<IActionResult> LecturerSignUp(UserProfileModel userProfile)
        {

            // Ensure the role is set as Lecturer
            userProfile.Role = "Lecturer";

            if (ModelState.IsValid)
            {
                await _tableServices.AddEntityAsync(userProfile);
                return RedirectToAction("LecturerLogin");
            }
            return View(userProfile);
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
            return RedirectToAction("StatusView");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _tableServices.GetUserByEmailAsync(loginModel.Email);
                if (user != null && user.Password == loginModel.Password)
                {
                    if (user.Role == "Lecturer")
                    {
                        // Redirect to Lecturer's dashboard
                        return RedirectToAction("Claims");
                    }
                    else
                    {
                        ModelState.AddModelError("", "You are not authorized to access the lecturer dashboard.");
                        return View(loginModel);
                    }
                }
                ModelState.AddModelError("", "Invalid login attempt.");
            }
            return View(loginModel);
        }

    }
}


