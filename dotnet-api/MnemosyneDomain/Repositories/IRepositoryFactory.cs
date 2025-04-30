using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MnemosyneDomain.Models;

namespace MnemosyneDomain.Repositories
{
    public interface IRepositoryFactory
    {
        IRepository<TEntity> CreateRepository<TEntity>() where TEntity : class;
    }
}
