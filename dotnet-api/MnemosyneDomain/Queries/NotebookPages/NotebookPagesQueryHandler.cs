using MnemosyneDomain.Authorization;
using MnemosyneDomain.Repositories;

namespace MnemosyneDomain.Queries.NotebookPages
{
    public class NotebookPagesQueryHandler
    {
        private readonly IRepository<Models.NotebookPage> _repository;
        private readonly IAuthorizationHandler _authService;
        public NotebookPagesQueryHandler(IRepository<Models.NotebookPage> repository, IAuthorizationHandler authService)
        {
            _repository = repository;
            _authService = authService;
        }

        public async Task<List<NotebookPage>> HandleAsync(GetNotebookPages request)
        {
            if (!await _authService.IsAuthorizedAsync(request.User, request.NotebookId, AuthorizationPolicies.NotebookOwner)) return new List<NotebookPage>();

            List<NotebookPage> pages = _repository.Find(p => p.NotebookId == request.NotebookId)
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
