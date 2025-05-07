using Microsoft.EntityFrameworkCore;
using MnemosyneDomain.Authorization;

namespace MnemosyneDomain.Queries.Pages
{
    public class PageQueryHandler
    {
        private readonly MnemosyneContext _context;
        private readonly IAuthorizationHandler _authService;

        public PageQueryHandler(MnemosyneContext context, IAuthorizationHandler authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<List<Page>> HandleAsync(GetPages request, CancellationToken cancellationToken = default)
        {
            if (!await _authService.IsAuthorizedAsync(request.User, request.NotebookId, AuthorizationPolicies.NotebookOwner)) return new List<Page>();

            List<Page> pages = await _context.Pages
                .Where(p => p.NotebookId == request.NotebookId)
                .Select(x => new Page
                (
                    x.PageId,
                    x.Created,
                    x.Updated,
                    x.PageNumber,
                    x.Title,
                    x.Contents
                ))
                .ToListAsync(cancellationToken);

            return pages;
        }
    }
}
