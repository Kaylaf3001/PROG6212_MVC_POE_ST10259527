﻿using Microsoft.AspNetCore.Mvc;
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

            //TODO: First break points hit but not adding
            var claim = await _tableClaimsServices.GetClaimById(claimId);

            if (!ValidateClaims(claim))
            {
                return Json(new { success = false, message = "Claim validation failed." });
            }

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
        private bool ValidateClaims(ClaimsModel claim)
        {
            // Example validation logic
            double hourlyRate = 100.0; // Replace with your policy value
            double maxHours = 40.0;    // Replace with your policy value

            if (claim.HoursWorked <= 0 || claim.HoursWorked > maxHours)
            {
                return false;
            }

            double calculatedAmount = claim.HoursWorked * hourlyRate;
            if (calculatedAmount != claim.CalculateTotalAmount())
            {
                return false;
            }

            return true;
        }

        //-----------------------------------------------------------------------------------------------------
        // Generate and send report to HR
        //-----------------------------------------------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> SendReportToHR()
        {
            var claims = await _tableServices.GetAllClaims();
            var approvedClaims = claims.Where(claim => claim.Status == "Approved").ToList();

            if (!approvedClaims.Any())
            {
                return Json(new { success = false, message = "No approved claims to report." });
            }

            var reportContent = GenerateReportContent(approvedClaims);
            var reportFileName = $"LecturerPaymentReport_{DateTime.Now:yyyyMMddHHmmss}.txt";

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(reportContent)))
            {
                await _fileService.UploadFileAsync("hrreports", reportFileName, stream);
            }

            return Json(new { success = true, message = "Report sent to HR successfully!" });
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
    }
}