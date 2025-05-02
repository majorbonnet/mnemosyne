using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MnemosyneDomain.Authorization;

namespace MnemosyneDomain.Commands.Pages
{
    public class DeletePage(User user, Guid notebookId, Guid pageId) : BaseRequest
    {
        public User User => user;
        public Guid NotebookId => notebookId;
        public Guid PageId => pageId;
    }
}
