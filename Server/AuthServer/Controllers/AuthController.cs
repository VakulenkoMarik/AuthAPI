using Microsoft.AspNetCore.Mvc;
using AuthServer.Models;
using AuthServer.Services;

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
                return BadRequest(new Dtos.AuthResponse(false, "Passwords do not match", new()));

            if (!UserStore.TryAddUser(request.Username, request.Password))
                return BadRequest(new Dtos.AuthResponse(false, "User already exists", new()));

            var user = UserStore.ValidateUser(request.Username, request.Password);

            return Ok(new Dtos.AuthResponse(true, "Registration successful", new UserData
            {
                Username = user.Username,
                Password = user.Password,
                Coins = user.Coins
            }));
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] Dtos.LoginRequest request)
        {
            var user = UserStore.ValidateUser(request.Username, request.Password);
            if (user == null)
                return Unauthorized(new Dtos.AuthResponse(false, "Invalid username or password", new UserData()));

            var userData = new UserData
            {
                Username = user.Username,
                Coins = user.Coins
            };

            return Ok(new Dtos.AuthResponse(true, "Login successful", userData));
        }

        [HttpPost("updateCoins")]
        public IActionResult UpdateCoins([FromBody] Dtos.UpdateCoinsRequest request)
        {
            var user = UserStore.ValidateUserByName(request.Username);
            if (user == null)
                return NotFound(new { Message = "User not found" });

            user.Coins = request.Coins;
            UserStore.Save();
            return Ok(new { Success = true, Message = "Coins updated", Coins = user.Coins });
        }
    }
}