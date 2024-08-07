using Microsoft.AspNetCore.Mvc;

namespace PROG6212_MVC_POE_ST10259527.Models
{
    public class ClaimModel
    {
        public int Id { get; set; }
        public string LecturerName { get; set; }
        public double HoursWorked { get; set; }
        public double HourlyRate { get; set; }
        public string Status { get; set; }
    }
}