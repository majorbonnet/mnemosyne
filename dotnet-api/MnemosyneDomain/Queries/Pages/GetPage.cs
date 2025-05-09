using MnemosyneDomain.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MnemosyneDomain.Queries.Pages
{
    public class GetPage(User user, Guid notebookId, Guid pageId) : BaseRequest
    {
        public User User => user;
        public Guid NotebookId => notebookId;
        public Guid PageId => pageId;
    }
}
