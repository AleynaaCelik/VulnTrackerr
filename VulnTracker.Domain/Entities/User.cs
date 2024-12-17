using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VulnTracker.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public bool IsTwoFactorEnabled { get; set; }
        public string? TwoFactorCode { get; set; } // 2FA için geçici kod
        public DateTime? TwoFactorExpiry { get; set; } // Kodun geçerlilik süresi
    }
}
