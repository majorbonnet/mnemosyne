using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MnemosyneDomain.Commands.Notebooks
{
    public class AddNotebookRequest(Guid userId) : BaseRequest
    {
        public Guid UserId => userId;
    }
}
