using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MnemosyneDomain.Models;

namespace MnemosyneDomain.Repositories
{
    public class NotebookPageRepository : INotebookPageRepository
    {
        private readonly MnemosyneContext _context;
        public NotebookPageRepository(MnemosyneContext context)
        {
            _context = context;
        }

        public async Task AddPageAsync(NotebookPage notebookPage)
        {
            await _context.NotebookPages.AddAsync(notebookPage);
            await _context.SaveChangesAsync();
        }

        public async Task<NotebookPage?> GetPageByIdAsync(Guid notebookPageId)
        {
            return await _context.NotebookPages.FirstOrDefaultAsync(p => p.NotebookPageId == notebookPageId);
        }

        public async Task<int> GetPageCountAsync(int notebookId)
        {
            return await _context.NotebookPages.CountAsync(p => p.NotebookId == notebookId);
        }

        public async Task<IEnumerable<NotebookPage>> GetPagesByNotebookIdAsync(int notebookId)
        {
            return await _context.NotebookPages
                            .Where(x => x.NotebookId == notebookId)
                            .ToListAsync();
        }

        public async Task RemovePageAsync(Guid notebookPageId)
        {
            NotebookPage? page = await _context.NotebookPages.FindAsync(notebookPageId);

            if (page is not null)
            {
                _context.NotebookPages.Remove(page);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdatePageAsync(NotebookPage notebookPage)
        {
            _context.NotebookPages.Update(notebookPage);
            await _context.SaveChangesAsync();
        }
    }
}
