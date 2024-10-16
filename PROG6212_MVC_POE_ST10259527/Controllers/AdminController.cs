using Microsoft.AspNetCore.Mvc;
using PROG6212_MVC_POE_ST10259527.Models;
using PROG6212_MVC_POE_ST10259527.Services;

namespace PROG6212_MVC_POE_ST10259527.Controllers
{
    //-----------------------------------------------------------------------------------------------------
    //Constructor for the AdminController
    //-----------------------------------------------------------------------------------------------------
    public class AdminController : Controller
    {
        private readonly TableServices _tableServices;
        private readonly TableServices _tableClaimsServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AdminController(TableServices tableServices, IHttpContextAccessor httpContextAccessor, TableServices tableClaimsServices)
        {
            _tableServices = tableServices;
            _httpContextAccessor = httpContextAccessor;
            _tableClaimsServices = tableClaimsServices;
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
        // Displays all the claims that need ot be verified
        //-----------------------------------------------------------------------------------------------------
        public async Task<IActionResult> VerifyClaimsView()
        {
            
           var claims = await _tableServices.GetAllClaims();

            // Only display the claims that are pending
           foreach (var claim in claims)
            {
                if (claim.Status == "Approved")
                {
                    claims.Remove(claim);
                }
                else if (claim.Status == "Rejected")
                {
                    claims.Remove(claim);
                }
            }
            return View(claims);
        }
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        // This method is called when the admin wants to approve a claim
        //-----------------------------------------------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> ApproveClaim(string claimId)
        {
            // Get the claim by ID
            await _tableClaimsServices.UpdateClaimStatus(claimId, "Approved");
          
            return Json(new { success = true, message = $"Claim {claimId} approved successfully!" });
        }
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        // This method is called when the admin wants to reject a claim
        //-----------------------------------------------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> RejectClaim(string claimId)
        {
            // Get the claim by ID
            await _tableServices.UpdateClaimStatus(claimId, "Rejected");
 
            return Json(new { success = true, message = $"Claim {claimId} rejected." });
        }
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        //This method allows a admin to create an account
        //-----------------------------------------------------------------------------------------------------
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
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        //This method allows a admin to login
        //-----------------------------------------------------------------------------------------------------
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
                        // Return the same view with a notification
                        ViewData["LoginResult"] = "1"; // 1 indicates failure
                        return View(loginModel);
                    }
                }
                ViewData["LoginResult"] = "1"; // 1 indicates failure
            }
            return View(loginModel);
        }
        //-----------------------------------------------------------------------------------------------------
    }
}
//-----------------------------------------------End-Of-File----------------------------------------------------
