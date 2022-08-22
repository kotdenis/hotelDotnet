using HotelApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace HotelApp.Data.Repositories.Implementation
{
    /// <summary>
    /// Generates save and transaction like a pattern of UnitOfWork <see cref="https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application"/>
    /// </summary>
    public class UnitOfWork : IDisposable
    {
        private readonly HotelDbContext _dbContext;
        public UnitOfWork(HotelDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Saves all changes in db
        /// </summary>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Task<bool></returns>
        public async Task<bool> SaveAsync(CancellationToken ct)
        {
            return await _dbContext.SaveChangesAsync(ct) >= 0;
        }

        /// <summary>
        /// Generates new transaction or takes current
        /// </summary>
        /// <param name="isolationLevel">IsolationLevel</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Task<IDbContextTransaction></returns>
        public Task<IDbContextTransaction> EnsureTransactionAsync(IsolationLevel isolationLevel, CancellationToken ct = default)
        {
            IDbContextTransaction transaction = _dbContext.Database.CurrentTransaction!;
            if (transaction == null)
                return _dbContext.Database.BeginTransactionAsync(isolationLevel, ct);
            return Task.FromResult(transaction);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
