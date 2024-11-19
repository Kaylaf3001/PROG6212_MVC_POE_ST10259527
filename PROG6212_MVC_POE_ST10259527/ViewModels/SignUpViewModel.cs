namespace PROG6212_MVC_POE_ST10259527.ViewModels
{
    public class SignUpViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // Changed from bool IsAdmin to string Role
    }
}
