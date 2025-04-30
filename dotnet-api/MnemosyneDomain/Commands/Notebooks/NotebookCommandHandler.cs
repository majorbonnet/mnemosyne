using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MnemosyneDomain.Authorization;
using MnemosyneDomain.Events;
using MnemosyneDomain.Models;
using MnemosyneDomain.Repositories;

namespace MnemosyneDomain.Commands.Notebooks
{
    public class NotebookCommandHandler
    {
        private readonly IRepository<Notebook> _repository;
        private readonly IAuthorizationHandler _authService;
        public NotebookCommandHandler(
            IRepository<Notebook> _repository,
            IAuthorizationHandler authService)
        {
            this._repository = _repository;
            _authService = authService;
        }

        public async Task<NotebookCreated> HandleAsync(CreateNotebook request)
        {
            Notebook newNotebook = new()
            {
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                UserId = request.User.UserId
            };

            await _repository.AddAsync(newNotebook);

            return new NotebookCreated(
                request.User,
                newNotebook.NotebookId,
                newNotebook.Created,
                newNotebook.Updated
            );
        }

        public async Task HandleAsync(DeleteNotebook request)
        {
            if (!await _authService.IsAuthorizedAsync(request.User, request.NotebookId, AuthorizationPolicies.NotebookOwner)) return;

            await _repository.DeleteAsync(new Notebook { NotebookId = request.NotebookId });
        }
    }
}
