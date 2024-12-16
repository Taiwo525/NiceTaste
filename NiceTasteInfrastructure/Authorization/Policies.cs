using Microsoft.AspNetCore.Authorization;

namespace LocalServicesMarketplace.Identity.API.Infrastructure.Authorization
{
    public static class Policies
    {
        public const string RequireAdminRole = "RequireAdminRole";
        public const string RequireModeratorRole = "RequireModeratorRole";
        public const string RequireVerifiedUser = "RequireVerifiedUser";
        public const string RequireActiveAccount = "RequireActiveAccount";

        public static void ConfigureAuthorizationPolicies(AuthorizationOptions options)
        {
            options.AddPolicy(RequireAdminRole, policy =>
                policy.RequireClaim("role", "admin"));

            options.AddPolicy(RequireModeratorRole, policy =>
                policy.RequireClaim("role", "admin", "moderator"));

            options.AddPolicy(RequireVerifiedUser, policy =>
                policy.RequireAssertion(context =>
                    context.User.HasClaim(c => c.Type == "email_verified" && c.Value == "true")));

            options.AddPolicy(RequireActiveAccount, policy =>
                policy.RequireAssertion(context =>
                    context.User.HasClaim(c => c.Type == "account_status" && c.Value == "Active")));
        }
    }
}
