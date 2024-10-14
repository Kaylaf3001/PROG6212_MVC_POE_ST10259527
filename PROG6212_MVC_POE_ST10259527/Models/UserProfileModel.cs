using Azure;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc;
using System;


namespace PROG6212_MVC_POE_ST10259527.Models
{
    public class UserProfileModel : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        // Custom properties
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public UserProfileModel()
        {
            PartitionKey = "User";
            RowKey = Guid.NewGuid().ToString();
        }
    }
}
