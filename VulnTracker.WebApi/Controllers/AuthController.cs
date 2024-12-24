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

        // Enable 2FA Endpoint
        [HttpPost("enable-2fa")]
        public async Task<IActionResult> Enable2FA([FromBody] string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
                return NotFound("User not found");

            var secretKey = TwoFactorAuthHelper.GenerateSecretKey();
            user.SecretKey = secretKey;
            user.IsTwoFactorEnabled = true;

            var qrCodeUri = TwoFactorAuthHelper.GenerateOtpUri(user.Email, secretKey);
            var qrCodeBitmap = TwoFactorAuthHelper.GenerateQrCode(qrCodeUri);
            string qrCodeBase64;

            using (var ms = new MemoryStream())
            {
                qrCodeBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                qrCodeBase64 = Convert.ToBase64String(ms.ToArray());
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok(new { QrCodeImage = qrCodeBase64 });
        }

        // Verify 2FA Endpoint
        [HttpPost("verify-2fa")]
        public async Task<IActionResult> Verify2FA([FromBody] TwoFactorVerifyRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user == null)
                return NotFound("User not found");

            if (user.IsTwoFactorEnabled && TwoFactorAuthHelper.ValidateOTP(user.SecretKey, request.OtpCode))
            {
                var token = JwtTokenHelper.GenerateToken(user, _config);
                return Ok(new { Token = token });
            }

            return Unauthorized("Invalid 2FA code.");
        }

        // Validate OTP Endpoint
        [HttpPost("validate-otp")]
        public async Task<IActionResult> ValidateOTP([FromBody] OTPValidationRequest request)
        {
            // Kullanıcıyı kimlik doğrulama yoluyla (JWT) veya test için sabit bir UserId ile al
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user == null)
                return NotFound("User not found");

            if (!user.IsTwoFactorEnabled)
                return BadRequest("Two-factor authentication is not enabled.");

            // OTP doğrulama işlemi
            var isValid = TwoFactorAuthHelper.ValidateOTP(user.SecretKey, request.OTP);
            if (!isValid)
                return BadRequest("Invalid OTP.");

            return Ok(new { Message = "OTP validated successfully." });
        }
    }

    // DTO sınıfları
    public class TwoFactorVerifyRequest
    {
        public string Username { get; set; }
        public string OtpCode { get; set; }
    }

    public class OTPValidationRequest
    {
        public string Username { get; set; }
        public string OTP { get; set; }
    }
}
