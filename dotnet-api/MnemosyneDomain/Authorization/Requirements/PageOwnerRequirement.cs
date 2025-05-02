using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MnemosyneDomain.Models;

namespace MnemosyneDomain.Authorization.Requirements
{
    public class PageOwnerRequirement : IAuthorizationRequirement<Page>
    {
        public bool IsMet(User user, Page resource)
        {
            if (resource?.Notebook is null)
            {
                return false;
            }

            return resource.Notebook.UserId == user.UserId;
        }
    }
}
