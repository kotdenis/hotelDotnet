using Hangfire;
using HotelApp.Jobs.Jobs;
using Microsoft.AspNetCore.Builder;

namespace HotelApp.Jobs.Extensions
{
    public static class HangfireExtension
    {
        public static void UseHangfire(this IApplicationBuilder app)
        {
            RecurringJob.AddOrUpdate<MailJob>((m) => m.SendMailAsync(JobCancellationToken.Null), Cron.Daily);
        }
    }
}
