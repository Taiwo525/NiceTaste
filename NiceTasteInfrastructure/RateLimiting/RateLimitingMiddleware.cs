using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;

namespace LocalServicesMarketplace.Identity.API.Infrastructure.RateLimiting
{
    public static class RateLimitingExtensions
    {
        public static IServiceCollection AddCustomRateLimiting(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<RateLimitingSettings>(
                configuration.GetSection("RateLimiting"));

            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                // Global rate limiting
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                {
                    return RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: context.User?.Identity?.Name ?? context.Request.Headers.Host.ToString(),
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            AutoReplenishment = true,
                            PermitLimit = 100,
                            QueueLimit = 0,
                            Window = TimeSpan.FromMinutes(1)
                        });
                });

                // Authentication endpoints
                options.AddPolicy("auth", context =>
                {
                    return RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? context.Request.Headers.Host.ToString(),
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            AutoReplenishment = true,
                            PermitLimit = 10,
                            QueueLimit = 0,
                            Window = TimeSpan.FromMinutes(1)
                        });
                });

                // Profile endpoints
                options.AddPolicy("profile", context =>
                {
                    return RateLimitPartition.GetTokenBucketLimiter(
                        partitionKey: context.User?.Identity?.Name ?? context.Request.Headers.Host.ToString(),
                        factory: _ => new TokenBucketRateLimiterOptions
                        {
                            AutoReplenishment = true,
                            TokenLimit = 50,
                            QueueLimit = 0,
                            ReplenishmentPeriod = TimeSpan.FromSeconds(10),
                            TokensPerPeriod = 5
                        });
                });

                // Configure rate limit exceeded behavior
                options.OnRejected = async (context, token) =>
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    
                    if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                    {
                        await context.HttpContext.Response.WriteAsJsonAsync(new
                        {
                            error = "Too many requests",
                            retryAfter = retryAfter.TotalSeconds
                        }, token);
                    }
                };
            });

            return services;
        }
    }

    public class RateLimitingSettings
    {
        public GlobalLimitSettings Global { get; set; } = new();
        public AuthLimitSettings Auth { get; set; } = new();
        public ProfileLimitSettings Profile { get; set; } = new();
    }

    public class GlobalLimitSettings
    {
        public int PermitLimit { get; set; } = 100;
        public int WindowInMinutes { get; set; } = 1;
    }

    public class AuthLimitSettings
    {
        public int PermitLimit { get; set; } = 10;
        public int WindowInMinutes { get; set; } = 1;
    }

    public class ProfileLimitSettings
    {
        public int TokenLimit { get; set; } = 50;
        public int TokensPerPeriod { get; set; } = 5;
        public int ReplenishmentPeriodInSeconds { get; set; } = 10;
    }
}
