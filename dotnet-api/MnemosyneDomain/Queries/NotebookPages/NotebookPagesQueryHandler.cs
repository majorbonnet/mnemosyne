using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MnemosyneDomain.Authorization;

namespace MnemosyneDomain.Queries.NotebookPages
{
    public class NotebookPagesQueryHandler
    {
        private readonly MnemosyneContext _context;
        private readonly AuthorizationHandler _authService;
        public NotebookPagesQueryHandler(MnemosyneContext context, AuthorizationHandler authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<List<NotebookPage>> HandleAsync(GetNotebookPages request)
        {
            if (!_authService.IsAuthorized(request.User, request.NotebookId, AuthorizationPolicies.NotebookOwner)) return new List<NotebookPage>();

            List<NotebookPage> pages = await _context.NotebookPages
                .Where(x => x.NotebookId == request.NotebookId)
                .Select(x => new NotebookPage
                (
                    x.NotebookPageId,
                    x.Created,
                    x.Updated,
                    x.PageNumber,
                    x.Title,
                    x.Contents
                ))
                .ToListAsync();

            return pages;
        }
    }
}
