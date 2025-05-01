using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MnemosyneDomain.Authorization;
using MnemosyneDomain.Commands.NotebookPages;
using MnemosyneDomain.Events;
using MnemosyneDomain.Models;
using Wolverine;

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

        public async Task<NotebookCreated> HandleAsync(CreateNotebook request)
        {
            Notebook newNotebook = new()
            {
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                UserId = request.User.UserId
            };

            _context.Notebooks.Add(newNotebook);
            await _context.SaveChangesAsync();

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

            _context.Notebooks.Remove(new Notebook { NotebookId = request.NotebookId });
            await _context.SaveChangesAsync();
        }
    }
}
