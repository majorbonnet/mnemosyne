using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MnemosyneDomain.Authorization;

namespace MnemosyneDomain.Events
{
    public record NotebookPageCreated(User User, int NotebookId, Guid NotebookPageId, int PageNumber);
}
