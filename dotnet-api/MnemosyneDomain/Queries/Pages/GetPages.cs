using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MnemosyneDomain.Authorization;

namespace MnemosyneDomain.Queries.Pages
{
    public class GetPages(User user, Guid notebookId) : BaseRequest
    {
        public User User => user;
        public Guid NotebookId => notebookId;
    }
}
