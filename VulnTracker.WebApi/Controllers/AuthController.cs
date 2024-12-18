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
            // Kullanıcıyı veritabanından bul
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
                return NotFound("User not found");

            // Secret key oluştur
            var secretKey = TwoFactorAuthHelper.GenerateSecretKey();
            user.SecretKey = secretKey; // Secret key'i kullanıcıya ata
            user.Is2FAEnabled = true;   // 2FA özelliğini etkinleştir

            // QR kodu için URI oluştur
            var qrCodeUri = TwoFactorAuthHelper.GenerateOtpUri(user.Email, secretKey);

            // QR kodunu base64 formatında oluştur
            var qrCodeBitmap = TwoFactorAuthHelper.GenerateQrCode(qrCodeUri);
            string qrCodeBase64;

            using (var ms = new MemoryStream())
            {
                qrCodeBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                qrCodeBase64 = Convert.ToBase64String(ms.ToArray());
            }

            // Kullanıcıyı güncelle
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            // QR kodunu base64 formatında geri gönder
            return Ok(new { QrCodeImage = qrCodeBase64 });
        }

        // Verify 2FA Endpoint
        [HttpPost("verify-2fa")]
        public async Task<IActionResult> Verify2FA([FromBody] TwoFactorVerifyRequest request)
        {
            // Kullanıcıyı veritabanından bul
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user == null)
                return NotFound("User not found");

            // 2FA doğrulamasını kontrol et
            if (user.Is2FAEnabled && TwoFactorAuthHelper.ValidateOTP(user.SecretKey, request.OtpCode))
            {
                // 2FA doğrulandı, JWT token oluşturulabilir
                var token = JwtTokenHelper.GenerateToken(user, _config);
                return Ok(new { Token = token });
            }

            return Unauthorized("Invalid 2FA code.");
        }
    }

    // DTO sınıfı: Verify2FA endpointine gelen istek için
    public class TwoFactorVerifyRequest
    {
        public string Username { get; set; }
        public string OtpCode { get; set; }
    }
}
