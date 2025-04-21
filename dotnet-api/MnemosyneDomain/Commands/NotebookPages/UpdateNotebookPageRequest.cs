using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MnemosyneDomain.Models;

namespace MnemosyneDomain.Commands.NotebookPages
{
    public class UpdateNotebookPageRequest(Guid notebookPageId, string? title, string contents) : BaseRequest
    {
        public Guid NotebookPageId => notebookPageId;
        public string? Title => title;
        public string Contents => contents;
    }
}
