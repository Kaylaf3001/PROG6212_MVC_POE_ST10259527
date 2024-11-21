using Microsoft.AspNetCore.Mvc;
using PROG6212_MVC_POE_ST10259527.Models;
using PROG6212_MVC_POE_ST10259527.Services;
using System.Text;

namespace PROG6212_MVC_POE_ST10259527.Controllers
{
    public class HRController : Controller
    {
        private readonly FileService _fileService;
        private readonly SqlService _sqlService;

        public HRController(FileService fileService, SqlService sqlService)
        {
            _fileService = fileService;
            _sqlService = sqlService;
        }

        public async Task<IActionResult> HRDashboard()
        {
            var reportFiles = await _fileService.ListFilesAsync("hrreports");
            var lecturers = await _sqlService.GetAllUsersAsync();
            ViewBag.Lecturers = lecturers;

            // Generate report content
            var claims = await _sqlService.GetAllClaimsAsync();
            var reportContent = GenerateReportContent(claims);
            ViewBag.ReportContent = reportContent;

            return View(reportFiles);
        }

        private List<string> GenerateReportContent(List<ClaimsModel> claims)
        {
            var reportRows = new List<string>
                {
                    "Module Code, Lecturer Name, Hours Worked, Hourly Rate, Date, Total Amount, Status"
                };
            reportRows.AddRange(claims.Select(claim => $"{claim.ModuleCode}, {claim.LecturerName}, {claim.HoursWorked}, {claim.HourlyRate}, {claim.Date:yyyy-MM-dd}, {claim.CalculateTotalAmount()}, {claim.Status}"));
            return reportRows;
        }

        public async Task<IActionResult> DownloadReport(string fileName)
        {
            var fileStream = await _fileService.DownloadFileAsync("hrreports", fileName);
            if (fileStream == null)
            {
                return NotFound();
            }

            return File(fileStream, "application/octet-stream", fileName);
        }

        [HttpPost]
        public async Task<IActionResult> PayReport(string fileName)
        {
            var claims = await _sqlService.GetAllClaimsAsync();
            var approvedClaims = claims.Where(claim => claim.Status == "Approved" && !claim.IsPaid).ToList();

            foreach (var claim in approvedClaims)
            {
                ClaimsModel.MarkAsPaid(claim);
                await _sqlService.UpdateClaimStatusAsync(claim);
            }

            await _fileService.DeleteFileAsync("hrreports", fileName);
            TempData["SuccessMessage"] = $"Payment for {fileName} processed and report removed successfully!";
            return RedirectToAction("HRDashboard");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteReport(string fileName)
        {
            try
            {
                await _fileService.DeleteFileAsync("hrreports", fileName);
                return Json(new { success = true, message = $"Report {fileName} deleted successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error deleting report {fileName}: {ex.Message}" });
            }
        }

        // New methods for editing lecturer information
        public async Task<IActionResult> EditLecturer(int id)
        {
            var lecturer = await _sqlService.GetUserByIDAsync(id);
            if (lecturer == null)
            {
                return NotFound();
            }
            return View(lecturer);
        }

        [HttpPost]
        public async Task<IActionResult> EditLecturer(UserProfileModel lecturer)
        {
            if (ModelState.IsValid)
            {
                await _sqlService.UpdateUserProfileAsync(lecturer);
                TempData["SuccessMessage"] = "Lecturer information updated successfully!";
                return RedirectToAction("HRDashboard");
            }
            return View(lecturer);
        }

        // Method to download the generated report as a text file
        public IActionResult DownloadGeneratedReport()
        {
            var claims = _sqlService.GetAllClaimsAsync().Result;
            var reportContent = GenerateReportContent(claims);
            var byteArray = Encoding.UTF8.GetBytes(string.Join("\n", reportContent));
            var stream = new MemoryStream(byteArray);
            return File(stream, "text/plain", "GeneratedReport.txt");
        }
    }
}

