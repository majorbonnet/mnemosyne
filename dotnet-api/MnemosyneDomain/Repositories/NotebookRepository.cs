using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MnemosyneDomain.Models;

namespace MnemosyneDomain.Repositories
{
    public class NotebookRepository : INotebookRepository
    {
        private readonly MnemosyneContext _context;

        public NotebookRepository(MnemosyneContext context)
        {
            _context = context;
        }

        public async Task AddNotebookAsync(Notebook notebook)
        {
            await _context.Notebooks.AddAsync(notebook);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Notebook>> GetNotebooksByUserIdAsync(Guid userId)
        {
            return await _context.Notebooks
                .Where(n => n.UserId == userId)
                .ToListAsync();
        }

        public async Task RemoveNotebookAsync(int notebookId)
        {
            List<NotebookPage> pages = await _context.NotebookPages
                .Where(p => p.NotebookId == notebookId)
                .ToListAsync();

            _context.NotebookPages.RemoveRange(pages);
            _context.Notebooks.Remove(new Notebook { NotebookId = notebookId });

            await _context.SaveChangesAsync();
        }
    }
}
