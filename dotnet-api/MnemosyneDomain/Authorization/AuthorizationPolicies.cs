using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MnemosyneDomain.Authorization.Requirements;
using MnemosyneDomain.Models;

namespace MnemosyneDomain.Authorization
{
    internal static class AuthorizationPolicies
    {
        public static List<IAuthorizationRequirement<Notebook>> NotebookOwner = new List<IAuthorizationRequirement<Notebook>>
        {
            new NotebookOwnerRequirement()
        };
    }
}
