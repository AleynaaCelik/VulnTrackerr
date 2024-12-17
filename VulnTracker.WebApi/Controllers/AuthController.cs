using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VulnTracker.Business.Helpers;
using VulnTracker.DataAccess.Context;
using VulnTracker.Domain.Entities;

namespace VulnTracker.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly VulnTrackerDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(VulnTrackerDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // Register Endpoint
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User userModel)
        {
            if (await _context.Users.AnyAsync(u => u.Username == userModel.Username))
                return BadRequest("Username already exists");

            var user = new User
            {
                Username = userModel.Username,
                Email = userModel.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userModel.PasswordHash) // Hashing password
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully");
        }

        // Login Endpoint
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User userModel)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == userModel.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(userModel.PasswordHash, user.PasswordHash))
                return Unauthorized("Invalid username or password");

            var token = JwtTokenHelper.GenerateToken(user, _config);
            return Ok(new { Token = token });
        }
    }
}

