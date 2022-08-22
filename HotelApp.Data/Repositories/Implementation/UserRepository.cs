using HotelApp.Data.Entities;
using HotelApp.Data.Repositories.Interfaces;

namespace HotelApp.Data.Repositories.Implementation
{
    /// <summary>
    /// <inheritdoc cref="GenericRepository{T}"/>
    /// </summary>
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext">Database context</param>
        public UserRepository(HotelDbContext dbContext) : base(dbContext)
        {

        }
    }
}
