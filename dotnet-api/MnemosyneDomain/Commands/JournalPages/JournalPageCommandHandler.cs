using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MnemosyneDomain.Models;

namespace MnemosyneDomain.Commands.JournalPages
{
    public class JournalPageCommandHandler : ICommandHandler<AddJournalPageRequest>, 
        ICommandHandler<DeleteJournalPageRequest>,
        ICommandHandler<UpdateJournalPageRequest>
    {
        private readonly MnemosyneContext _context;
        public JournalPageCommandHandler(MnemosyneContext context)
        {
            _context = context;
        }

        public async Task Handle(AddJournalPageRequest request)
        {
            int existingPageCount = _context.JournalPages.Count(p => p.JournalId == request.JournalId);

            JournalPage page = new JournalPage
            {
                Created = DateTime.Now,
                Updated = DateTime.Now,
                JournalId = request.JournalId,
                PageNumber = existingPageCount
            };

            await _context.JournalPages.AddAsync(page);
            await _context.SaveChangesAsync();
        }

        public async Task Handle(DeleteJournalPageRequest request)
        {
            JournalPage? page = _context.JournalPages.Find(request.JournalPageId);
            
            if (page is not null)
            {
                _context.JournalPages.Remove(page);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Handle(UpdateJournalPageRequest request)
        {
            JournalPage? page = _context.JournalPages.Find(request.JournalPageId);

            if (page is not null)
            {
                page.Contents = request.Contents;
                page.Title = request.Title;
                page.Updated = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }
    }
}
