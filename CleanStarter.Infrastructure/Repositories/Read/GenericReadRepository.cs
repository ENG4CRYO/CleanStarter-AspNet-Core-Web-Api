using CleanStarter.Application.Interfaces.RepositoryInterfaces.Read;
using CleanStarter.Application.Interfaces.RepositoryInterfaces.Write;
using CleanStarter.Core.Entities.BaseEntity;
using CleanStarter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;

#if IsRepository
namespace CleanStarter.Infrastructure.Repositories.Read
{
    public class GenericReadRepository<T, TId> : IGenericReadRepository<T, TId> where T : class, IEntity<TId>
    {
        private readonly AppDbContext _context;

        public GenericReadRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task<T?> GetByIdAsync(TId id, CancellationToken cancellationToken)
        {
            return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(e => e.Id.Equals(id), cancellationToken);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _context.Set<T>().AsNoTracking().Where(predicate).ToListAsync(cancellationToken);
        }
    }
}
#endif