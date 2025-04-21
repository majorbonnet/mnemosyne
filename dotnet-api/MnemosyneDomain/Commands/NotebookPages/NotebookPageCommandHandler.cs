using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MnemosyneDomain.Models;

namespace MnemosyneDomain.Commands.NotebookPages
{
    public class NotebookPageCommandHandler : ICommandHandler<AddNotebookPageRequest>, 
        ICommandHandler<DeleteNotebookPageRequest>,
        ICommandHandler<UpdateNotebookPageRequest>
    {
        private readonly MnemosyneContext _context;
        public NotebookPageCommandHandler(MnemosyneContext context)
        {
            _context = context;
        }

        public async Task Handle(AddNotebookPageRequest request)
        {
            int existingPageCount = _context.NotebookPages.Count(p => p.NotebookId == request.NotebookId);

            NotebookPage page = new()
            {
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                NotebookId = request.NotebookId,
                PageNumber = existingPageCount
            };

            await _context.NotebookPages.AddAsync(page);
            await _context.SaveChangesAsync();
        }

        public async Task Handle(DeleteNotebookPageRequest request)
        {
            NotebookPage? page = _context.NotebookPages.Find(request.NotebookPageId);
            
            if (page is not null)
            {
                _context.NotebookPages.Remove(page);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Handle(UpdateNotebookPageRequest request)
        {
            NotebookPage? page = _context.NotebookPages.Find(request.NotebookPageId);

            if (page is not null)
            {
                page.Contents = request.Contents;
                page.Title = request.Title;
                page.Updated = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}
