using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MnemosyneDomain.Authorization;
using MnemosyneDomain.Events;
using MnemosyneDomain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace MnemosyneDomain.Commands.Pages
{
    public class PageCommandHandler
    {
        private readonly MnemosyneContext _context;
        private readonly IAuthorizationHandler _authService;

        public PageCommandHandler(MnemosyneContext context, IAuthorizationHandler authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<PageCreated?> HandleAsync(CreatePage request)
        {
            if (!await _authService.IsAuthorizedAsync(request.User, request.NotebookId, AuthorizationPolicies.NotebookOwner)) return null;

            int existingPageCount = _context.Pages.Count(x => x.NotebookId == request.NotebookId);

            Page page = new()
            {
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                NotebookId = request.NotebookId,
                PageId = Guid.NewGuid(),
                PageNumber = existingPageCount
            };

            await _context.Pages.AddAsync(page);
            await _context.SaveChangesAsync();

            return new PageCreated(
                request.User,
                request.NotebookId,
                page.PageId,
                page.PageNumber
            );
        }

        public async Task HandleAsync(DeletePage request)
        {
            var page = await _context.Pages.FirstOrDefaultAsync(p => p.PageId == request.PageId);

            if (page is null) return;

            if (!await _authService.IsAuthorizedAsync(request.User, page.NotebookId, AuthorizationPolicies.NotebookOwner))
                return;

            _context.Pages.Remove(page);
            await _context.SaveChangesAsync();
        }

        public async Task HandleAsync(UpdatePage request)
        {
            if (!await _authService.IsAuthorizedAsync(request.User, request.NotebookId, AuthorizationPolicies.NotebookOwner)) return;

            Page? page = await _context.Pages.FindAsync(request.PageId);

            if (page is not null)
            {
                page.Contents = request.Contents;
                page.Updated = DateTime.UtcNow;

                _context.Pages.Update(page);
                await _context.SaveChangesAsync();
            }
        }

    }
}
