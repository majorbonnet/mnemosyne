using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MnemosyneDomain.Models;

namespace MnemosyneDomain.Commands.Notebooks
{
    public class NotebookCommandHandler : ICommandHandler<AddNotebookRequest, NotebookDto>, 
        ICommandHandler<DeleteNotebookRequest>
    {
        private readonly MnemosyneContext _context;
        public NotebookCommandHandler(MnemosyneContext context)
        {
            _context = context;
        }

        public async Task<NotebookDto> Handle(AddNotebookRequest request)
        {
            Notebook newNotebook = new()
            {
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                UserId = request.UserId
            };

            await _context.Notebooks.AddAsync(newNotebook);
            await _context.SaveChangesAsync();

            return new NotebookDto(
                newNotebook.NotebookId,
                newNotebook.Created,
                newNotebook.Updated
            );
        }

        public async Task Handle(DeleteNotebookRequest request)
        {
            Notebook? notebook = await _context.Notebooks.FindAsync(request.NotebookId);

            if (notebook is not null)
            {
                _context.Notebooks.Remove(notebook);
                await _context.SaveChangesAsync();
            } 
        }
    }
}
