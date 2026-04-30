using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

#if IsRepository
namespace CleanStarter.Application.Interfaces.RepositoryInterfaces.Write
{
    public interface IGenericWriteRepository<T, TId> where T : class
    {
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task DeleteRangeAsync(IEnumerable<T> entities);
        Task<T?> GetByIdAsync(object id,CancellationToken cancellationToken);
        Task<T?> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);
    }
}
#endif