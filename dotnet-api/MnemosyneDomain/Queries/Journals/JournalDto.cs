using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MnemosyneDomain.Queries.Journals
{
    public record JournalDto(int JournalId, DateTime Created, DateTime Updated, string? Title);
}
