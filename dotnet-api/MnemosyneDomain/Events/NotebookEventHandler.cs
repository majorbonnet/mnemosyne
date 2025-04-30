using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MnemosyneDomain.Commands.NotebookPages;

namespace MnemosyneDomain.Events
{
    public class NotebookEventHandler
    {
        // whenever a notebook is created, we want to create the first page
        public CreateNotebookPage HandleAsync(NotebookCreated @event)
        {
            return new CreateNotebookPage(@event.User, @event.NotebookId);
        }
    }
}
