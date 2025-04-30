using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MnemosyneDomain.Models;

namespace MnemosyneDomain.Repositories
{
    public interface INotebookRepository
    {
        Task AddNotebookAsync(Notebook notebook);
        Task RemoveNotebookAsync(int notebookId);
        Task<IEnumerable<Notebook>> GetNotebooksByUserIdAsync(Guid userId);
    }
}
