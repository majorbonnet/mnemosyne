using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MnemosyneDomain.Authorization;

namespace MnemosyneDomain.Commands.NotebookPages
{
    public class CreateNotebookPage(User user, int notebookId) : BaseRequest
    {
        public User User => user;
        public int NotebookId => notebookId;
    }
}
