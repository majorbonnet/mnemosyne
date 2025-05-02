using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MnemosyneDomain.Authorization;
using MnemosyneDomain.Authorization.Requirements;

namespace MnemosyneDomain.Test
{
    internal class FakeAuthorizationHandler : IAuthorizationHandler
    {
        public FakeAuthorizationHandler(bool isAuthorized = true)
        {
            IsAuthorized = isAuthorized;
        }

        public bool IsAuthorized { get; init; }

        public Task<bool> IsAuthorizedAsync<TResource>(User user, Guid resourceId, List<IAuthorizationRequirement<TResource>> requirements) where TResource : class
        {
            return Task.FromResult(IsAuthorized);
        }

        public Task<bool> IsAuthorizedAsync<TResource>(User user, TResource resource, List<IAuthorizationRequirement<TResource>> requirements) where TResource : class
        {
            return Task.FromResult(IsAuthorized);
        }

        public static FakeAuthorizationHandler CreateAuthorized()
        {
            return new FakeAuthorizationHandler(true);
        }

        public static FakeAuthorizationHandler CreateUnauthorized()
        {
            return new FakeAuthorizationHandler(false);
        }
    }
}
