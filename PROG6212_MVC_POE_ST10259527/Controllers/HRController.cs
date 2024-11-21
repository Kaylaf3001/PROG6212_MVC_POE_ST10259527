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
            var reportContent = new Dictionary<string, string>();
            foreach (var reportFile in reportFiles)
            {
                var fileStream = await _fileService.DownloadFileAsync("hrreports", reportFile);
                using (var reader = new StreamReader(fileStream))
                {
                    var content = await reader.ReadToEndAsync();
                    reportContent[reportFile] = content;
                }
            }
            ViewBag.ReportContent = reportContent;

            return View(reportFiles);
        }

        private Dictionary<string, string> PreviewReportContent(List<ClaimsModel> claims)
        {
            var reportContent = new Dictionary<string, string>();
            foreach (var claim in claims)
            {
                var content = new StringBuilder();
                content.AppendLine("Module Code, Lecturer Name, Hours Worked, Hourly Rate, Date, Total Amount, Status");
                content.AppendLine($"{claim.ModuleCode}, {claim.LecturerName}, {claim.HoursWorked}, {claim.HourlyRate}, {claim.Date:yyyy-MM-dd}, {claim.CalculateTotalAmount()}, {claim.Status}");
                reportContent[claim.SupportingDocumentName] = content.ToString();
            }
            return reportContent;
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
            var reportContent = PreviewReportContent(claims);
            var byteArray = Encoding.UTF8.GetBytes(string.Join("\n", reportContent.Values));
            var stream = new MemoryStream(byteArray);
            return File(stream, "text/plain", "GeneratedReport.txt");
        }

        // Generate and send report to HR
        [HttpPost]
        public async Task<IActionResult> GenerateReport()
        {
            var claims = await _sqlService.GetAllClaimsAsync();
            var approvedClaims = claims.Where(claim => claim.Status == "Approved" && !claim.IsPaid).ToList();

            if (!approvedClaims.Any())
            {
                TempData["ErrorMessage"] = "No approved claims to report.";
                return RedirectToAction("HRDashboard");
            }

            var reportContent = GenerateReportContent(approvedClaims);
            var reportFileName = $"LecturerPaymentReport_{DateTime.Now:yyyyMMddHHmmss}.txt";

            // Check if the report already exists
            var existingReports = await _fileService.ListFilesAsync("hrreports");
            foreach (var existingReport in existingReports)
            {
                var existingReportStream = await _fileService.DownloadFileAsync("hrreports", existingReport);
                using (var reader = new StreamReader(existingReportStream))
                {
                    var existingReportContent = await reader.ReadToEndAsync();
                    if (existingReportContent == reportContent)
                    {
                        TempData["ErrorMessage"] = "This report has already been generated. Please try again later.";
                        return RedirectToAction("HRDashboard");
                    }
                }
            }

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(reportContent)))
            {
                await _fileService.UploadFileAsync("hrreports", reportFileName, stream);
            }

            TempData["SuccessMessage"] = "Report generated and sent to HR successfully!";
            return RedirectToAction("HRDashboard");
        }

        // Generate the report content
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

