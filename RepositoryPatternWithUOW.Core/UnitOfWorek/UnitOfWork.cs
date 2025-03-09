using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace RepositoryPatternWithUOW.Core.UnitOfWorek
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly ConcurrentDictionary<Type, object> _repositories = new();
        private readonly ILogger<UnitOfWork> _logger;
        private bool _disposed = false;

        public UnitOfWork(AppDbContext context, ILogger<UnitOfWork> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context), "DbContext cannot be null.");
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null.");
        }

        /// <summary>
        /// Get or create a repository for the given type T.
        /// </summary>
        public IBaseRepository<T> Repository<T>() where T : class
        {
            return (IBaseRepository<T>)_repositories.GetOrAdd(typeof(T), _ => new BaseRepository<T>(_context));
        }
        /// <summary>
        /// Save changes asynchronously to the database.
        /// </summary>
        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "❌ Error occurred while saving changes to the database.");
                throw new Exception("❌ Database save failed. See inner exception for details.", ex);
            }
        }

        /// <summary>
        /// Execute an action inside a transaction safely.
        /// </summary>
        public async Task ExecuteTransactionAsync(Func<Task> action)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await action();
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                _logger.LogInformation("✅ Transaction executed and committed successfully.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "❌ Transaction execution failed. Rolled back.");
                throw new Exception("Transaction execution failed. See inner exception for details.", ex);
            }
        }

        /// <summary>
        /// Dispose the current context and free resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose pattern implementation to release resources.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
                _logger.LogInformation("✅ DbContext disposed successfully.");
            }
            _disposed = true;
        }
    }


}
