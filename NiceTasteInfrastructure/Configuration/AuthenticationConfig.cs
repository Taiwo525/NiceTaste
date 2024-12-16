namespace LocalServicesMarketplace.Identity.API.Infrastructure.Configuration
{
    public class AuthenticationConfig
    {
        public string Authority { get; set; }
        public string Audience { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string[] AllowedOrigins { get; set; }
        public int TokenExpirationMinutes { get; set; } = 60;
        public int RefreshTokenExpirationDays { get; set; } = 30;

        public Dictionary<string, ExternalProviderConfig> ExternalProviders { get; set; }
    }

    public class ExternalProviderConfig
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AuthorizationEndpoint { get; set; }
        public string TokenEndpoint { get; set; }
        public string UserInfoEndpoint { get; set; }
        public string[] Scopes { get; set; }
    }
}
