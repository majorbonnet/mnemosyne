using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MnemosyneDomain.Authorization.Requirements;
using MnemosyneDomain.Models;

namespace MnemosyneDomain.Authorization
{
    public static class AuthorizationPolicies
    {
        public static List<IAuthorizationRequirement<Notebook>> NotebookOwner = new List<IAuthorizationRequirement<Notebook>>
        {
            new NotebookOwnerRequirement()
        };

        // this was added initially to be able to test the IsAuthorized method using a Guid, in practice ownership check will probably always be done at notebook level
        // if I add sharing, may add a NotebookPage whitelist of some kind
        public static List<IAuthorizationRequirement<NotebookPage>> NotebookPageOwner = new List<IAuthorizationRequirement<NotebookPage>>
        {
            new NotebookPageOwnerRequirement()
        };
    }
}
