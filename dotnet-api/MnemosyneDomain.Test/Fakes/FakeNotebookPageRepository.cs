using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MnemosyneDomain.Models;
using MnemosyneDomain.Repositories;

namespace MnemosyneDomain.Test.Fakes
{
    internal class FakeNotebookPageRepository : INotebookPageRepository
    {
        private readonly List<NotebookPage> _notebookPages = new();

        public List<NotebookPage> NotebookPages => _notebookPages;

        public Task AddPageAsync(NotebookPage notebookPage)
        {
            _notebookPages.Add(notebookPage);
            return Task.CompletedTask;
        }

        public Task<NotebookPage?> GetPageByIdAsync(Guid notebookPageId)
        {
            return Task.FromResult(_notebookPages.FirstOrDefault(p => p.NotebookPageId == notebookPageId));
        }

        public Task<int> GetPageCountAsync(int notebookId)
        {
            return Task.FromResult(_notebookPages.Count(p => p.NotebookId == notebookId));
        }

        public Task<IEnumerable<NotebookPage>> GetPagesByNotebookIdAsync(int notebookId)
        {
            return Task.FromResult(_notebookPages.Where(x => x.NotebookId == notebookId).AsEnumerable());
        }

        public Task RemovePageAsync(Guid notebookPageId)
        {
            NotebookPage? page = _notebookPages.FirstOrDefault(p => p.NotebookPageId == notebookPageId);

            if (page is not null)
            {
                _notebookPages.Remove(page);
            }

            return Task.CompletedTask;
        }

        public Task UpdatePageAsync(NotebookPage notebookPage)
        {
            NotebookPage? page = _notebookPages.FirstOrDefault(p => p.NotebookPageId == notebookPage.NotebookPageId);

            if (page is not null)
            {
                page.Contents = notebookPage.Contents;
                page.Title = notebookPage.Title;
                page.Updated = notebookPage.Updated;
            }

            return Task.CompletedTask;
        }
    }
}
