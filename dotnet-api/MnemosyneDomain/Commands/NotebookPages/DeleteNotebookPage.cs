using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MnemosyneDomain.Authorization;

namespace MnemosyneDomain.Commands.NotebookPages
{
    public class DeleteNotebookPage(User user, Guid notebookPageId) : BaseRequest
    {
        public User User => user;
        public Guid NotebookPageId => notebookPageId;
    }
}
