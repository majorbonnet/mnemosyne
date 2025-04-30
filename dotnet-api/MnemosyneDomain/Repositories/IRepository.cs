using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using MnemosyneDomain.Models;

namespace MnemosyneDomain.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll();
        Task<TEntity?> FindOneAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity?> FindOneAsync(params object[] keyValues);
        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        Task<bool> AddIfNotExistsAsync(TEntity entity);
        Task AddAsync(TEntity entity);
        Task AddManyAsync(IEnumerable<TEntity> entities);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task DeleteManyAsync(Expression<Func<TEntity, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
