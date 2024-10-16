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
        
        public AdminController(TableServices tableServices, TableServices tableClaimsServices)
        {
            _tableServices = tableServices;
            _tableClaimsServices = tableClaimsServices;
        }
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        // Displays all the claims that need ot be verified
        //-----------------------------------------------------------------------------------------------------
        public async Task<IActionResult> VerifyClaimsView()
        {
            var claims = await _tableServices.GetAllClaims();

            // Only display the claims that are pending
            claims = claims.Where(claim => claim.Status != "Approved" && claim.Status != "Rejected").ToList();

            return View(claims);
        }
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        // This method is called when the admin wants to approve a claim
        //-----------------------------------------------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> ApproveClaim(string claimId)
        {

            //TODO: First break points hit but not adding
            var claim = await _tableClaimsServices.GetClaimById(claimId);

            var approvedClaim = ClaimsModel.ApproveClaim(claim);

            await _tableClaimsServices.UpdateClaimStatus(approvedClaim);
          
            return Json(new { success = true, message = $"Claim {claimId} approved successfully!" });
        }
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        // This method is called when the admin wants to reject a claim
        //-----------------------------------------------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> RejectClaim(string claimId)
        {
            var claim = await _tableServices.GetClaimById(claimId);

            var rejectedClaim = ClaimsModel.RejectClaim(claim);
            
            await _tableServices.UpdateClaimStatus(rejectedClaim);
 
            return Json(new { success = true, message = $"Claim {claimId} rejected." });
        }
        //-----------------------------------------------------------------------------------------------------
    }
}
//-----------------------------------------------End-Of-File----------------------------------------------------
