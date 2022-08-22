using HotelApp.Data.Entities;
using HotelApp.Data.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelApp.Data
{
    public class HotelDbContext : IdentityDbContext<User, Role, Guid>
    {
        public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Room>()
                .HasMany(x => x.Books)
                .WithOne(x => x.Room);

            builder.Entity<Room>()
                .HasQueryFilter(x => x.IsDeleted == false);

            builder.Entity<Guest>()
                .HasMany(x => x.Books)
                .WithOne(x => x.Guest);

            builder.Entity<Guest>()
                .HasQueryFilter(x => x.IsDeleted == false);

            builder.Entity<Book>()
                .HasOne(x => x.Guest)
                .WithMany(x => x.Books);

            builder.Entity<Book>()
                .HasOne(x => x.Room)
                .WithMany(x => x.Books);

            builder.Entity<Book>()
                .HasQueryFilter(x => x.IsDeleted == false);

            builder.Entity<RefreshToken>()
                .HasQueryFilter(x => x.IsDeleted == false);
        }

        public override Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            this.OnBeforeSaving();
            return base.SaveChangesAsync(ct);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }
    }
}
