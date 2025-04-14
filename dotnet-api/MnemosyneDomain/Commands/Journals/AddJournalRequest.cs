using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MnemosyneDomain.Commands.Journals
{
    public class AddJournalRequest(Guid userId) : BaseRequest
    {
        public Guid UserId => userId;
    }
}
