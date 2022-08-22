using HotelApp.Data.Entities;
using HotelApp.Data.Repositories.Interfaces;

namespace HotelApp.Data.Repositories.Implementation
{
    /// <summary>
    /// <inheritdoc cref="GenericRepository{T}"/>
    /// </summary>
    public class GuestRepository : GenericRepository<Guest>, IGuestRepository
    {
        public GuestRepository(HotelDbContext dbContext) : base(dbContext)
        {

        }
    }
}
