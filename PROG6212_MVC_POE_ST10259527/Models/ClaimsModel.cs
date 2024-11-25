﻿using Azure.Data.Tables;
using Azure;
using System.ComponentModel.DataAnnotations.Schema;


namespace PROG6212_MVC_POE_ST10259527.Models
{
    public class ClaimsModel
    {
        public int ClaimID { get; set; }
        public int userID { get; set; }
        public string LecturerName { get; set; }
        public string ModuleCode { get; set; }
        public DateTime Date
        {
            get => _date;
            set => _date = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }
        private DateTime _date;
        public double HoursWorked { get; set; }
        public double HourlyRate { get; set; }
        public string Status { get; set; }
        public string? SupportingDocumentName { get; set; }
        public bool IsValid { get; set; } // Indicates if the claim meets the policy
        public bool IsPaid { get; set; } // Indicates if the claim has been paid

        
        public double TotalAmount { get; set; }

        public ClaimsModel()
        {
        }

        //-----------------------------------------------------------------------------------------------------
        // Calculate the total amount of the claim
        //-----------------------------------------------------------------------------------------------------
        public double CalculateTotalAmount()
        {
            TotalAmount = HoursWorked * HourlyRate;
            return TotalAmount;
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

        //-----------------------------------------------------------------------------------------------------
        // Mark the claim as paid
        //-----------------------------------------------------------------------------------------------------
        public static ClaimsModel MarkAsPaid(ClaimsModel claim)
        {
            claim.IsPaid = true;
            return claim;
        }
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        //Checks if the claim is valid
        //-----------------------------------------------------------------------------------------------------
        public bool IsValidClaim()
        {
            if (HoursWorked > 8)
            {
                return false;
            }
            var dayDifference = (DateTime.Now - Date).Days;
            if (dayDifference > 90)
            {
                return false;
            }
            if (Date > DateTime.Now)
            {
                return false;
            }
            return true;
        }
        //-----------------------------------------------------------------------------------------------------
    }
}
//-----------------------------------------------End-Of-File----------------------------------------------------