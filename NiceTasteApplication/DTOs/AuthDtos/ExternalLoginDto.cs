

namespace NiceTasteApplication.DTOs.AuthDtos
{
    internal class ExternalLoginDto
    {
        public string Provider { get; set; } = string.Empty;
        public string ProviderKey { get; set; } = string.Empty;
        public DateTime ConnectedAt { get; set; }
        public Dictionary<string, string> ProviderMetadata { get; set; } = new();
    }
}
