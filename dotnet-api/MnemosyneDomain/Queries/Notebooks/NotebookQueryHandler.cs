using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MnemosyneDomain.Authorization;

namespace MnemosyneDomain.Queries.Notebooks
{
    public class NotebookQueryHandler
    {
        private readonly MnemosyneContext _context;

        public NotebookQueryHandler(MnemosyneContext context)
        {
            _context = context;
        }

        public List<Notebook> Handle(GetNotebooks request)
        {
            List<Notebook> notebooks = _context.Notebooks
                .Where(n => n.UserId == request.User.UserId)
                .Select(x => new Notebook(
                    x.NotebookId,
                    x.Created,
                    x.Updated,
                    x.Title
                ))
                .ToList();

            return notebooks;
        }
    }
}
