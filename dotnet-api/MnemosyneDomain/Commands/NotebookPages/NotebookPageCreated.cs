using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MnemosyneDomain.Commands.NotebookPages
{
    public record NotebookPageCreated(int NotebookId, Guid NotebookPageId, int PageNumber);
}
