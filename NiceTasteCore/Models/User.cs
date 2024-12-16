using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceTasteCore.Models
{
    public class User
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
        public UserProfile Profile { get; set; } = new();
        public UserPreferences Preferences { get; set; } = new();
        public List<ExternalLogin> ExternalLogins { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        //public DateTime? UpdatedAt { get; set; }
        //public DateTime? DeletedAt { get; set; }
        //public bool MarketingConsent { get; set; }
        //public bool DataProcessingConsent { get; set; }
        //public string? VerificationToken { get; set; }
        //public DateTime? VerificationTokenExpiryTime { get; set; }
        //public string? PasswordResetToken { get; set; }
        //public DateTime? PasswordResetTokenExpiryTime { get; set; }
    }

    public class UserProfile
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? DisplayName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Avatar { get; set; }
        public string? Bio { get; set; }
        public Address? Address { get; set; }
        public Dictionary<string, string> CustomFields { get; set; } = new();
    }

    public class Address
    {
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
    }

    public class UserPreferences
    {
        public bool EmailNotifications { get; set; } = true;
        public bool SmsNotifications { get; set; } = true;
        public Dictionary<string, string> Preferences { get; set; } = new();
    }

    public class ExternalLogin
    {
        public string Provider { get; set; } = string.Empty;
        public string ProviderKey { get; set; } = string.Empty;
        public DateTime ConnectedAt { get; set; }
        public Dictionary<string, string> ProviderMetadata { get; set; } = new();
    }
    
}
