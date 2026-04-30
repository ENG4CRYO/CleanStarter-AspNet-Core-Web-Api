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
        Task<T?> GetByIdAsync(TId id, CancellationToken cancellationToken);
        Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken);
        Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);
        Task<T?> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);
    }
}
#endif