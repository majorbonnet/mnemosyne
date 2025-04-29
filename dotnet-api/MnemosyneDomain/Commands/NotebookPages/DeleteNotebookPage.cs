using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MnemosyneDomain.Authorization;

namespace MnemosyneDomain.Commands.NotebookPages
{
    public class DeleteNotebookPage(User user, int notebookId, Guid notebookPageId) : BaseRequest
    {
        public User User => user;
        public int NotebookId => notebookId;
        public Guid NotebookPageId => notebookPageId;
    }
}
