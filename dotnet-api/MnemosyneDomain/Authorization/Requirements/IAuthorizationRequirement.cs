using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MnemosyneDomain.Authorization.Requirements
{
    public interface IAuthorizationRequirement<TResource> where TResource : class
    {
        bool IsMet(User user, TResource resource);
    }
}
