using MnemosyneDomain.Authorization;
using MnemosyneDomain.Repositories;

namespace MnemosyneDomain.Queries.NotebookPages
{
    public class NotebookPagesQueryHandler
    {
        private readonly INotebookPageRepository _repository;
        private readonly IAuthorizationHandler _authService;
        public NotebookPagesQueryHandler(INotebookPageRepository repository, IAuthorizationHandler authService)
        {
            _repository = repository;
            _authService = authService;
        }

        public async Task<List<NotebookPage>> HandleAsync(GetNotebookPages request)
        {
            if (!_authService.IsAuthorized(request.User, request.NotebookId, AuthorizationPolicies.NotebookOwner)) return new List<NotebookPage>();

            List<NotebookPage> pages = (await _repository.GetPagesByNotebookIdAsync(request.NotebookId))
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
