using Microsoft.AspNetCore.Mvc;
using PROG6212_MVC_POE_ST10259527.Models;

namespace PROG6212_MVC_POE_ST10259527.Controllers
{
    //-----------------------------------------------------------------------------------------------------
    // Deals with the privacy view and Admin and Lecturer choices
    //-----------------------------------------------------------------------------------------------------
    public class HomeController : Controller
    {
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult AdminLecturer()
        {
            return View();
        }
    }       
}
//-----------------------------------------------End-Of-File----------------------------------------------------
