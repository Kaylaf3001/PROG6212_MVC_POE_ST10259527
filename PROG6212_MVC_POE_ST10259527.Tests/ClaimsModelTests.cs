using PROG6212_MVC_POE_ST10259527.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG6212_MVC_POE_ST10259527.Tests
{
    public class ClaimsModelTests
    {
        //-----------------------------------------------------------------------------------------------------
        //Tests the calculate total amount method
        //-----------------------------------------------------------------------------------------------------
        [Fact]
        public void CalculateTotalAmount_ValidData_ReturnsCorrectAmount()
        {
            // Arrange
            var claim = new ClaimsModel
            {
                HoursWorked = 10,
                HourlyRate = 20
            };

            // Act
            var result = claim.CalculateTotalAmount();

            // Assert
            Assert.Equal(200, result);
        }
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        //Tests changing the status of a claim to approved
        //-----------------------------------------------------------------------------------------------------
        [Fact]
        public void ApproveClaim_ValidClaim_ChangesStatusToApproved()
        {
            // Arrange
            var claim = new ClaimsModel
            {
                Status = "Pending"
            };

            // Act
            var result = ClaimsModel.ApproveClaim(claim);

            // Assert
            Assert.Equal("Approved", result.Status);
        }
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        //Tests changing the status of a claim to rejected
        //-----------------------------------------------------------------------------------------------------
        [Fact]
        public void RejectClaim_ValidClaim_ChangesStatusToRejected()
        {
            // Arrange
            var claim = new ClaimsModel
            {
                Status = "Pending"
            };

            // Act
            var result = ClaimsModel.RejectClaim(claim);

            // Assert
            Assert.Equal("Rejected", result.Status);
        }
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        //Tests if the sorting works correctly
        //-----------------------------------------------------------------------------------------------------
        [Fact]
        public void GetAllApprovedClaims_ValidData_ReturnsOnlyApprovedClaims()
        {
            // Arrange
            var claims = new List<ClaimsModel>
                {
                    new ClaimsModel { Status = "Approved" },
                    new ClaimsModel { Status = "Rejected" },
                    new ClaimsModel { Status = "Approved" }
                };

            // Act
            var result = new ClaimsModel().GetAllAprovedClaims(claims);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, claim => Assert.Equal("Approved", claim.Status));
        }
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        //Tests if the sorting works correctly
        //-----------------------------------------------------------------------------------------------------
        [Fact]
        public void GetAllRejectedClaims_ValidData_ReturnsOnlyRejectedClaims()
        {
            // Arrange
            var claims = new List<ClaimsModel>
                {
                    new ClaimsModel { Status = "Rejected" },
                    new ClaimsModel { Status = "Approved" },
                    new ClaimsModel { Status = "Rejected" }
                };

            // Act
            var result = new ClaimsModel().GetAllRejectedClaims(claims);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, claim => Assert.Equal("Rejected", claim.Status));
        }
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        //Tests if the sorting works correctly
        //-----------------------------------------------------------------------------------------------------
        [Fact]
        public void GetAllPendingClaims_ValidData_ReturnsOnlyPendingClaims()
        {
            // Arrange
            var claims = new List<ClaimsModel>
                {
                    new ClaimsModel { Status = "Pending" },
                    new ClaimsModel { Status = "Approved" },
                    new ClaimsModel { Status = "Pending" }
                };

            // Act
            var result = new ClaimsModel().GetAllPendingClaims(claims);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, claim => Assert.Equal("Pending", claim.Status));
        }
        //-----------------------------------------------------------------------------------------------------
    }
}
//-----------------------------------------------End-Of-File----------------------------------------------------
