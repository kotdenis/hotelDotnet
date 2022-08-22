using HotelApp.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelApp.Data.Extensions
{
    public static class DbContextExtensions
    {
        /// <summary>
        /// Extension for DbContext. Sets BaseEntity before saving
        /// </summary>
        /// <param name="context">DbContext</param>
        public static void OnBeforeSaving(this DbContext context)
        {
            foreach (var entry in context.ChangeTracker.Entries())
            {
                if (entry.Entity is BaseEntity baseEntity)
                {
                    var now = DateTime.UtcNow;

                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            baseEntity.Updated = now;
                            break;

                        case EntityState.Added:
                            baseEntity.Created = now;
                            baseEntity.Updated = now;
                            break;
                    }
                }
            }
        }
    }
}
