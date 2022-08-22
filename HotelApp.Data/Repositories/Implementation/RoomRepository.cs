using HotelApp.Data.Entities;
using HotelApp.Data.Repositories.Interfaces;

namespace HotelApp.Data.Repositories.Implementation
{
    /// <summary>
    /// <inheritdoc cref="GenericRepository{T}"/>
    /// </summary>
    internal class RoomRepository : GenericRepository<Room>, IRoomRepository
    {
        public RoomRepository(HotelDbContext dbContext) : base(dbContext)
        {

        }
    }
}
