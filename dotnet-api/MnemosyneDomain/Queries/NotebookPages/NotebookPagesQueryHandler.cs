using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MnemosyneDomain.Queries.NotebookPages
{
    public class NotebookPagesQueryHandler
    {
        private readonly MnemosyneContext _context;
        internal NotebookPagesQueryHandler(MnemosyneContext context)
        {
            _context = context;
        }

        public List<NotebookPageDto> GetNotebookPagesByNotebookId(ByNotebookIdRequest request)
        {
            List<NotebookPageDto> pages = _context.NotebookPages
                .Where(x => x.NotebookId == request.NotebookId)
                .Select(x => new NotebookPageDto
                (
                    x.NotebookPageId,
                    x.Created,
                    x.Updated,
                    x.PageNumber,
                    x.Title,
                    x.Contents
                ))
                .ToList();

            return pages;
        }
    }
}
