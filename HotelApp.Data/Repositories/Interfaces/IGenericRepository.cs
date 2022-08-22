using System.Linq.Expressions;

namespace HotelApp.Data.Repositories.Interfaces
{
    /// <summary>
    /// Describes methods of GenericRepository <see cref="HotelApp.Data.Repositories.Implementation.GenericRepository{T}"/>
    /// </summary>
    /// <typeparam name="TEntity">Class of Db Entity</typeparam>
    public interface IGenericRepository<TEntity>
    {
        public Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, CancellationToken ct = default);
        Task<TEntity> GetAsync(Guid id, CancellationToken ct);
        Task CreateAsync(TEntity entity, CancellationToken ct);
        Task SoftDeleteAsync(Guid id, CancellationToken ct);
        Task DeleteAsync(TEntity entity, CancellationToken ct);
        Task UpdateAsync(TEntity entity, CancellationToken ct);
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct);
        IQueryable<TEntity> GetIncludable(Expression<Func<TEntity, bool>> predicate = null);
    }
}
