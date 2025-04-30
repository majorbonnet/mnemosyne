using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MnemosyneDomain.Models;

namespace MnemosyneDomain.Authorization.Requirements
{
    public class NotebookPageOwnerRequirement : IAuthorizationRequirement<NotebookPage>
    {
        public bool IsMet(User user, NotebookPage resource)
        {
            if (resource.Notebook is null)
            {
                return false;
            }

            return resource.Notebook.UserId == user.UserId;
        }
    }
}
