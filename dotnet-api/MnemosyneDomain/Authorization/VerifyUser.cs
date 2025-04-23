using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MnemosyneDomain.Authorization
{
    public class VerifyUser(Guid userId) : BaseRequest
    {
        public Guid UserId => userId;
    }
}
