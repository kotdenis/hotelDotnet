using HotelApp.Data.Repositories.Implementation;
using HotelApp.Data.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HotelApp.Data.Configuration
{
    /// <summary>
    /// Sets dependency injection for repositories
    /// </summary>
    public static class RepositoryConfiguration
    {
        public static void AddRepositories(this IServiceCollection service)
        {
            service.AddScoped<IGuestRepository, GuestRepository>();
            service.AddScoped<IRoomRepository, RoomRepository>();
            service.AddScoped<IRefreshRepository, RefreshRepository>();
            service.AddScoped<IUserRepository, UserRepository>();
            service.AddScoped<IBookRepository, BookRepository>();
            service.AddScoped<UnitOfWork>();
        }
    }
}
