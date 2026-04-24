using CleanStarter.Core.Entities.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

#if IsRepository
namespace CleanStarter.Application.Interfaces.RepositoryInterfaces.Read
{
    public interface IGenericReadRepository<T, TId> where T : class, IEntity<TId>
    {
        Task<T?> GetByIdAsync(TId id);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>> predicate);
        Task<T?> GetAsync(Expression<Func<T, bool>> predicate);
    }
}
#endif