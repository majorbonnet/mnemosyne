using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MnemosyneDomain.Queries.Notebooks
{
    public class ByUserIdRequest(Guid userId) : BaseRequest
    {
        public Guid UserId => userId;
    }
}
