using HotelApp.Data.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HotelApp.Data
{
    public static class DbPreparation
    {
        /// <summary>
        /// Sets initial Room and User entities
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        /// <returns></returns>
        public static async Task PopulateAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<HotelDbContext>();
            context.Database.Migrate();
            if (!context.Set<Room>().Any())
            {
                await context.AddRangeAsync(new[]
                {
                    new Room { Number = 101, Capacity = 3, PriceForDay = 2000, IsVacant = true},
                    new Room { Number = 102, Capacity = 3, PriceForDay = 2000, IsVacant = true},
                    new Room { Number = 103, Capacity = 3, PriceForDay = 2000, IsVacant = true},
                    new Room { Number = 104, Capacity = 3, PriceForDay = 2000, IsVacant = true},
                    new Room { Number = 201, Capacity = 2, PriceForDay = 1500, IsVacant = true},
                    new Room { Number = 202, Capacity = 2, PriceForDay = 1500, IsVacant = true},
                    new Room { Number = 203, Capacity = 2, PriceForDay = 1500, IsVacant = true},
                    new Room { Number = 204, Capacity = 2, PriceForDay = 1500, IsVacant = true},
                    new Room { Number = 301, Capacity = 1, PriceForDay = 1000, IsVacant = true},
                    new Room { Number = 302, Capacity = 1, PriceForDay = 1000, IsVacant = true},
                    new Room { Number = 303, Capacity = 1, PriceForDay = 1000, IsVacant = true},
                    new Room { Number = 304, Capacity = 1, PriceForDay = 1000, IsVacant = true},
                });
                await context.SaveChangesAsync();
            }
            var manager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
            if (!context.Users.Any())
            {
                var user = new User { UserName = "Admin", Email = "admin@admin.com" };
                await manager.CreateAsync(user, "Secret123$");
                user = await manager.FindByNameAsync(user.UserName);
                await roleManager.CreateAsync(new Role { Name = "Admin" });
                await manager.AddToRoleAsync(user, "Admin");
            }
        }
    }
}
