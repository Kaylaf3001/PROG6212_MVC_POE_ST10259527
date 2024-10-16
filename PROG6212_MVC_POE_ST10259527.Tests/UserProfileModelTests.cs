using PROG6212_MVC_POE_ST10259527.Models;
using PROG6212_MVC_POE_ST10259527.ViewModels;

namespace PROG6212_MVC_POE_ST10259527.Tests
{
    public class UserProfileModelTests
    {
        //-----------------------------------------------------------------------------------------------------
        // Dummy database for testing
        //-----------------------------------------------------------------------------------------------------
        private List<UserProfileModel> _dummyDatabase;

        public UserProfileModelTests()
        {
            _dummyDatabase = new List<UserProfileModel>
                {
                    new UserProfileModel("John", "Doe", "john.doe@example.com", "password123", false),
                    new UserProfileModel("Jane", "Smith", "jane.smith@example.com", "password456", true)
                };
        }
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        // Test LoginUser method
        //-----------------------------------------------------------------------------------------------------
        [Fact]
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
            Assert.NotNull(result);
            Assert.Equal("John", result.FirstName);
            Assert.Equal("Doe", result.LastName);
        }
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        // Test LoginUser method with invalid credentials
        //-----------------------------------------------------------------------------------------------------
        [Fact]
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
            Assert.Null(result);
        }
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        // Test LoginUser method with non-existent user
        //-----------------------------------------------------------------------------------------------------
        [Fact]
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
            Assert.Null(result);
        }
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        // Test SignUpUser method
        //-----------------------------------------------------------------------------------------------------
        [Fact]
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
            Assert.NotNull(result);
            Assert.Equal("Alice", result.FirstName);
            Assert.Equal("Johnson", result.LastName);
            Assert.Equal("alice.johnson@example.com", result.Email);
            Assert.Equal("password789", result.Password);
            Assert.False(result.IsAdmin);

            // Add to dummy database
            _dummyDatabase.Add(result);

            // Verify the user is added to the dummy database
            var addedUser = _dummyDatabase.Find(user => user.Email == "alice.johnson@example.com");
            Assert.NotNull(addedUser);
            Assert.Equal("Alice", addedUser.FirstName);
            Assert.Equal("Johnson", addedUser.LastName);
            Assert.Equal("alice.johnson@example.com", addedUser.Email);
            Assert.Equal("password789", addedUser.Password);
            Assert.False(addedUser.IsAdmin);
        }
        //-----------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------
        // Test SignUpUser method as admin
        //-----------------------------------------------------------------------------------------------------
        [Fact]
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
            Assert.NotNull(result);
            Assert.Equal("Bob", result.FirstName);
            Assert.Equal("Brown", result.LastName);
            Assert.Equal("bob.brown@example.com", result.Email);
            Assert.Equal("adminpassword", result.Password);
            Assert.True(result.IsAdmin);

            // Add to dummy database
            _dummyDatabase.Add(result);

            // Verify the user is added to the dummy database
            var addedUser = _dummyDatabase.Find(user => user.Email == "bob.brown@example.com");
            Assert.NotNull(addedUser);
            Assert.Equal("Bob", addedUser.FirstName);
            Assert.Equal("Brown", addedUser.LastName);
            Assert.Equal("bob.brown@example.com", addedUser.Email);
            Assert.Equal("adminpassword", addedUser.Password);
            Assert.True(addedUser.IsAdmin);
        }
        //-----------------------------------------------------------------------------------------------------
    }
}
//-----------------------------------------------End-Of-File----------------------------------------------------