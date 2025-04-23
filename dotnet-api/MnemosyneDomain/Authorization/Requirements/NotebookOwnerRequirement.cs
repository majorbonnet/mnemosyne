using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MnemosyneDomain.Models;

namespace MnemosyneDomain.Authorization.Requirements
{
    internal class NotebookOwnerRequirement : IAuthorizationRequirement<Notebook>
    {
        public bool IsMet(User user, Notebook resource)
        {
            return user.UserId == resource.UserId;
        }
    }
}
