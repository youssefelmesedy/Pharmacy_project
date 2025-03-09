using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.Repositoris
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync (params Expression<Func<T, object>>[] Includes);
        Task<T> GetByIDAsync (int ID, params Expression<Func<T, object>>[] Includes);
        Task<T?> FindAsync(Expression<Func<T, bool>> criteria, params Expression<Func<T, object>>[] Includes);
        Task<bool> ExistAsync(Expression<Func<T, bool>> exist);
        Task<T> CreateAsync( T entity);
        Task<IEnumerable<T>> CreateListAsync(IEnumerable<T> entity);
        Task<T> UpdateAsync(T entity);
        Task<IEnumerable<T>> UpdateListAsync(IEnumerable<T> entity);
        Task<T> DeleteAsync(T entity);
        Task<IEnumerable<T>> DeleteListAsync(IEnumerable<T> entity);
    }
}
