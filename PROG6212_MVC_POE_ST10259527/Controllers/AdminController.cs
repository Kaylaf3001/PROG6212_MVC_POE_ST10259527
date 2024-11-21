using Microsoft.AspNetCore.Mvc;
using PROG6212_MVC_POE_ST10259527.Models;
using PROG6212_MVC_POE_ST10259527.Services;
using System.Text;
using FluentValidation;

namespace PROG6212_MVC_POE_ST10259527.Controllers
{
    //-----------------------------------------------------------------------------------------------------
    //Constructor for the AdminController
    //-----------------------------------------------------------------------------------------------------
    public class AdminController : Controller
    {
        private readonly SqlService _sqlService;
        private readonly FileService _fileService;
        private readonly IValidator<ClaimsModel> _claimsValidator;

        public AdminController(FileService fileService, SqlService sqlService, IValidator<ClaimsModel> claimsValidator)
        {
            _sqlService = sqlService;
            _fileService = fileService;
            _claimsValidator = claimsValidator;
        }

        public async Task<IActionResult> VerifyClaimsView()
        {
            var claims = await _sqlService.GetAllClaimsAsync();
            claims = claims.Where(claim => claim.Status != "Approved" && claim.Status != "Rejected").ToList();

            // Validate each claim and update IsValid property
            foreach (var claim in claims)
            {
                var validationResult = _claimsValidator.Validate(claim);
                claim.IsValid = validationResult.IsValid;
            }

            return View(claims);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveClaim(int claimId)
        {
            var claim = await _sqlService.GetClaimByIdAsync(claimId);

            var validationResult = _claimsValidator.Validate(claim);
            if (!validationResult.IsValid)
            {
                TempData["ErrorMessage"] = "Claim validation failed.";
                return RedirectToAction("VerifyClaimsView");
            }

            var approvedClaim = ClaimsModel.ApproveClaim(claim);
            await _sqlService.UpdateClaimStatusAsync(approvedClaim);

            TempData["SuccessMessage"] = $"Claim {claimId} approved successfully!";
            return RedirectToAction("VerifyClaimsView");
        }

        [HttpPost]
        public async Task<IActionResult> RejectClaim(int claimId)
        {
            var claim = await _sqlService.GetClaimByIdAsync(claimId);
            var rejectedClaim = ClaimsModel.RejectClaim(claim);
            await _sqlService.UpdateClaimStatusAsync(rejectedClaim);

            TempData["SuccessMessage"] = $"Claim {claimId} rejected.";
            return RedirectToAction("VerifyClaimsView");
        }

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

        [HttpPost]
        public async Task<IActionResult> GenerateReport()
        {
            var claims = await _sqlService.GetAllClaimsAsync();
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

        public IActionResult Policy()
        {
            return View();
        }
    }
}