using HotelApp.Data.Entities;
using HotelApp.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HotelApp.Data.Repositories.Implementation
{
    /// <summary>
    /// Implements common CRUD methods
    /// </summary>
    /// <typeparam name="TEntity">Database entity</typeparam>
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, new()
    {
        protected readonly HotelDbContext _dbContext;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext">Database context</param>
        public GenericRepository(HotelDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Get all entities from database
        /// </summary>
        /// <param name="predicate">Linq expression</param>
        /// <param name="orderBy">Linq expression</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>List of <typeparam name="TEntity"/></returns>
        public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, CancellationToken ct = default)
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();
            if (predicate != null)
                query = query.Where(predicate).AsQueryable();
            if (orderBy != null)
                query = orderBy(query);
            await Task.CompletedTask;
            return query.ToList();
        }

        /// <summary>
        /// Gets database' entity by id
        /// </summary>
        /// <param name="id">Guid</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns><typeparamref name="TEntity"/></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<TEntity> GetAsync(Guid id, CancellationToken ct)
        {
            var entity = await _dbContext.Set<TEntity>().FindAsync(id, ct);
            if (entity != null)
                return entity;
            else
                throw new ArgumentException(nameof(GetAsync));
        }

        /// <summary>
        /// Creates new entity
        /// </summary>
        /// <param name="entity"><typeparamref name="TEntity"/></param>
        /// <param name="ct">CancellationToken</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task CreateAsync(TEntity entity, CancellationToken ct)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(CreateAsync));
            _dbContext.Entry(entity).State = EntityState.Added;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Sets IsDeleted to true
        /// </summary>
        /// <param name="id">Guid</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task SoftDeleteAsync(Guid id, CancellationToken ct)
        {
            var entity = await _dbContext.Set<TEntity>().FindAsync(id, ct);
            if (entity == null)
                throw new ArgumentNullException(nameof(SoftDeleteAsync));
            if (entity is BaseEntity baseEntity)
                baseEntity.IsDeleted = true;
        }

        /// <summary>
        /// Deletes entity from database
        /// </summary>
        /// <param name="entity"><typeparamref name="TEntity"/></param>
        /// <param name="ct">CancellationToken</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task DeleteAsync(TEntity entity, CancellationToken ct)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(DeleteAsync));
            _dbContext.Set<TEntity>().Remove(entity);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Updates database' entity
        /// </summary>
        /// <param name="entity"><typeparamref name="TEntity"/></param>
        /// <param name="ct">CancellationToken</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task UpdateAsync(TEntity entity, CancellationToken ct)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(UpdateAsync));
            _dbContext.Set<TEntity>().Update(entity);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Finds entity by predicate
        /// </summary>
        /// <param name="predicate">Linq expression</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns><typeparam name="TEntity"/></returns>
        public Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct)
        {
            return _dbContext.Set<TEntity>().FirstOrDefaultAsync(predicate, ct)!;
        }

        /// <summary>
        /// Gets entities as Queryable
        /// </summary>
        /// <param name="predicate">Linq expression</param>
        /// <returns>IQueryable</returns>
        public IQueryable<TEntity> GetIncludable(Expression<Func<TEntity, bool>> predicate = null)
        {
            if (predicate == null)
                predicate = (x) => true;
            var query = _dbContext.Set<TEntity>().Where(predicate);
            return query;
        }
    }
}
