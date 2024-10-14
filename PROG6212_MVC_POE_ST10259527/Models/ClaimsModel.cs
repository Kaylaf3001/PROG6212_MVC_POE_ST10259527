using Azure.Data.Tables;
using Azure;
using Microsoft.AspNetCore.Mvc;

namespace PROG6212_MVC_POE_ST10259527.Models
{
    public class ClaimsModel : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public int Id { get; set; }
        public string LecturerName { get; set; }
        public double HoursWorked { get; set; }
        public double HourlyRate { get; set; }
        public string Status { get; set; }
        public string SupportingDocumentUrl { get; set; } // New field for file URL
        public ClaimsModel()
        {
            PartitionKey = "Claims";
            RowKey = Guid.NewGuid().ToString();
        }
    }
}