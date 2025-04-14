using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MnemosyneDomain.Queries.Journals
{
    public class JournalQueryHandler
    {
        private readonly MnemosyneContext _context;
        public JournalQueryHandler(MnemosyneContext context)
        {
            _context = context;
        }

        public List<JournalDto> GetJournalsByUserId(ByUserIdRequest request)
        {
            List<JournalDto> journals = _context.Journals
                .Where(x => x.UserId == request.UserId)
                .Select(x => new JournalDto(
                    x.JournalId,
                    x.Created,
                    x.Updated,
                    x.Title
                ))
                .ToList();
            return journals;
        }
    }
}
