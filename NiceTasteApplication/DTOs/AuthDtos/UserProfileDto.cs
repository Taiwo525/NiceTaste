

namespace NiceTasteApplication.DTOs.AuthDtos
{
    internal class UserProfileDto
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
}
