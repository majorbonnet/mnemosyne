using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MnemosyneDomain.Commands.JournalPages
{
    public class UpdateJournalPageRequest(Guid journalPageId, string? title, string contents) : BaseRequest
    {
        public Guid JournalPageId => journalPageId;
        public string? Title => title;
        public string Contents => contents;
    }
}
