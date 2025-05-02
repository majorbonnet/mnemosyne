using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MnemosyneDomain.Authorization;
using MnemosyneDomain.Models;

namespace MnemosyneDomain.Commands.Pages
{
    public class UpdatePage(User user, Guid notebookId, Guid pageId, string contents) : BaseRequest
    {
        public User User => user;
        public Guid NotebookId => notebookId;    
        public Guid PageId => pageId;
        public string Contents => contents;
    }
}
