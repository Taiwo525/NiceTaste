using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Timeout;
using Polly.Wrap;
using Microsoft.Extensions.Options;

namespace LocalServicesMarketplace.Identity.API.Infrastructure.Resilience
{
    public interface IResiliencePolicy
    {
        AsyncPolicyWrap<T> CreatePolicy<T>();
        IAsyncPolicy<T> CreateCircuitBreakerPolicy<T>();
        IAsyncPolicy<T> CreateRetryPolicy<T>();
        IAsyncPolicy<T> CreateTimeoutPolicy<T>();
    }

    public class ResiliencePolicy : IResiliencePolicy
    {
        private readonly ResilienceSettings _settings;
        private readonly ILogger<ResiliencePolicy> _logger;

        public ResiliencePolicy(
            IOptions<ResilienceSettings> settings,
            ILogger<ResiliencePolicy> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        public AsyncPolicyWrap<T> CreatePolicy<T>()
        {
            return Policy<T>
                .WrapAsync(
                    CreateCircuitBreakerPolicy<T>(),
                    CreateRetryPolicy<T>(),
                    CreateTimeoutPolicy<T>());
        }

        public IAsyncPolicy<T> CreateCircuitBreakerPolicy<T>()
        {
            return Policy<T>
                .Handle<Exception>()
                .CircuitBreakerAsync(
                    exceptionsAllowedBeforeBreaking: _settings.CircuitBreaker.ExceptionsAllowedBeforeBreaking,
                    durationOfBreak: TimeSpan.FromSeconds(_settings.CircuitBreaker.DurationOfBreakInSeconds),
                    onBreak: (exception, duration) =>
                    {
                        _logger.LogWarning(
                            exception,
                            "Circuit breaker opened for {Duration}s",
                            duration.TotalSeconds);
                    },
                    onReset: () =>
                    {
                        _logger.LogInformation("Circuit breaker reset");
                    },
                    onHalfOpen: () =>
                    {
                        _logger.LogInformation("Circuit breaker half-open");
                    });
        }

        public IAsyncPolicy<T> CreateRetryPolicy<T>()
        {
            return Policy<T>
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    retryCount: _settings.Retry.MaxRetries,
                    sleepDurationProvider: retryAttempt =>
                        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (exception, timeSpan, retryCount, context) =>
                    {
                        _logger.LogWarning(
                            exception,
                            "Retry {RetryCount} of {MaxRetries} after {DelayMs}ms",
                            retryCount,
                            _settings.Retry.MaxRetries,
                            timeSpan.TotalMilliseconds);
                    });
        }

        public IAsyncPolicy<T> CreateTimeoutPolicy<T>()
        {
            return Policy.TimeoutAsync<T>(
                seconds: _settings.Timeout.TimeoutInSeconds,
                onTimeoutAsync: async (context, timeSpan, task) =>
                {
                    _logger.LogWarning(
                        "Timeout after {TimeoutSeconds}s",
                        timeSpan.TotalSeconds);
                });
        }
    }

    public class ResilienceSettings
    {
        public CircuitBreakerSettings CircuitBreaker { get; set; } = new();
        public RetrySettings Retry { get; set; } = new();
        public TimeoutSettings Timeout { get; set; } = new();
    }

    public class CircuitBreakerSettings
    {
        public int ExceptionsAllowedBeforeBreaking { get; set; } = 5;
        public int DurationOfBreakInSeconds { get; set; } = 30;
    }

    public class RetrySettings
    {
        public int MaxRetries { get; set; } = 3;
    }

    public class TimeoutSettings
    {
        public int TimeoutInSeconds { get; set; } = 30;
    }
}
