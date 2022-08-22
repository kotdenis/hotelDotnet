using FluentValidation;
using HotelApp.Core.Models;
using HotelApp.Core.Options;
using HotelApp.Core.Services.Implementation;
using HotelApp.Core.Services.Interfaces;
using HotelApp.Core.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotelApp.Core.Configuration
{
    /// <summary>
    /// Sets dependency injections for services and options
    /// </summary>
    public static class ServiceConfiguration
    {
        public static void AddServices(this IServiceCollection service)
        {
            service.AddScoped<ITokenService, TokenService>();
            service.AddScoped<IUserService, UserService>();
            service.AddScoped<IUserDashboardService, UserDashboardService>();
            service.AddScoped<IMailService, MailService>();
            service.AddScoped<IAdminService, AdminService>();
            service.AddScoped<ICacheManager, DistributedCacheManager>();
            service.AddScoped<IValidator<RegisterModel>, RegisterValidator>();
            service.AddScoped<IValidator<LoginModel>, LoginValidator>();
            service.AddScoped<IValidator<RoomSearchModel>, RoomSearchModelValidator>();
        }

        public static void AddConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.Jwt));
            services.Configure<MailOptions>(configuration.GetSection(MailOptions.MailSettings));
        }
    }
}
