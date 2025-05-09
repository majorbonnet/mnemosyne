using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MnemosyneDomain.Authorization;
using MnemosyneDomain.Commands.Pages;
using MnemosyneDomain.Events;
using MnemosyneDomain.Models;
using Wolverine;
using Wolverine.Attributes;

namespace MnemosyneDomain.Commands.Notebooks
{
    public class NotebookCommandHandler
    {
        private readonly MnemosyneContext _context;
        private readonly IAuthorizationHandler _authService;

        public NotebookCommandHandler(MnemosyneContext context, IAuthorizationHandler authService)
        {
            _context = context;
            _authService = authService;
        }

        [AlwaysPublishResponse]
        public async Task<NotebookCreated> HandleAsync(CreateNotebook request, CancellationToken cancellationToken = default)
        {
            int existingNotebookCount = _context.Notebooks.Count(x => x.UserId == request.User.UserId);

            Notebook newNotebook = new()
            {
                NotebookId = Guid.NewGuid(),
                Title = $"Notebook {existingNotebookCount + 1}",
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                UserId = request.User.UserId,
            };

            _context.Notebooks.Add(newNotebook);
            await _context.SaveChangesAsync(cancellationToken);

            return new NotebookCreated(
                request.User,
                newNotebook.NotebookId,
                newNotebook.Created,
                newNotebook.Updated
            );
        }

        public async Task HandleAsync(DeleteNotebook request, CancellationToken cancellationToken = default)
        {
            if (!await _authService.IsAuthorizedAsync(request.User, request.NotebookId, AuthorizationPolicies.NotebookOwner)) return;

            _context.Notebooks.Remove(new Notebook { NotebookId = request.NotebookId });
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
