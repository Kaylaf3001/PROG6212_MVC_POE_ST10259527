using Azure;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc;
using PROG6212_MVC_POE_ST10259527.ViewModels;
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
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool IsAdmin { get; set; } = false;

        // Parameterless constructor
        public UserProfileModel()
        {
            PartitionKey = "User";
            RowKey = Guid.NewGuid().ToString();
        }

        // Constructor with parameters
        public UserProfileModel(string firstName, string lastName, string email, string password, bool isAdmin)
        {
            PartitionKey = "User";
            RowKey = Guid.NewGuid().ToString();
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            IsAdmin = isAdmin;
        }

        //-----------------------------------------------------------------------------------------------------
        //Test 
        // Login a user
        //-----------------------------------------------------------------------------------------------------
        public static UserProfileModel LoginUser(List<UserProfileModel> listOfUsers, LoginViewModel tryUser)
        {
            foreach (var user in listOfUsers)
            {
                if (user.Email == tryUser.Email && user.Password == tryUser.Password)
                {
                    return user;
                }
            }
            return null;
        }
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        //Test
        // Sign up a user
        //-----------------------------------------------------------------------------------------------------
        public static UserProfileModel SignUpUser(SignUpViewModel tryNewUser)
        {
            var newUser = new UserProfileModel(tryNewUser.FirstName, tryNewUser.LastName, tryNewUser.Email, tryNewUser.Password, tryNewUser.IsAdmin);
            return newUser;
        }
        //-----------------------------------------------------------------------------------------------------
    }
}
//-----------------------------------------------End-Of-File----------------------------------------------------