using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MnemosyneDomain.Authorization;

namespace MnemosyneDomain.Queries.Notebooks
{
    public class GetNotebook(User user, int notebookId) : BaseRequest
    {
        public User User => user;
        public int NotebookId => notebookId;
    }
}
