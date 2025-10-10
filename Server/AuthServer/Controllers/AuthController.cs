using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services;

namespace AuthServer.Controllers
{
    [ApiController]
    [Route("/")]
    public class AuthController : ControllerBase
    {
        [HttpPost("register")]
        public IActionResult Register([FromBody] Dtos.RegisterRequest request)
        {
            if (request.Password != request.ConfirmPassword)
                return BadRequest(new Dtos.AuthResponse(false, "Passwords do not match"));

            if (!UserStore.TryAddUser(request.Username, request.Password))
                return BadRequest(new Dtos.AuthResponse(false, "User already exists"));

            return Ok(new Dtos.AuthResponse(true, "Registration successful"));
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] Dtos.LoginRequest request)
        {
            bool isValid = UserStore.ValidateUser(request.Username, request.Password);
            if (!isValid)
                return BadRequest(new Dtos.AuthResponse(false, "Invalid username or password"));

            return Ok(new Dtos.AuthResponse(true, "Login successful"));
        }
    }
}