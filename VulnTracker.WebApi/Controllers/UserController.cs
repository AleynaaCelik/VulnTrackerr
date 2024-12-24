using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VulnTracker.DataAccess.Context;
using VulnTracker.Domain.Entities;

namespace VulnTracker.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly VulnTrackerDbContext _context;

        public UserController(VulnTrackerDbContext context)
        {
            _context = context;
        }

        // Kullanıcı Listeleme
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        // Kullanıcı Detay
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound("User not found");

            return Ok(user);
        }

        // Kullanıcı Oluşturma
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User userModel)
        {
            if (await _context.Users.AnyAsync(u => u.Username == userModel.Username))
                return BadRequest("Username already exists");

            userModel.Id = Guid.NewGuid();
            userModel.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userModel.PasswordHash);

            _context.Users.Add(userModel);
            await _context.SaveChangesAsync();

            return Ok("User created successfully");
        }

        // Kullanıcı Güncelleme
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] User updatedUser)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound("User not found");

            user.Username = updatedUser.Username ?? user.Username;
            user.Email = updatedUser.Email ?? user.Email;

            if (!string.IsNullOrEmpty(updatedUser.PasswordHash))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(updatedUser.PasswordHash);
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok("User updated successfully");
        }

        // Kullanıcı Silme
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound("User not found");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok("User deleted successfully");
        }
    }
}

