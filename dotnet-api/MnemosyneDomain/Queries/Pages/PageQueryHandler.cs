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
                .OrderBy(p => p.PageNumber)
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

        public async Task<Page?> HandleAsync(GetPage request, CancellationToken cancellationToken = default)
        {
            if (!await _authService.IsAuthorizedAsync(request.User, request.NotebookId, AuthorizationPolicies.NotebookOwner)) return null;

            if (await _context.Pages.FindAsync(request.PageId, cancellationToken) is Models.Page page)
            {
                return new Page
                (
                    page.PageId,
                    page.Created,
                    page.Updated,
                    page.PageNumber,
                    page.Title,
                    page.Contents
                );
            }

            return null;
        }

        public async Task<List<Page>> HandleAsync(SearchPages request, CancellationToken cancellationToken = default)
        {
            List<Page> pages = new();

            if (string.IsNullOrWhiteSpace(request.Query))
            {
                return pages;
            }

            if (request.IsExactMatch)
            {
                string query = request.Query.Replace("\"", "");

                pages = await _context.Pages
                    .Where(p => p.Notebook.UserId == request.User.UserId
                        && ((p.Notebook.Title != null
                                && p.Notebook.Title.Contains(query))
                           || (p.Title != null
                                && p.Title.Contains(query))
                           || (p.Contents != null
                                && p.Contents.Contains(query))))
                    .OrderBy(p => p.PageNumber)
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

            }
            else
            {
                pages = await _context.Pages
                    .Where(p => p.Notebook.UserId == request.User.UserId
                        && ((p.Notebook.SearchText != null
                                && p.Notebook.SearchText.Matches(EF.Functions.PlainToTsQuery(request.Query)))
                           || (p.SearchText != null
                                && p.SearchText.Matches(EF.Functions.PlainToTsQuery(request.Query)))))
                    .OrderBy(p => p.PageNumber)
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
            }

            return pages;
        }
    }
}
