using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MnemosyneDomain.Queries.JournalPages
{
    public class ByJournalIdRequest(int journalId) : BaseRequest
    {
        public int JournalId => journalId;
    }
}
