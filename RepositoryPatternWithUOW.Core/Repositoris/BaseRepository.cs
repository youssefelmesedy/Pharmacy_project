namespace RepositoryPatternWithUOW.Core.Repositoris
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;
        public BaseRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>() ?? throw new InvalidOperationException($"DbSet for {typeof(T).Name} could not be initialized.");
        }

        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();
            query = includes.Aggregate(query, (current, include) => current.Include(include));
            return await query.ToListAsync();
        }

        public async Task<T> GetByIDAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), $"ID must be greater than zero.");

            IQueryable<T> query = _dbSet.AsNoTracking();
            query = includes.Aggregate(query, (current, include) => current.Include(include));

            var entity = await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
            return entity ?? throw new KeyNotFoundException($"Entity with ID {id} not found.");
        }

        public async Task<T?> FindAsync(Expression<Func<T, bool>> criteria, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();
            query = includes.Aggregate(query, (current, include) => current.Include(include));

            return await query.SingleOrDefaultAsync(criteria);
        }

        public async Task<bool> ExistAsync(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate), "Search criteria must be provided.");
            return await _dbSet.AnyAsync(predicate);
        }

        public async Task<T> CreateAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            await _dbSet.AddAsync(entity);

            return entity;
        }

        public async Task<IEnumerable<T>> CreateListAsync(IEnumerable<T> entities)
        {
            if (entities == null || !entities.Any()) throw new ArgumentException("Entity list cannot be empty.");

            await _dbSet.AddRangeAsync(entities);

            return entities;
        }

        public Task<T> UpdateAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            _dbSet.Update(entity);
            
            return Task.FromResult(entity);
        }

        public Task<IEnumerable<T>> UpdateListAsync(IEnumerable<T> entities)
        {
            if (entities == null || !entities.Any()) throw new ArgumentException("Entity list cannot be empty.");

            _dbSet.UpdateRange(entities);

            return Task.FromResult(entities);
        }

        public Task<T> DeleteAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            _dbSet.Remove(entity);

            return Task.FromResult(entity);
        }

        public Task<IEnumerable<T>> DeleteListAsync(IEnumerable<T> entities)
        {
            if (entities == null || !entities.Any()) throw new ArgumentException("Entity list cannot be empty.");

            _dbSet.RemoveRange(entities);

            return Task.FromResult(entities);
        }

    }

}

