

namespace NiceTasteApplication.DTOs.AuthDtos
{
    public class UserPreferenceDto
    {
        public bool EmailNotifications { get; set; } = true;
        public bool SmsNotifications { get; set; } = true;
        public Dictionary<string, string> Preferences { get; set; } = new();
    }
}
