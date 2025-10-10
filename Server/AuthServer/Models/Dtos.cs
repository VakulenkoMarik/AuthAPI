namespace Server.Models
{
    public class Dtos
    {
        public class RegisterRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string ConfirmPassword { get; set; }

            public RegisterRequest() { }

            public RegisterRequest(string username, string password, string confirmPassword)
            {
                Username = username;
                Password = password;
                ConfirmPassword = confirmPassword;
            }
        }

        public class LoginRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }

            public LoginRequest() { }

            public LoginRequest(string username, string password)
            {
                Username = username;
                Password = password;
            }
        }

        public class AuthResponse
        {
            public bool Success { get; set; }
            public string Message { get; set; }

            public AuthResponse() { }

            public AuthResponse(bool success, string message)
            {
                Success = success;
                Message = message;
            }
        }
    }
}