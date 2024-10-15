using Azure.Storage.Files.Shares;
using Microsoft.AspNetCore.Mvc;
using PROG6212_MVC_POE_ST10259527.Models;
using PROG6212_MVC_POE_ST10259527.Services;
using System.Threading.Tasks;

namespace PROG6212_MVC_POE_ST10259527.Controllers
{
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

        public IActionResult LecturerLogin()
        {
            return View();
        }

        public async Task<IActionResult> LecturerSignUp(UserProfileModel userProfile)
        {
            // Ensure the role is set as Lecturer
            userProfile.Role = "Lecturer";

            if (ModelState.IsValid)
            {
                await _tableServices.AddEntityAsync(userProfile);
                return RedirectToAction("LecturerLogin");
            }
            return View(userProfile);
        }

        public IActionResult AddClaim()
        {
            return View();
        }

        public async Task<IActionResult> StatusView()
        {
            var claims = await _tableServices.GetAllClaims();
            return View(claims);
        }

        [HttpPost]
        public async Task<IActionResult> AddClaim(ClaimsModel claim, IFormFile SupportingDocument)
        {
            claim.Status = "Pending"; // Set the claim status to Pending
            claim.LecturerID = _httpContextAccessor.HttpContext.Session.GetString("UserID");

            if (SupportingDocument != null && SupportingDocument.Length > 0)
            {
                // Upload the file to Azure File Share
                var fileUrl = await _fileService.UploadFileAsync(SupportingDocument.FileName, SupportingDocument.OpenReadStream());
                claim.SupportingDocumentUrl = fileUrl; // Store the file URL in the claim
            }

            // Add the claim to the table storage
            await _tableClaimsServices.AddClaim(claim);

            return RedirectToAction("StatusView");
           
            return View(claim);
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            
            if (ModelState.IsValid)
            {
                var user = await _tableServices.GetUserByEmailAsync(loginModel.Email);
                if (user != null && user.Password == loginModel.Password)
                {
                    if (user.Role == "Lecturer")
                    {
                        _httpContextAccessor.HttpContext.Session.SetString("UserID", user.RowKey);
                        // Redirect to Lecturer's dashboard
                        return RedirectToAction("AddClaim");
                    }
                    else
                    {
                        ModelState.AddModelError("", "You are not authorized to access the lecturer dashboard.");
                        return View(loginModel);
                    }
                }
                ModelState.AddModelError("", "Invalid login attempt.");
            }
            return View(loginModel);
        }
    }
}
