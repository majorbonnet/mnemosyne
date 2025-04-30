using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MnemosyneDomain.Authorization;

namespace MnemosyneDomain.Events
{
    public record NotebookCreated(User User, int NotebookId, DateTime Created, DateTime Updated);
}
