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
        private readonly TableServices _tableServices;
        private readonly TableServices _tableClaimsServices;
        private readonly FileService _fileService;

        public LecturerController(TableServices tableServices, TableServices tableClaimsServices, FileService fileService, IHttpContextAccessor httpContextAccessor)
        {
            _tableServices = tableServices;
            _tableClaimsServices = tableClaimsServices;
            _fileService = fileService;
            _httpContextAccessor = httpContextAccessor;
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
            var lecturerID = _httpContextAccessor.HttpContext.Session.GetString("UserID");
            var claims = await _tableServices.GetAllClaims();
            claims = claims.Where(l => l.LecturerID == lecturerID).ToList();
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
            claim.LecturerID = _httpContextAccessor.HttpContext.Session.GetString("UserID");

            var lecturer = await _tableServices.GetUserByIDAsync(claim.LecturerID);
            claim.LecturerName = lecturer.FirstName;


            if (SupportingDocument != null && SupportingDocument.Length > 0)
            {
                // Upload the file to Azure File Share
                claim.SupportingDocumentName = SupportingDocument.FileName;
                var fileUrl = await _fileService.UploadFileAsync("lecturer-files",SupportingDocument.FileName, SupportingDocument.OpenReadStream());
                claim.SupportingDocumentUrl = fileUrl; // Store the file URL in the claim
            }

            // Add the claim to the table storage
            await _tableClaimsServices.AddClaim(claim);

            return RedirectToAction("StatusView");
        }
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        // Filter claims by status
        //-----------------------------------------------------------------------------------------------------
        public async Task<IActionResult> FilterByStatus(string status)
        {
            var claims = await _tableServices.GetAllClaims();
            var filteredClaims = claims.Where(c => c.Status == status).ToList();
            return View("StatusView", filteredClaims);
        }
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        // Clear filters (show all claims)
        //-----------------------------------------------------------------------------------------------------
        public async Task<IActionResult> ClearFilters()
        {
            var claims = await _tableServices.GetAllClaims();
            return View("StatusView", claims);
        }
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
    }
}
//-----------------------------------------------End-Of-File----------------------------------------------------
