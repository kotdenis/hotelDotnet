using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HotelApp.Api.Healthcheck
{
    /// <summary>
    /// Liveness healthcheck
    /// </summary>
    public class LivenessCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(HealthCheckResult.Healthy("Healthy"));
        }
    }
}
