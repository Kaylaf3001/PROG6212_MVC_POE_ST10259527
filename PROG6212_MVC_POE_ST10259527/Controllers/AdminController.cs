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
        private readonly TableServices _tableServices;
        private readonly TableServices _tableClaimsServices;
        private readonly FileService _fileService;

        public AdminController(TableServices tableServices, TableServices tableClaimsServices, FileService fileService)
        {
            _tableServices = tableServices;
            _tableClaimsServices = tableClaimsServices;
            _fileService = fileService;
        }
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        // Displays all the claims that need ot be verified
        //-----------------------------------------------------------------------------------------------------
        public async Task<IActionResult> VerifyClaimsView()
        {
            var claims = await _tableServices.GetAllClaims();

            claims = claims.Where(claim => claim.Status != "Approved" && claim.Status != "Rejected").ToList();

            // Validate each claim and update IsValid property
            foreach (var claim in claims)
            {
                claim.IsValid = ValidateClaims(claim);
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
            var claim = await _tableClaimsServices.GetClaimById(claimId);

            if (!ValidateClaims(claim))
            {
                TempData["ErrorMessage"] = "Claim validation failed.";
                return RedirectToAction("VerifyClaimsView");
            }

            var approvedClaim = ClaimsModel.ApproveClaim(claim);
            await _tableClaimsServices.UpdateClaimStatus(approvedClaim);

            TempData["SuccessMessage"] = $"Claim {claimId} approved successfully!";
            return RedirectToAction("VerifyClaimsView");
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
        //Validate and policy check for the claims
        //-----------------------------------------------------------------------------------------------------
        private bool ValidateClaims(ClaimsModel claim)
        {
            // Replace with your policy value
            // Daily hours worked should not exceed 8 hours
            double maxHours = 8;  

            if (claim.HoursWorked <= 0 || claim.HoursWorked > maxHours)
            {
                return false;
            }

            double calculatedAmount = claim.HoursWorked * claim.HourlyRate;
            if (calculatedAmount != claim.CalculateTotalAmount())
            {
                return false;
            }

            return true;
        }
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        // Generate and send report to HR
        //-----------------------------------------------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> GenerateReport()
        {
            var claims = await _tableServices.GetAllClaims();
            var approvedClaims = claims.Where(claim => claim.Status == "Approved" && !claim.IsPaid).ToList();

            if (!approvedClaims.Any())
            {
                TempData["ErrorMessage"] = "No approved claims to report.";
                return RedirectToAction("VerifyClaimsView");
            }

            var reportContent = GenerateReportContent(approvedClaims);
            var reportFileName = $"LecturerPaymentReport_{DateTime.Now:yyyyMMddHHmmss}.txt";

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(reportContent)))
            {
                await _fileService.UploadFileAsync("hrreports", reportFileName, stream);
            }

            TempData["SuccessMessage"] = "Report generated and sent to HR successfully!";
            return RedirectToAction("VerifyClaimsView");
        }
        //----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        // Generate the report content
        //-----------------------------------------------------------------------------------------------------
        private string GenerateReportContent(List<ClaimsModel> approvedClaims)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Lecturer Payment Report");
            sb.AppendLine("=====================================");
            sb.AppendLine("Lecturer Name\tModule Code\tTotal Amount");

            foreach (var claim in approvedClaims)
            {
                sb.AppendLine($"{claim.LecturerName}\t{claim.ModuleCode}\t{claim.CalculateTotalAmount()}");
            }

            sb.AppendLine("=====================================");
            sb.AppendLine($"Total Amount to be Paid: {approvedClaims.Sum(c => c.CalculateTotalAmount())}");

            return sb.ToString();
        }
        //-----------------------------------------------------------------------------------------------------
    }
}