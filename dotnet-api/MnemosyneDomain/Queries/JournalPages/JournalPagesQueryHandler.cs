using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MnemosyneDomain.Queries.JournalPages
{
    public class JournalPagesQueryHandler
    {
        private readonly MnemosyneContext _context;
        internal JournalPagesQueryHandler(MnemosyneContext context)
        {
            _context = context;
        }

        public List<JournalPageDto> GetJournalPagesByUserId(ByJournalIdRequest request)
        {
            List<JournalPageDto> pages = _context.JournalPages
                .Where(x => x.JournalId == request.JournalId)
                .Select(x => new JournalPageDto
                (
                    x.JournalPageId,
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
