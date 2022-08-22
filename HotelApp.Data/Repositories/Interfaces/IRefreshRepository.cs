using HotelApp.Data.Entities;

namespace HotelApp.Data.Repositories.Interfaces
{
    /// <summary>
    /// Has contract with IGenericRepository<inheritdoc cref="IGenericRepository{TEntity}"/> 
    /// implements all its methods. Has class RefreshToken<see cref="RefreshToken"/> as generic type
    /// </summary>
    public interface IRefreshRepository : IGenericRepository<RefreshToken>
    {
    }
}
