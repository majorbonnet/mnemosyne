using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MnemosyneDomain.Authorization.Requirements;
using MnemosyneDomain.Models;

namespace MnemosyneDomain.Authorization
{
    public interface IAuthorizationHandler
    {
        Task<bool> IsAuthorizedAsync<TResource>(User user, Guid resourceId, List<IAuthorizationRequirement<TResource>> requirements) where TResource : class;
        Task<bool> IsAuthorizedAsync<TResource>(User user, TResource resource, List<IAuthorizationRequirement<TResource>> requirements) where TResource : class;
    }
}
