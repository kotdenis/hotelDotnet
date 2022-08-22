using HotelApp.Data.Entities;
using HotelApp.Data.Repositories.Interfaces;

namespace HotelApp.Data.Repositories.Implementation
{

    /// <summary>
    /// <inheritdoc cref="GenericRepository{T}"/>
    /// </summary>
    public class BookRepository : GenericRepository<Book>, IBookRepository
    {
        public BookRepository(HotelDbContext dbContext) : base(dbContext)
        {

        }
    }
}
