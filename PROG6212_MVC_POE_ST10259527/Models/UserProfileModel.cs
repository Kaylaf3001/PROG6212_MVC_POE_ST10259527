using Azure;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc;
using PROG6212_MVC_POE_ST10259527.ViewModels;
using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace PROG6212_MVC_POE_ST10259527.Models
{
    public class UserProfileModel 
    {
        // Custom properties
        public int userID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string Role { get; set; } // Changed from bool IsAdmin to string Role

        // Parameterless constructor
        public UserProfileModel()
        {  
        }

        // Constructor with parameters
        public UserProfileModel(string firstName, string lastName, string email, string password, string role)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            Role = role;
        }

        //-----------------------------------------------------------------------------------------------------
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
        // Sign up a user
        //-----------------------------------------------------------------------------------------------------
        public static UserProfileModel SignUpUser(SignUpViewModel tryNewUser)
        {
            var newUser = new UserProfileModel(tryNewUser.FirstName, tryNewUser.LastName, tryNewUser.Email, tryNewUser.Password, tryNewUser.Role);
            return newUser;
        }
        //-----------------------------------------------------------------------------------------------------
    }
}
//-----------------------------------------------End-Of-File----------------------------------------------------