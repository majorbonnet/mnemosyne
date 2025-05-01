using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MnemosyneDomain.Authorization;
using MnemosyneDomain.Commands.Notebooks;
using MnemosyneDomain.Events;
using Wolverine;

namespace MnemosyneDomain.Queries.Notebooks
{
    public class NotebookQueryHandler
    {
        private readonly MnemosyneContext _context;

        public NotebookQueryHandler(MnemosyneContext context)
        {
            _context = context;
        }

        public async Task<List<Notebook>> HandleAsync(GetNotebooks request)
        {
            List<Notebook> notebooks = await _context.Notebooks
                .Where(n => n.UserId == request.User.UserId)
                .Select(x => new Notebook(
                    x.NotebookId,
                    x.Created,
                    x.Updated,
                    x.Title
                ))
                .ToListAsync();

            return notebooks;
        }
    }
}
