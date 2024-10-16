using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using PROG6212_MVC_POE_ST10259527.Models;
using System.Collections.Generic;

namespace PROG6212_MVC_POE_ST10259527.Test
{
    [TestClass]
    public class UserProfileModelTest
    {
        private List<UserProfileModel> _dummyDatabase;

        [TestInitialize]
        public void Setup()
        {
            _dummyDatabase = new List<UserProfileModel>
            {
                new UserProfileModel("John", "Doe", "john.doe@example.com", "password123", false),
                new UserProfileModel("Jane", "Smith", "jane.smith@example.com", "password456", true)
            };
        }

        [TestMethod]
        public void TestLoginUser_ValidCredentials_ReturnsUser()
        {
            // Arrange
            var loginViewModel = new LoginViewModel
            {
                Email = "john.doe@example.com",
                Password = "password123"
            };

            // Act
            var result = UserProfileModel.LoginUser(_dummyDatabase, loginViewModel);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("John", result.FirstName);
            Assert.AreEqual("Doe", result.LastName);
        }

        [TestMethod]
        public void TestLoginUser_InvalidCredentials_ReturnsNull()
        {
            // Arrange
            var loginViewModel = new LoginViewModel
            {
                Email = "john.doe@example.com",
                Password = "wrongpassword"
            };

            // Act
            var result = UserProfileModel.LoginUser(_dummyDatabase, loginViewModel);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestLoginUser_NonExistentUser_ReturnsNull()
        {
            // Arrange
            var loginViewModel = new LoginViewModel
            {
                Email = "nonexistent@example.com",
                Password = "password123"
            };

            // Act
            var result = UserProfileModel.LoginUser(_dummyDatabase, loginViewModel);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestLoginUser_NullInput_ReturnsNull()
        {
            // Act
            var result = UserProfileModel.LoginUser(_dummyDatabase, null);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestSignUpUser_ValidData_ReturnsNewUser()
        {
            // Arrange
            var signUpViewModel = new SignUpViewModel
            {
                FirstName = "Alice",
                LastName = "Johnson",
                Email = "alice.johnson@example.com",
                Password = "password789",
                IsAdmin = false
            };

            // Act
            var result = UserProfileModel.SignUpUser(signUpViewModel);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Alice", result.FirstName);
            Assert.AreEqual("Johnson", result.LastName);
            Assert.AreEqual("alice.johnson@example.com", result.Email);
            Assert.AreEqual("password789", result.Password);
            Assert.IsFalse(result.IsAdmin);

            // Add to dummy database
            _dummyDatabase.Add(result);

            // Verify the user is added to the dummy database
            var addedUser = _dummyDatabase.Find(user => user.Email == "alice.johnson@example.com");
            Assert.IsNotNull(addedUser);
            Assert.AreEqual("Alice", addedUser.FirstName);
            Assert.AreEqual("Johnson", addedUser.LastName);
            Assert.AreEqual("alice.johnson@example.com", addedUser.Email);
            Assert.AreEqual("password789", addedUser.Password);
            Assert.IsFalse(addedUser.IsAdmin);
        }

        [TestMethod]
        public void TestSignUpUser_AsAdmin_ReturnsNewAdminUser()
        {
            // Arrange
            var signUpViewModel = new SignUpViewModel
            {
                FirstName = "Bob",
                LastName = "Brown",
                Email = "bob.brown@example.com",
                Password = "adminpassword",
                IsAdmin = true
            };

            // Act
            var result = UserProfileModel.SignUpUser(signUpViewModel);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Bob", result.FirstName);
            Assert.AreEqual("Brown", result.LastName);
            Assert.AreEqual("bob.brown@example.com", result.Email);
            Assert.AreEqual("adminpassword", result.Password);
            Assert.IsTrue(result.IsAdmin);

            // Add to dummy database
            _dummyDatabase.Add(result);

            // Verify the user is added to the dummy database
            var addedUser = _dummyDatabase.Find(user => user.Email == "bob.brown@example.com");
            Assert.IsNotNull(addedUser);
            Assert.AreEqual("Bob", addedUser.FirstName);
            Assert.AreEqual("Brown", addedUser.LastName);
            Assert.AreEqual("bob.brown@example.com", addedUser.Email);
            Assert.AreEqual("adminpassword", addedUser.Password);
            Assert.IsTrue(addedUser.IsAdmin);
        }
    }
}
