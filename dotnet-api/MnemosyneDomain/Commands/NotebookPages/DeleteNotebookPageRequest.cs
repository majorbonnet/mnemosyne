using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MnemosyneDomain.Commands.NotebookPages
{
    public class DeleteNotebookPageRequest(Guid notebookPageId) : BaseRequest
    {
        public Guid NotebookPageId => notebookPageId;
    }
}
