using Microsoft.AspNetCore.Mvc;

namespace PROG6212_MVC_POE_ST10259527.Models
{
    public class SupportingDocumentModel
    {
        public int Id { get; set; }
        public int ClaimId { get; set; }
        public string FilePath { get; set; }
    }
}
