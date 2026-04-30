using CleanStarter.Application.Interfaces.RepositoryInterfaces.Write;
using CleanStarter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
#if IsRepository
namespace CleanStarter.Infrastructure.Repositories.Write
{
    public class GenericWriteRepository<T, TId> : IGenericWriteRepository<T, TId> where T : class
    {
        private readonly AppDbContext _context;
        public GenericWriteRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
        public async Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }
        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task<T?> GetByIdAsync(object id, CancellationToken cancellationToken)
        {
            return await _context.Set<T>().FindAsync(id, cancellationToken);
        }

    }
}
#endif