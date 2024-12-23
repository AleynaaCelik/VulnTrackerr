using System;

namespace VulnTracker.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid(); // Benzersiz kullanıcı kimliği
        public string Username { get; set; } // Kullanıcı adı
        public string PasswordHash { get; set; } // Şifre (hashlenmiş)
        public string Email { get; set; } // Kullanıcı e-posta adresi

        // İki faktörlü kimlik doğrulama (2FA)
        public bool IsTwoFactorEnabled { get; set; } // 2FA aktif mi?
        public string? SecretKey { get; set; } // 2FA için Secret Key (Totp ile kullanılır)
        public string? TwoFactorCode { get; set; } // 2FA için geçici kod
        public DateTime? TwoFactorExpiry { get; set; } // Kodun geçerlilik süresi
    }
}
