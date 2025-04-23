using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MnemosyneDomain.Authorization;
using MnemosyneDomain.Models;

namespace MnemosyneDomain.Commands.NotebookPages
{
    public class NotebookPageCommandHandler
    {
        private readonly MnemosyneContext _context;
        private readonly AuthorizationHandler _authService;

        public NotebookPageCommandHandler(MnemosyneContext context, AuthorizationHandler authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task HandleAsync(CreateNotebookPage request)
        {
            if (!_authService.IsAuthorized(request.User, request.NotebookId, AuthorizationPolicies.NotebookOwner)) return;

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

        public async Task HandleAsync(DeleteNotebookPage request)
        {
            NotebookPage? page = _context.NotebookPages.Find(request.NotebookPageId);
            
            if (page is not null)
            {
                if (!_authService.IsAuthorized(request.User, page.NotebookId, AuthorizationPolicies.NotebookOwner)) return;

                _context.NotebookPages.Remove(page);
                await _context.SaveChangesAsync();
            }
        }

        public async Task HandleAsync(UpdateNotebookPage request)
        {
            NotebookPage? page = _context.NotebookPages.Find(request.NotebookPageId);

            if (page is not null)
            {
                if (!_authService.IsAuthorized(request.User, page.NotebookId, AuthorizationPolicies.NotebookOwner)) return;

                page.Contents = request.Contents;
                page.Title = request.Title;
                page.Updated = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}
