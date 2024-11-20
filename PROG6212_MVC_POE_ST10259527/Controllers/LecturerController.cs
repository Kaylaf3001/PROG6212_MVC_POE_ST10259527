using Azure.Storage.Files.Shares;
using Microsoft.AspNetCore.Mvc;
using PROG6212_MVC_POE_ST10259527.Models;
using PROG6212_MVC_POE_ST10259527.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PROG6212_MVC_POE_ST10259527.Controllers
{
    //-----------------------------------------------------------------------------------------------------
    // Constructor for the LecturerController
    //-----------------------------------------------------------------------------------------------------
    public class LecturerController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SqlService _sqlService;
        private readonly FileService _fileService;

        public LecturerController(FileService fileService, IHttpContextAccessor httpContextAccessor, SqlService sqlService)
        {
            _fileService = fileService;
            _httpContextAccessor = httpContextAccessor;
            _sqlService = sqlService;
        }
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        // Displays all the views
        //-----------------------------------------------------------------------------------------------------
        public IActionResult LecturerLogin()
        {
            return View();
        }

        public IActionResult AddClaim()
        {
            return View();
        }
        //----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        //Displays the status of the claims
        //-----------------------------------------------------------------------------------------------------
        public async Task<IActionResult> StatusView()
        {
            int lecturerID = _httpContextAccessor.HttpContext.Session.GetInt32("UserID") ?? 0;
            var claims = await _sqlService.GetAllClaimsAsync();
            claims = claims.Where(l => l.userID == lecturerID).ToList();
            return View(claims);
        }
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        //Allows the lecturer to add a claim
        //-----------------------------------------------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> AddClaim(ClaimsModel claim, IFormFile SupportingDocument)
        {
            claim.Status = "Pending"; // Set the claim status to Pending
            claim.IsPaid = false; // Set the claim to unpaid
            claim.userID = _httpContextAccessor.HttpContext.Session.GetInt32("UserID") ?? 0;

            var lecturer = await _sqlService.GetUserByIDAsync(claim.userID);
            claim.LecturerName = lecturer.FirstName;


            if (SupportingDocument != null && SupportingDocument.Length > 0)
            {
                // Upload the file to Azure File Share
                claim.SupportingDocumentName = SupportingDocument.FileName;
                var fileUrl = await _fileService.UploadFileAsync("lecturer-files",SupportingDocument.FileName, SupportingDocument.OpenReadStream());
            }

            // Add the claim to the table storage
            await _sqlService.AddClaimAsync(claim);

            return RedirectToAction("StatusView");
        }
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        // Filter claims by status
        //-----------------------------------------------------------------------------------------------------
        public async Task<IActionResult> FilterByStatus(string status)
        {
            var claims = await _sqlService.GetAllClaimsAsync();
            var filteredClaims = claims.Where(c => c.Status == status).ToList();
            return View("StatusView", filteredClaims);
        }
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        // Clear filters (show all claims)
        //-----------------------------------------------------------------------------------------------------
        public async Task<IActionResult> ClearFilters()
        {
            var claims = await _sqlService.GetAllClaimsAsync();
            return View("StatusView", claims);
        }
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        // Allows the lecturer to download a file
        //-----------------------------------------------------------------------------------------------------
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            var fileStream = await _fileService.DownloadFileAsync("lecturer-files", fileName);
            if (fileStream == null)
            {
                return NotFound();
            }

            return File(fileStream, "application/octet-stream", fileName);
        }
        //-----------------------------------------------------------------------------------------------------
    }
}
//-----------------------------------------------End-Of-File----------------------------------------------------
