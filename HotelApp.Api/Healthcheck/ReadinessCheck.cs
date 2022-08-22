using HotelApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HotelApp.Api.Healthcheck
{
    /// <summary>
    /// Health check for database
    /// </summary>
    public class ReadinessCheck : IHealthCheck
    {
        private readonly HotelDbContext _dbContext;

        public ReadinessCheck(HotelDbContext scalesDbContext) => _dbContext = scalesDbContext;
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            _ = await _dbContext.Database.ExecuteSqlInterpolatedAsync($"select 1;", cancellationToken);
            return HealthCheckResult.Healthy("Database is working");
        }
    }
}
