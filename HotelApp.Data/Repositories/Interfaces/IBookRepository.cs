using HotelApp.Data.Entities;

namespace HotelApp.Data.Repositories.Interfaces
{
    /// <summary>
    /// Has contract with IGenericRepository<inheritdoc cref="IGenericRepository{TEntity}"/> 
    /// implements all its methods. Has class Book<see cref="Book"/> as generic type
    /// </summary>
    public interface IBookRepository : IGenericRepository<Book>
    {
    }
}
