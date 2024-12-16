using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceTasteApplication.DTOs.AuthDtos
{
    internal class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string Role { get; set; }
        public DateTime DateRegister { get; set; } = DateTime.UtcNow;
        public bool TwoFactorEnabled { get; set; }
        public string? TwoFactorSecret { get; set; }
        public UserProfileDto Profile { get; set; } = new();
        public UserPreferenceDto Preferences { get; set; } = new();
        public List<ExternalLoginDto> ExternalLogins { get; set; } = new();
        public DateTime CreatedAt { get; set; }
    }
}
