using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MnemosyneDomain.Authorization;
using MnemosyneDomain.Events;
using MnemosyneDomain.Models;
using Microsoft.EntityFrameworkCore;

namespace MnemosyneDomain.Commands.NotebookPages
{
    public class NotebookPageCommandHandler
    {
        private readonly MnemosyneContext _context;
        private readonly IAuthorizationHandler _authService;

        public NotebookPageCommandHandler(MnemosyneContext context, IAuthorizationHandler authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<NotebookPageCreated?> HandleAsync(CreateNotebookPage request)
        {
            if (!await _authService.IsAuthorizedAsync(request.User, request.NotebookId, AuthorizationPolicies.NotebookOwner)) return null;

            int existingPageCount = _context.NotebookPages.Count(x => x.NotebookId == request.NotebookId);

            NotebookPage page = new()
            {
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                NotebookId = request.NotebookId,
                NotebookPageId = Guid.NewGuid(),
                PageNumber = existingPageCount
            };

            await _context.NotebookPages.AddAsync(page);
            await _context.SaveChangesAsync();

            return new NotebookPageCreated(
                request.User,
                request.NotebookId,
                page.NotebookPageId,
                page.PageNumber
            );
        }

        public async Task HandleAsync(DeleteNotebookPage request)
        {
            var notebookPage = await _context.NotebookPages.FirstOrDefaultAsync(p => p.NotebookPageId == request.NotebookPageId);

            if (notebookPage is null) return;

            if (!await _authService.IsAuthorizedAsync(request.User, notebookPage.NotebookId, AuthorizationPolicies.NotebookOwner))
                return;

            _context.NotebookPages.Remove(notebookPage);
            await _context.SaveChangesAsync();
        }

        public async Task HandleAsync(UpdateNotebookPage request)
        {
            if (!await _authService.IsAuthorizedAsync(request.User, request.NotebookId, AuthorizationPolicies.NotebookOwner)) return;

            NotebookPage? page = await _context.NotebookPages.FindAsync(request.NotebookPageId);

            if (page is not null)
            {
                page.Contents = request.Contents;
                page.Title = request.Title;
                page.Updated = DateTime.UtcNow;

                _context.NotebookPages.Update(page);
                await _context.SaveChangesAsync();
            }
        }
    }
}
