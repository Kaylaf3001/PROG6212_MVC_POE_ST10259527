using Microsoft.AspNetCore.Mvc;
using PROG6212_MVC_POE_ST10259527.Services;

namespace PROG6212_MVC_POE_ST10259527.Controllers
{
    public class HRController : Controller
    {
        private readonly FileService _fileService;

        public HRController(FileService fileService)
        {
            _fileService = fileService;
        }

        public async Task<IActionResult> HRDashboard()
        {
            var reportFiles = await _fileService.ListFilesAsync("hrreports");
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
    }
}
