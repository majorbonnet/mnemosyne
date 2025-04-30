using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MnemosyneDomain.Models;
using MnemosyneDomain.Repositories;

namespace MnemosyneDomain.Test.Fakes
{
    internal class FakeNotebookRepository : INotebookRepository
    {
        private readonly List<Notebook> _notebooks = new();

        public List<Notebook> Notebooks => _notebooks;

        public Task AddNotebookAsync(Notebook notebook)
        {
            notebook.NotebookId = _notebooks.Count > 0 ? _notebooks.Max(n => n.NotebookId) + 1 : 1;
            _notebooks.Add(notebook);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Notebook>> GetNotebooksByUserIdAsync(Guid userId)
        {
            return Task.FromResult(_notebooks.Where(n => n.UserId == userId).AsEnumerable());
        }

        public Task RemoveNotebookAsync(int notebookId)
        {
            Notebook? notebook = _notebooks.FirstOrDefault(n => n.NotebookId == notebookId);

            if (notebook is not null)
            {
                _notebooks.Remove(notebook);
            }

            return Task.CompletedTask;
        }
    }
}
