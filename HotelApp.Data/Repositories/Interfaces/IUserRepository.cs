using HotelApp.Data.Entities;

namespace HotelApp.Data.Repositories.Interfaces
{
    /// <summary>
    /// Has contract with IGenericRepository<inheritdoc cref="IGenericRepository{TEntity}"/> 
    /// implements all its methods. Has class User<see cref="User"/> as generic type
    /// </summary>
    public interface IUserRepository : IGenericRepository<User>
    {
    }
}
