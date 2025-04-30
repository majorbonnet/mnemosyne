using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MnemosyneDomain.Authorization;
using MnemosyneDomain.Events;
using MnemosyneDomain.Models;
using MnemosyneDomain.Repositories;

namespace MnemosyneDomain.Commands.NotebookPages
{
    public class NotebookPageCommandHandler
    {
        private readonly IRepository<NotebookPage> _repository;
        private readonly IAuthorizationHandler _authService;

        public NotebookPageCommandHandler(IRepository<NotebookPage> repository, IAuthorizationHandler authService)
        {
            _repository = repository;
            _authService = authService;
        }

        public async Task<NotebookPageCreated?> HandleAsync(CreateNotebookPage request)
        {
            if (!await _authService.IsAuthorizedAsync(request.User, request.NotebookId, AuthorizationPolicies.NotebookOwner)) return null;

            int existingPageCount = await _repository.CountAsync(x => x.NotebookId == request.NotebookId);

            NotebookPage page = new()
            {
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                NotebookId = request.NotebookId,
                NotebookPageId = Guid.NewGuid(),
                PageNumber = existingPageCount
            };

            await _repository.AddAsync(page);

            return new NotebookPageCreated(
                request.User,
                request.NotebookId,
                page.NotebookPageId,
                page.PageNumber
            );
        }

        public async Task HandleAsync(DeleteNotebookPage request)
        {
            if (!await _authService.IsAuthorizedAsync(request.User, request.NotebookId, AuthorizationPolicies.NotebookOwner)) return;

            await _repository.DeleteAsync(new NotebookPage {  NotebookPageId = request.NotebookPageId });
        }

        public async Task HandleAsync(UpdateNotebookPage request)
        {
            if (!await _authService.IsAuthorizedAsync(request.User, request.NotebookId, AuthorizationPolicies.NotebookOwner)) return;

            NotebookPage? page = await _repository.FindOneAsync(x => x.NotebookPageId == request.NotebookPageId);

            if (page is not null)
            {
                page.Contents = request.Contents;
                page.Title = request.Title;
                page.Updated = DateTime.UtcNow;

                await _repository.UpdateAsync(page);
            }
        }
    }
}
