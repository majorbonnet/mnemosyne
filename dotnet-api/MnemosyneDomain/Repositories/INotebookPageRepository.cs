using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MnemosyneDomain.Models;

namespace MnemosyneDomain.Repositories
{
    public interface INotebookPageRepository
    {
        Task<int> GetPageCountAsync(int notebookId);
        Task<IEnumerable<NotebookPage>> GetPagesByNotebookIdAsync(int notebookId);
        Task<NotebookPage?> GetPageByIdAsync(Guid notebookPageId);
        Task AddPageAsync(NotebookPage notebookPage);
        Task RemovePageAsync(Guid notebookPageId);
        Task UpdatePageAsync(NotebookPage notebookPage);

    }
}
