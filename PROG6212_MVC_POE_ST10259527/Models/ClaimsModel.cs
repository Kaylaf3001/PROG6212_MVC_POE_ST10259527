using Azure.Data.Tables;
using Azure;

namespace PROG6212_MVC_POE_ST10259527.Models
{
    public class ClaimsModel : ITableEntity
    {
        private double? totalAmount;

        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public string LecturerID { get; set; }
        public double HoursWorked { get; set; }
        public double HourlyRate { get; set; }
        public string Status { get; set; }
        public string? SupportingDocumentUrl { get; set; }

        public ClaimsModel()
        {
            PartitionKey = "Claims";
            RowKey = Guid.NewGuid().ToString();
        }

        //-----------------------------------------------------------------------------------------------------
        // Test
        // Calculate the total amount of the claim
        //-----------------------------------------------------------------------------------------------------
        public double CalculateTotalAmount()
        {
            totalAmount = HoursWorked * HourlyRate;
            return totalAmount.Value;
        }
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        //Test
        // Approve the claim
        //-----------------------------------------------------------------------------------------------------
        public static ClaimsModel ApproveClaim(ClaimsModel claim)
        {
            claim.Status = "Approved";
            return claim;
        }
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        //Test
        // Reject the claim
        //-----------------------------------------------------------------------------------------------------
        public static ClaimsModel RejectClaim(ClaimsModel claim)
        {
            claim.Status = "Rejected";
            return claim;
        }
        //-----------------------------------------------------------------------------------------------------
    }
}