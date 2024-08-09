using Microsoft.AspNetCore.Mvc;

namespace PROG6212_MVC_POE_ST10259527.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }
    }
}
