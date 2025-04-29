using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MnemosyneDomain.Authorization.Requirements;

namespace MnemosyneDomain.Authorization
{
    public interface IAuthorizationHandler
    {
        bool IsAuthorized<TResource>(User user, Guid resourceId, List<IAuthorizationRequirement<TResource>> requirements) where TResource : class;
        bool IsAuthorized<TResource>(User user, int resourceId, List<IAuthorizationRequirement<TResource>> requirements) where TResource : class;
        bool IsAuthorized<TResource>(User user, TResource resource, List<IAuthorizationRequirement<TResource>> requirements) where TResource : class;
    }
}
