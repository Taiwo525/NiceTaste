using Prometheus;
using System.Diagnostics;

namespace LocalServicesMarketplace.Identity.API.Infrastructure.Metrics
{
    public static class MetricsExtensions
    {
        private static readonly Counter AuthAttempts = Metrics
            .CreateCounter("identity_auth_attempts_total", "Number of authentication attempts",
                new CounterConfiguration
                {
                    LabelNames = new[] { "status", "method" }
                });

        private static readonly Counter UserOperations = Metrics
            .CreateCounter("identity_user_operations_total", "Number of user operations",
                new CounterConfiguration
                {
                    LabelNames = new[] { "operation", "status" }
                });

        private static readonly Histogram RequestDuration = Metrics
            .CreateHistogram("identity_request_duration_seconds", "Request duration in seconds",
                new HistogramConfiguration
                {
                    LabelNames = new[] { "endpoint", "method" },
                    Buckets = new[] { 0.1, 0.25, 0.5, 1, 2.5, 5, 10 }
                });

        private static readonly Gauge ActiveUsers = Metrics
            .CreateGauge("identity_active_users", "Number of active users");

        public static IApplicationBuilder UseCustomMetrics(this IApplicationBuilder app)
        {
            return app.UseMiddleware<MetricsMiddleware>();
        }

        public static void RecordAuthAttempt(string status, string method)
        {
            AuthAttempts.WithLabels(status, method).Inc();
        }

        public static void RecordUserOperation(string operation, string status)
        {
            UserOperations.WithLabels(operation, status).Inc();
        }

        public static void RecordRequestDuration(string endpoint, string method, double durationSeconds)
        {
            RequestDuration.WithLabels(endpoint, method).Observe(durationSeconds);
        }

        public static void SetActiveUsers(int count)
        {
            ActiveUsers.Set(count);
        }
    }

    public class MetricsMiddleware
    {
        private readonly RequestDelegate _next;

        public MetricsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value;
            var method = context.Request.Method;

            var sw = Stopwatch.StartNew();

            try
            {
                await _next(context);
            }
            finally
            {
                sw.Stop();
                MetricsExtensions.RecordRequestDuration(
                    path ?? "",
                    method,
                    sw.Elapsed.TotalSeconds);
            }
        }
    }

    public class MetricsService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<MetricsService> _logger;
        private Timer? _timer;

        public MetricsService(
            IServiceProvider serviceProvider,
            ILogger<MetricsService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(CollectMetrics, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        private async void CollectMetrics(object? state)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

                // Update active users metric
                var activeUsers = await userRepository.GetActiveUsersCountAsync();
                MetricsExtensions.SetActiveUsers(activeUsers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error collecting metrics");
            }
        }
    }
}
