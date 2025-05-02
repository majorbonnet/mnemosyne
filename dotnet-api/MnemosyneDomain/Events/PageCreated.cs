using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MnemosyneDomain.Authorization;

namespace MnemosyneDomain.Events
{
    public record PageCreated(User User, Guid NotebookId, Guid PageId, int PageNumber);
}
