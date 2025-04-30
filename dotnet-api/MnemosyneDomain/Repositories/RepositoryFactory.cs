using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MnemosyneDomain.Models;

namespace MnemosyneDomain.Repositories
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly MnemosyneContext _context;
        public RepositoryFactory(MnemosyneContext context)
        {
            _context = context;
        }

        public IRepository<TEntity> CreateRepository<TEntity>() where TEntity : class
        {
            return new Repository<TEntity>(_context);
        }
    }
}
