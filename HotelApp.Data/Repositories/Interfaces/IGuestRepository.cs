using HotelApp.Data.Entities;

namespace HotelApp.Data.Repositories.Interfaces
{
    /// <summary>
    /// Has contract with IGenericRepository<inheritdoc cref="IGenericRepository{TEntity}"/> 
    /// implements all its methods. Has class Guest<see cref="Guest"/> as generic type
    /// </summary>
    public interface IGuestRepository : IGenericRepository<Guest>
    {
    }
}
