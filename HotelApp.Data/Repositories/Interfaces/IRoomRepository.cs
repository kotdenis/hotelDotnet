using HotelApp.Data.Entities;

namespace HotelApp.Data.Repositories.Interfaces
{
    /// <summary>
    /// Has contract with IGenericRepository<inheritdoc cref="IGenericRepository{TEntity}"/> 
    /// implements all its methods. Has class Room<see cref="Room"/> as generic type
    /// </summary>
    public interface IRoomRepository : IGenericRepository<Room>
    {
    }
}
