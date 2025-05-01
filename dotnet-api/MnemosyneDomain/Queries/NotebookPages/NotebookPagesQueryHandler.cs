using MnemosyneDomain.Authorization;

namespace MnemosyneDomain.Queries.NotebookPages
{
    public class NotebookPagesQueryHandler
    {
        private readonly MnemosyneContext _context;
        private readonly IAuthorizationHandler _authService;

        public NotebookPagesQueryHandler(MnemosyneContext context, IAuthorizationHandler authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<List<NotebookPage>> HandleAsync(GetNotebookPages request)
        {
            if (!await _authService.IsAuthorizedAsync(request.User, request.NotebookId, AuthorizationPolicies.NotebookOwner)) return new List<NotebookPage>();

            List<NotebookPage> pages = _context.NotebookPages
                .Where(p => p.NotebookId == request.NotebookId)
                .Select(x => new NotebookPage
                (
                    x.NotebookPageId,
                    x.Created,
                    x.Updated,
                    x.PageNumber,
                    x.Title,
                    x.Contents
                ))
                .ToList();

            return pages;
        }
    }
}
