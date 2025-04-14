using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MnemosyneDomain.Commands.JournalPages
{
    public class AddJournalPageRequest(int journalId) : BaseRequest
    {
        public int JournalId => journalId;
    }
}
