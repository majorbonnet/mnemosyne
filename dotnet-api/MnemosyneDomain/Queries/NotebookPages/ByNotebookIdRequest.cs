using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MnemosyneDomain.Queries.NotebookPages
{
    public class ByNotebookIdRequest(int notebookId) : BaseRequest
    {
        public int NotebookId => notebookId;
    }
}
