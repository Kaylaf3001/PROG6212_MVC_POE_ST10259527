using Microsoft.AspNetCore.Mvc;
using PROG6212_MVC_POE_ST10259527.Models;
using PROG6212_MVC_POE_ST10259527.Services;

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
            return View(reportFiles);
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
            await _fileService.DeleteFileAsync("hrreports", fileName);
            TempData["SuccessMessage"] = $"Report {fileName} deleted successfully!";
            return RedirectToAction("HRDashboard");
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
    }
}

