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
        public string LecturerName { get; set; }
        public double HoursWorked { get; set; }
        public double HourlyRate { get; set; }
        public string Status { get; set; }
        public string? SupportingDocumentUrl { get; set; }
        public string? SupportingDocumentName { get; set; }

        public ClaimsModel()
        {
            PartitionKey = "Claims";
            RowKey = Guid.NewGuid().ToString();
        }

        //-----------------------------------------------------------------------------------------------------
        // Calculate the total amount of the claim
        //-----------------------------------------------------------------------------------------------------
        public double CalculateTotalAmount()
        {
            totalAmount = HoursWorked * HourlyRate;
            return totalAmount.Value;
        }
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        // Approve the claim
        //-----------------------------------------------------------------------------------------------------
        public static ClaimsModel ApproveClaim(ClaimsModel claim)
        {
            claim.Status = "Approved";
            return claim;
        }
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        // Reject the claim
        //-----------------------------------------------------------------------------------------------------
        public static ClaimsModel RejectClaim(ClaimsModel claim)
        {
            claim.Status = "Rejected";
            return claim;
        }

        //-----------------------------------------------------------------------------------------------------
        // Get all the approved claims
        //-----------------------------------------------------------------------------------------------------
        public List<ClaimsModel> GetAllAprovedClaims(List<ClaimsModel> listOfApprovedClaims)
        {
            List<ClaimsModel> approvedClaims = new List<ClaimsModel>();
            foreach (var claim in listOfApprovedClaims)
            {
                if (claim.Status == "Approved")
                {
                    approvedClaims.Add(claim);
                }
            }
            return approvedClaims;
        }
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        // Get all the approved claims
        //-----------------------------------------------------------------------------------------------------
        public List<ClaimsModel> GetAllRejectedClaims(List<ClaimsModel> listOfRejectedClaims)
        {
            List<ClaimsModel> rejectedClaims = new List<ClaimsModel>();
            foreach (var claim in listOfRejectedClaims)
            {
                if (claim.Status == "Rejected")
                {
                    rejectedClaims.Add(claim);
                }
            }
            return rejectedClaims;
        }
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        // Get all the pending claims
        //-----------------------------------------------------------------------------------------------------
        public List<ClaimsModel> GetAllPendingClaims(List<ClaimsModel> listOfPendingClaims)
        {
            List<ClaimsModel> pendingClaims = new List<ClaimsModel>();
            foreach (var claim in listOfPendingClaims)
            {
                if (claim.Status == "Pending")
                {
                    pendingClaims.Add(claim);
                }
            }
            return pendingClaims;
        }
        //-----------------------------------------------------------------------------------------------------
    }
}
//-----------------------------------------------End-Of-File----------------------------------------------------