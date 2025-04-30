using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MnemosyneDomain.Authorization;
using MnemosyneDomain.Authorization.Requirements;

namespace MnemosyneDomain.Test.Fakes
{
    internal class FakeAuthorizationHandler : IAuthorizationHandler
    {
        private bool _authorized = false;

        public void SetIsAuthorized(bool isAuthorized)
        {
            _authorized = isAuthorized;
        }

        public bool IsAuthorized<TResource>(User user, Guid resourceId, List<IAuthorizationRequirement<TResource>> requirements) where TResource : class
        {
            return _authorized;
        }

        public bool IsAuthorized<TResource>(User user, int resourceId, List<IAuthorizationRequirement<TResource>> requirements) where TResource : class
        {
            return _authorized;
        }

        public bool IsAuthorized<TResource>(User user, TResource resource, List<IAuthorizationRequirement<TResource>> requirements) where TResource : class
        {
            return _authorized;
        }
    }
}
