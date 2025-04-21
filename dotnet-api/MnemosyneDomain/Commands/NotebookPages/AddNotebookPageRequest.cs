using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MnemosyneDomain.Commands.NotebookPages
{
    public class AddNotebookPageRequest(int notebookId) : BaseRequest
    {
        public int NotebookId => notebookId;
    }
}
