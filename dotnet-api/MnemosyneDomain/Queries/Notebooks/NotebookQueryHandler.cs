using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MnemosyneDomain.Queries.Notebooks
{
    public class NotebookQueryHandler
    {
        private readonly MnemosyneContext _context;
        public NotebookQueryHandler(MnemosyneContext context)
        {
            _context = context;
        }

        public List<NotebookDto> GetNotebooksByUserId(ByUserIdRequest request)
        {
            List<NotebookDto> notebooks = _context.Notebooks
                .Where(x => x.UserId == request.UserId)
                .Select(x => new NotebookDto(
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
