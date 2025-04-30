using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using MnemosyneDomain.Models;

namespace MnemosyneDomain.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly MnemosyneContext _context;

        public Repository(MnemosyneContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AddIfNotExistsAsync(TEntity entity)
        {
            if (_context.Set<TEntity>().Any(e => e.Equals(entity)))
            {
                return false; // Entity already exists
            }

            _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync();

            return true; // Entity added successfully
        }

        public async Task AddManyAsync(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().AddRange(entities);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().AnyAsync(predicate);
        }

        public Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().CountAsync(predicate);
        }

        public async Task DeleteAsync(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteManyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = _context.Set<TEntity>().Where(predicate);

            _context.Set<TEntity>().RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().Where(predicate);
        }

        public async Task<TEntity?> FindOneAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        public async Task<TEntity?> FindOneAsync(params object[] keyValues)
        {
            return await _context.Set<TEntity>().FindAsync(keyValues);
        }

        public IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
