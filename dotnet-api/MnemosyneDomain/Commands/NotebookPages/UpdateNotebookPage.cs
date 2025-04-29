using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MnemosyneDomain.Authorization;
using MnemosyneDomain.Models;

namespace MnemosyneDomain.Commands.NotebookPages
{
    public class UpdateNotebookPage(User user, int notebookId, Guid notebookPageId, string? title, string? contents) : BaseRequest
    {
        public User User => user;
        public int NotebookId => notebookId;    
        public Guid NotebookPageId => notebookPageId;
        public string? Title => title;
        public string? Contents => contents;
    }
}
