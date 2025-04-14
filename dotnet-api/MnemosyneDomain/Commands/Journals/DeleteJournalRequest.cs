using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MnemosyneDomain.Commands.Journals
{
    public class DeleteJournalRequest(int journalId) : BaseRequest
    {
        public int JournalId => journalId;
    }

}
