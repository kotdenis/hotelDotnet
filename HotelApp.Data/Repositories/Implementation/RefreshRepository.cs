using HotelApp.Data.Entities;
using HotelApp.Data.Repositories.Interfaces;

namespace HotelApp.Data.Repositories.Implementation
{
    /// <summary>
    /// <inheritdoc cref="GenericRepository{T}"/>
    /// </summary>
    public class RefreshRepository : GenericRepository<RefreshToken>, IRefreshRepository
    {
        public RefreshRepository(HotelDbContext dbContext) : base(dbContext)
        {

        }
    }
}
