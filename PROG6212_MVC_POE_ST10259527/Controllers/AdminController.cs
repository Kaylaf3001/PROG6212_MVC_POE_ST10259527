using Microsoft.AspNetCore.Mvc;
using PROG6212_MVC_POE_ST10259527.Models;
using PROG6212_MVC_POE_ST10259527.Services;
using System.Text;

namespace PROG6212_MVC_POE_ST10259527.Controllers
{
    //-----------------------------------------------------------------------------------------------------
    //Constructor for the AdminController
    //-----------------------------------------------------------------------------------------------------
    public class AdminController : Controller
    {
        private readonly SqlService _sqlService;
        private readonly FileService _fileService;

        public AdminController(FileService fileService, SqlService sqlService)
        {
            _sqlService = sqlService;
            _fileService = fileService;
        }
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        // Displays all the claims that need ot be verified
        //-----------------------------------------------------------------------------------------------------
        public async Task<IActionResult> VerifyClaimsView()
        {
            var claims = await _sqlService.GetAllClaimsAsync();

            claims = claims.Where(claim => claim.Status != "Approved" && claim.Status != "Rejected").ToList();
            
            return View(claims);
        }
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        // This method is called when the admin wants to approve a claim
        //-----------------------------------------------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> ApproveClaim(int claimId)
        {
            var claim = await _sqlService.GetClaimByIdAsync(claimId);

            var approvedClaim = ClaimsModel.ApproveClaim(claim);
            await _sqlService.UpdateClaimStatusAsync(approvedClaim);

            TempData["SuccessMessage"] = $"Claim {claimId} approved successfully!";
            return RedirectToAction("VerifyClaimsView");
        }
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        // This method is called when the admin wants to reject a claim
        //-----------------------------------------------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> RejectClaim(int claimId)
        {
            var claim = await _sqlService.GetClaimByIdAsync(claimId);
            var rejectedClaim = ClaimsModel.RejectClaim(claim);
            await _sqlService.UpdateClaimStatusAsync(rejectedClaim);

            TempData["SuccessMessage"] = $"Claim {claimId} rejected.";
            return RedirectToAction("VerifyClaimsView");
        }
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        // This method is called when the admin wants to download a file
        //-----------------------------------------------------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            var fileStream = await _fileService.DownloadFileAsync("lecturerclaimsfiles", fileName);
            if (fileStream == null)
            {
                return NotFound();
            }

            return File(fileStream, "application/octet-stream", fileName);
        }
        //-----------------------------------------------------------------------------------------------------

        
    }
}