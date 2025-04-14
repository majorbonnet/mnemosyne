using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MnemosyneDomain.Models;

namespace MnemosyneDomain.Commands.Journals
{
    public class JournalCommandHandler : ICommandHandler<AddJournalRequest>, ICommandHandler<DeleteJournalRequest>
    {
        private readonly MnemosyneContext _context;
        public JournalCommandHandler(MnemosyneContext context)
        {
            _context = context;
        }

        public async Task Handle(AddJournalRequest request)
        {
            Journal newJournal = new Journal
            {
                Created = DateTime.Now,
                Updated = DateTime.Now,
                UserId = request.UserId
            };

            await _context.Journals.AddAsync(newJournal);
            await _context.SaveChangesAsync();
        }

        public async Task Handle(DeleteJournalRequest request)
        {
            Journal? journal = await _context.Journals.FindAsync(request.JournalId);

            if (journal is not null)
            {
                _context.Journals.Remove(journal);
                await _context.SaveChangesAsync();
            } 
        }
    }
}
