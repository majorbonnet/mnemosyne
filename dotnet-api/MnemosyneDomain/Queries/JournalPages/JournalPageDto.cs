using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MnemosyneDomain.Queries.JournalPages
{
    public record JournalPageDto (Guid JournalPageId, DateTime Created, DateTime Updated, int PageNumber, string? Title, string? Contents);
}
