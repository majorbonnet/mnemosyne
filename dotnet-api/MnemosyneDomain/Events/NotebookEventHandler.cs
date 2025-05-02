using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MnemosyneDomain.Commands.Pages;

namespace MnemosyneDomain.Events
{
    public class NotebookEventHandler
    {
        // whenever a notebook is created, we want to create the first page
        public CreatePage HandleAsync(NotebookCreated @event)
        {
            return new CreatePage(@event.User, @event.NotebookId);
        }
    }
}
